using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField]
    float _speed = 1f;

    [SerializeField]
    Rigidbody2D _body;


    [SerializeField]
    EnemyAttack _enemyAttack;

    private bool _targetReached;
    private bool _runningAway;

    public bool TargetReached {  get { return _targetReached; } set { _targetReached = value; } }


    // Start is called before the first frame update
    void Start()
    {
        _runningAway = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void MoveToTarget(Transform targetTransform)
    {

        Vector2 currentPosition = new Vector2(this.transform.position.x, this.transform.position.y);
        Vector2 targetPosition = new Vector2(targetTransform.position.x, targetTransform.position.y);


        Vector3 tempScale = transform.localScale;

        if (currentPosition.x > targetPosition.x)
        {
            if (tempScale.x > 0)
            {
                tempScale.x *= -1;
            }

        }
        else if (currentPosition.x < targetPosition.x)
        {
            if (tempScale.x < 0)
            {
                tempScale.x *= -1;
            }
        }
        transform.localScale = tempScale;


        if ((targetPosition - currentPosition).sqrMagnitude < _enemyAttack.ReachTreshold)
        {
            TargetReached = true;
            return;
        }else
        {
            TargetReached = false;
        }
        
        transform.position = Vector2.MoveTowards(this.transform.position, targetTransform.position, _speed * Time.fixedDeltaTime);
    }

    public void RunAway()
    {
        transform.position = this.transform.position;
        _body.velocity = new Vector3(3, 0, 0);
        this.gameObject.layer = 0;
        _runningAway = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Enemy" && _runningAway)
        {
            foreach(Collider2D col in this.transform.GetComponentsInChildren<BoxCollider2D>())
            {
                Physics2D.IgnoreCollision(collision.collider, col);
            }
        }
    }
}
