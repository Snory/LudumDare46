using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, ILife
{
    [SerializeField]
    private int _maxLifeForce, _currentLifeForce, _life, _maxLife, lifeForceTreshold;




    [SerializeField]
    private GameObject _lifeForceBallPrefab;

    public int MaxLifeForce { get { return _maxLifeForce; } set { _maxLifeForce = value;} }
    public int CurrentLifeForce { get { return _currentLifeForce; } 
            set { 
                if (value <= (MaxLifeForce + lifeForceTreshold)) {
                    _currentLifeForce = value;
                    AudioManager.Instance.LifeForceGained();
            }  
            } 
    }

    public int Health { get { return _life; }
        set { 
            _life = value;
            HUDManager.Instance.SetHealthBar(Health / (float)MaxHealth);
            if (_life <= 0)
            {
                HUDManager.Instance.ShowGameOver();
                GameManager.Instance.UpdateState(GameManager.GameState.PAUSED);
            }


        }
    }


    public int MaxLifeForceUsage { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int MaxHealth { get { return _maxLife; } set { _maxLife = value; } }

    public void OnCombatResult(CombatResult result)
    {

        this._currentLifeForce -= result.UsedLifeForce;
        if (!result.DefenseWon)
        {
            Health += result.LifeDamage;
        }

        Debug.Log($"[PlayerStats]: Life = {Health}; LifeForce = {CurrentLifeForce} and it was {GameManager.Instance.LastEnemy} last enemy.");
        if(Health <= 0 || CurrentLifeForce <= 0)
        {
            HUDManager.Instance.ShowGameOver();
            GameManager.Instance.UpdateState(GameManager.GameState.PAUSED);
        } else { 
            Transform spell = Instantiate(_lifeForceBallPrefab, this.transform.position, Quaternion.identity).transform;
            SpellFollowTarget spellFollow = spell.GetComponent<SpellFollowTarget>();
            spellFollow.Cast(result.OpponentLifeForce.transform);
            spellFollow.EnemyKilledEv.AddListener(OnEnemyKilled);
            spell.transform.parent = GameObject.Find("=== spells ===").transform;
            
        }

        if(GameManager.Instance.LastEnemy && GameManager.Instance.WithoutEnemy())
        {
            if(_currentLifeForce >= _maxLifeForce) { 
                HUDManager.Instance.ShowWinGame();
            } else
            {
                HUDManager.Instance.ShowEndGame();
            }
            GameManager.Instance.UpdateState(GameManager.GameState.PAUSED);
        }

    }

    private void OnEnemyKilled(bool arg0)
    {
 
        if(GameManager.Instance.LastEnemy)
        {
            if(_currentLifeForce >= _maxLifeForce) { 
                HUDManager.Instance.ShowWinGame();
            } else
            {
                HUDManager.Instance.ShowEndGame();
            }
            GameManager.Instance.UpdateState(GameManager.GameState.PAUSED);
        }
    }

    public void TransferAllLifeForceTo(ILife targetLifeForce)
    {
        throw new System.NotImplementedException();
    }

    public void UseLifeForce(int countOfLifeForce)
    {
        _currentLifeForce -= countOfLifeForce; ;
    }

    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = Health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
