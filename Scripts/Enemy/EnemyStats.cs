using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour, ILife, ISpawnable
{

    [SerializeField]
    private int _maxLifeForce, _life, _maxLifeForceUsage, _maxHealth;

    private int _currentLifeForce;


    [SerializeField]
    private GameObject _lifeForceBallPrefab;

    [SerializeField]
    private float _spawnChance;

    [SerializeField]
    private float _fromPercentageSpawn;

    public int MaxLifeForce { get { return _maxLifeForce; } set { _maxLifeForce = value; } }
    public int CurrentLifeForce { get { return _maxLifeForce; } set { _maxLifeForce = value; } }

    public int Health { get { return _life; } set { _life = value; } }

    public int MaxLifeForceUsage { get { return _maxLifeForceUsage; } set { _maxLifeForceUsage = value; } }

    public float SpawnChance { get { return _spawnChance; } set { _spawnChance = value; } }

    private bool _ranAway;

    public bool RanAway { get { return _ranAway; } }

    public int MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }

    public float FromSpawnLanePercentage { get { return _fromPercentageSpawn; } set { _fromPercentageSpawn = value; } }


    // Start is called before the first frame update
    void Start()
    {
        _currentLifeForce = _maxLifeForce;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseLifeForce(int countOfLifeForce)
    {
        _currentLifeForce -= countOfLifeForce;
    }


    public void Death()
    {
        Destroy(this.gameObject);
    }


    public void RunAway()
    {
        _ranAway = true;
    }

    public void TransferAllLifeForceTo(ILife targetLifeForce)
    {
        targetLifeForce.CurrentLifeForce += this.CurrentLifeForce;

    }

    public void OnCombatResult(CombatResult result)
    {
        this.CurrentLifeForce -= result.UsedLifeForce;
     
        if (!result.DefenseWon)
        {
            this.Health -= result.LifeDamage;
            if (this.Health <= 0) { 
                this.TransferAllLifeForceTo(result.OpponentLifeForce.GetComponent<ILife>());
            }
        } else
        {
            if(this.CurrentLifeForce <= this.MaxLifeForceUsage)
            {
                RunAway();
            }
        }

        Transform spell = Instantiate(_lifeForceBallPrefab, this.transform.position, Quaternion.identity).transform;
        spell.GetComponent<SpellFollowTarget>().Cast(result.OpponentLifeForce.transform);
        spell.transform.parent = GameObject.Find("=== spells ===").transform;

    }
}
