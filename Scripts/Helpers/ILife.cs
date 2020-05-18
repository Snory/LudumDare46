using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILife
{

    int MaxLifeForce { get; set; }
    int CurrentLifeForce { get; set; }

    int MaxLifeForceUsage { get; set; }

    int Health { get; set; }

    int MaxHealth { get; set; }

    void UseLifeForce(int countOfLifeForce);

    void TransferAllLifeForceTo(ILife targetLifeForce);

    void OnCombatResult(CombatResult result);
}
