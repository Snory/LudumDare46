using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    Coroutine _attackingRoutine;

    [SerializeField]
    float _attackSpeead;

    [SerializeField]
    int dmgDeal;

    [SerializeField]
    float reachTreshold;

    public float ReachTreshold { get { return reachTreshold; } set { reachTreshold = value; } }

    ILife _health;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(ILife healt)
    {
        _health = healt;
        if(_attackingRoutine == null) { 
            _attackingRoutine = StartCoroutine(StartAttacking());
        }
    }


    private IEnumerator StartAttacking()
    {
        yield return new WaitForSeconds(_attackSpeead);
        _health.Health -= dmgDeal;
        HUDActionPanelManager.Instance.UpdateActionPanel();
        StartCoroutine(StartAttacking());
    }


    public void StopAttack()
    {
        if(_attackingRoutine != null) { 
            StopCoroutine(_attackingRoutine);
        }
    }






}
