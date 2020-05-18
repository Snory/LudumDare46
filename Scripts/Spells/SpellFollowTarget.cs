using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyKilledEvent : UnityEvent<bool> { }; //delegate?
public class SpellFollowTarget : MonoBehaviour
{
    float _spellSpeed = 5f;
    Transform _targetTransform;

    public EnemyKilledEvent EnemyKilledEv;
   
    // Start is called before the first frame update
    void Start()
    {
       
        Destroy(this.gameObject, 10f);
    }

    private void Update()
    {
        if(GameManager.Instance.CurrentGameState != GameManager.GameState.PAUSED) { 
            Rotate();
            Move();
        }
    }

    // Update is called once per frame
    public void Cast(Transform targetTransform)
    {
        _targetTransform = targetTransform;
        EnemyKilledEv = new EnemyKilledEvent();
        
    }

    private void Move()
    {
        if(_targetTransform != null) { 
            Vector2 currentPosition = new Vector2(this.transform.position.x, this.transform.position.y);
            Vector2 targetPosition = new Vector2(_targetTransform.position.x, _targetTransform.position.y);
            transform.position = Vector2.MoveTowards(this.transform.position, _targetTransform.position, _spellSpeed * Time.fixedDeltaTime);
        }
    }

    private void Rotate()
    {
        Vector3 spellToTarget = _targetTransform.position - this.transform.position;



        //Quaternion rotationFromPlayerToMouse = Quaternion.LookRotation(playerToMouse);
        //_body.MoveRotation(rotationFromPlayerToMouse);

        var angle = Mathf.Atan2(spellToTarget.y, spellToTarget.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ILife life = collision.transform.GetComponentInParent<ILife>();
      

        if (life != null && collision.transform.parent == _targetTransform.transform)
        {
            if((life.Health <= 0 || life.CurrentLifeForce <= 0) && collision.tag == "Enemy")
            {
                Destroy(collision.transform.parent.gameObject);
                EnemyKilledEv.Invoke(true);
            }
            
            if(collision.tag == "Enemy" || collision.tag == "Player") {
                this.gameObject.SetActive(false);
            
            }
        }

    }
}
