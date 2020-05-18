using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{


    EnemyMovement _enenyMovement;

    [SerializeField]
    EnemyStats _enemyStats;

    [SerializeField]
    EnemyAttack _enemyAttack;

    [SerializeField]
    string _tagToFollow;
    Transform _targetTransform;

    [SerializeField]
    Animator _anim;

    ILife _targetILife;

    private void Awake()
    {
        _enenyMovement = this.GetComponent<EnemyMovement>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _anim.SetBool("walking", true);
        _targetTransform = GameObject.FindWithTag(_tagToFollow).transform;
        _targetILife = _targetTransform.GetComponent<ILife>();
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    private void FixedUpdate()
    {

 
        if (!_enemyStats.RanAway) { 
            _enenyMovement.MoveToTarget(_targetTransform);
            if (_enenyMovement.TargetReached)
            {
                _anim.SetBool("walking", false);
                _anim.SetBool("attacking", true);
                _enemyAttack.Attack(_targetILife);

            } else
            {
                _anim.SetBool("walking", true);
                _anim.SetBool("attacking", false);
                _enemyAttack.StopAttack();
            }
        }
        else
        {
            _enenyMovement.RunAway();
        }
    }

    public void SetTargetToFollow(Transform targetToFollow)
    {
        _targetTransform = targetToFollow;
        _targetILife = _targetTransform.GetComponent<ILife>();
    }
}
