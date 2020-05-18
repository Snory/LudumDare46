using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDiceAI : MonoBehaviour
{


    EnemyStats _stats;

    [SerializeField]
    float rationAttackToDefense;


    [SerializeField]
    float weightOfPlayerUsage;

    

    private void Awake()
    {
        _stats = this.GetComponent<EnemyStats>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public EnemyDiceThrow GetEnemyDiceThrow(ILife _playerLife)
    {

        //basic distrubution
        int countToAttack = Mathf.RoundToInt((_stats.MaxLifeForceUsage) * rationAttackToDefense);
        int countToDefense = _stats.MaxLifeForceUsage - countToAttack;


        return new EnemyDiceThrow(countToAttack, countToDefense);
     
    }
}


public struct EnemyDiceThrow
{
    public EnemyDiceThrow(int attackDiceCount, int defenseDiceCount)
    {
        AttackDiceCount = attackDiceCount;
        DefenseDiceCount = defenseDiceCount;
    }

    public int AttackDiceCount { get; }
    public int DefenseDiceCount { get; }
}
