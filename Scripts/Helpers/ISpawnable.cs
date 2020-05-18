using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnable 
{

    float SpawnChance { get; set; }

    float FromSpawnLanePercentage { get; set; }

}
