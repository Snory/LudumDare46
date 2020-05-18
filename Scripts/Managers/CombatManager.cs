using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{

    public void RequestCombat(CombatRequest playerRequest, CombatRequest enemyRequest)
    {


        ILife enemyLifeForce = enemyRequest.Combatan.GetComponent<ILife>();
        ILife playerLifeForce = playerRequest.Combatan.GetComponent<ILife>();

        int attackPlayer = DiceManager.Instance.GetSumOfXThrownDice(playerRequest.AttackCountDice);
        int defenseEnemy = DiceManager.Instance.GetSumOfXThrownDice(enemyRequest.DefenseCountDice);

        int attackEnemy = DiceManager.Instance.GetSumOfXThrownDice(enemyRequest.AttackCountDice);
        int defensePlayer = DiceManager.Instance.GetSumOfXThrownDice(playerRequest.DefenseCountDice);




        //compare thown against player
        int attackDiff = attackPlayer - defenseEnemy;
        int deffenceDiff = defensePlayer - attackEnemy;


       

        CombatResult playerCombatResult = 
                new CombatResult(
                      attackDiff > 0 && attackEnemy > 0 ? true : false
                    , deffenceDiff >= 0 ? true : false
                    , playerRequest.AttackCountDice + playerRequest.DefenseCountDice
                    , deffenceDiff
                    , attackPlayer
                    , defensePlayer
                    , enemyRequest.Combatan
                );

        CombatResult enemyCombatResult =
              new CombatResult(
                    deffenceDiff < 0 ? true : false
                  , (attackDiff <= 0 && attackEnemy > 0) || attackEnemy == 0 ? true : false
                  , enemyRequest.AttackCountDice + enemyRequest.DefenseCountDice
                  , attackDiff
                  , attackEnemy
                  , defenseEnemy
                  , playerRequest.Combatan
              );

        HUDActionPanelManager.Instance.SetCombatResult(playerCombatResult, enemyCombatResult);

        playerRequest.CallBack(playerCombatResult);
        enemyRequest.CallBack(enemyCombatResult);


    }


}


public struct CombatRequest
{
    public Transform Combatan { get; }
    public int AttackCountDice { get; }
    public int DefenseCountDice { get; }
    public Action<CombatResult> CallBack { get; }

       
    public CombatRequest(Transform combatan, int attackCountDie, int defenseCountDie, Action<CombatResult> callBack)
    {
        this.Combatan = combatan;
        this.AttackCountDice = attackCountDie;
        this.DefenseCountDice = defenseCountDie;
        this.CallBack = callBack;
    }

}

public struct CombatResult
{

    public CombatResult(bool attackWon, bool defenseWon, int usedLifeForce, int lifeDamage, int attackSum, int DefenseSum, Transform opponentLifeForce)
    {
        AttackWon = attackWon;
        DefenseWon = defenseWon;
        UsedLifeForce = usedLifeForce;
        LifeDamage = lifeDamage;
        AttackSum = attackSum;
        this.DefenseSum = DefenseSum;
        OpponentLifeForce = opponentLifeForce;
    }

    public bool AttackWon { get; }
    public bool DefenseWon { get; }
    public int UsedLifeForce { get; }
    public int LifeDamage { get; }
    public int AttackSum { get; }
    public int DefenseSum { get; }
    public Transform OpponentLifeForce { get; }
}
