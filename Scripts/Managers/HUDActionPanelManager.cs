using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDActionPanelManager : Singleton<HUDActionPanelManager>
{
    [SerializeField]
    Canvas _actionCanvas;

    public bool _open;

    int _attNumInt, _decNumInt,  _playerAvailableLifeForceInt;

    /** player panel **/
    [SerializeField]
    Text _attNumText, _decNumText, _enemyLifeForceText, _enemyCubeUsageText, _playerAvailableLifeForceText, _playerLifeText, _enemyHealthText;

    [SerializeField]
    Text _playerAttackResult, _playerDefenseResult, _enemyAttackResult, _enemyDefenseResult;

    [SerializeField]
    GameObject _playerResult, _enemyResult;

    

    ILife _playerLifeForce, _enemyLifeForce;
    Transform _player, _enemy;
    EnemyDiceAI _enemyDiceAI;
    Coroutine _showRoutine;

    // Start is called before the first frame update
    void Start()
    {
        if (_actionCanvas == null)
        {
            Debug.LogError($"[HUDManager]: missing _actionCanvas");
        }

        _actionCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HideActionCanvas();
        }
    }

    private IEnumerator StartWaitBeforeDissapear()
    {

        ShowActionCanvasFor(_player.gameObject, _enemy.gameObject, true);
        yield return new WaitForSeconds(0.3f);
        _actionCanvas.enabled = false;
        GameManager.Instance.UpdateState(GameManager.GameState.RUNNING);
    }

    private void StopWaitBeforeDissapear()
    {
        if (_showRoutine != null)
        {
            StopCoroutine(_showRoutine);
            _open = false;
        }
    }


    public void ShowResult(bool show)
    {
        _playerResult.SetActive(show);
        _enemyResult.SetActive(show);
    }

    public void SetCombatResult(CombatResult player, CombatResult enemy)
    {
        _playerAttackResult.text = player.AttackSum.ToString();
        _playerDefenseResult.text = player.DefenseSum.ToString();
        _enemyAttackResult.text = enemy.AttackSum.ToString();
        _enemyDefenseResult.text = enemy.DefenseSum.ToString();
    }
    

    public void UpdateActionPanel()
    {
        if (_open) { 
            SetPlayerLife(_playerLifeForce.Health);
            SetPlayerAvailableLifeForce(_playerLifeForce.CurrentLifeForce);
            SetEnemyLifeForce(_enemyLifeForce.CurrentLifeForce);
            SetMaxLifeForceUsage(_enemyLifeForce.MaxLifeForceUsage);
            SetEnemyHealth(_enemyLifeForce.Health);
        }
    }

    public void RunCombat()
    {

        if (_enemy.gameObject.layer == 0)
        {
            return;
        }
        AudioManager.Instance.ButtonClick();
        StopWaitBeforeDissapear();
        //ShowResult(true);
        //player request
        CombatRequest playerCombatRequest = new CombatRequest(_player, _attNumInt,_decNumInt,_playerLifeForce.OnCombatResult);
        //enemyrequest
        EnemyDiceThrow aiThrow = _enemyDiceAI.GetEnemyDiceThrow(_playerLifeForce);
        CombatRequest enemyCombatRequest = new CombatRequest(_enemy, aiThrow.AttackDiceCount, aiThrow.DefenseDiceCount, _enemyLifeForce.OnCombatResult);
        //combatmanager
        CombatManager.Instance.RequestCombat(playerCombatRequest, enemyCombatRequest);
        _showRoutine = StartCoroutine(StartWaitBeforeDissapear());
    }




    public void ShowActionCanvasFor(GameObject player, GameObject enemy, bool afterCombat)
    {



        if(GameManager.Instance.CurrentGameState == GameManager.GameState.PAUSED)
        {
            return;
        }
        

        if (enemy == null)
        {
            Debug.Log("[HUDmanager]: missing enemy object");
        }

        this._playerLifeForce = player.GetComponent<ILife>();
        _player = player.transform;
        if(this._playerLifeForce == null)
        {
            Debug.Log("[HUDmanager]: missing player component");
            return;
        }
        this._enemyLifeForce = enemy.GetComponentInParent<ILife>();
        _enemy = enemy.transform;
        if (this._enemyLifeForce == null)
        {
            Debug.Log("[HUDmanager]: missing enemy component");
        }
        this._enemyDiceAI = enemy.GetComponentInParent<EnemyDiceAI>();

        if (afterCombat)
        {
            ShowResult(true);
            if(enemy.gameObject.layer == 0)
            {
                return;
            }
        }
        else
        {
            ShowResult(false);
        }

        GameManager.Instance.UpdateState(GameManager.GameState.ACTION); 
        _actionCanvas.enabled = true;
        _open = true;
        //ShowResult(false);
        SetAttack(0);
        SetDefense(0);
        UpdateActionPanel();

    }


    public void SetEnemyHealth(int health)
    {
        _enemyHealthText.text = health.ToString();
    }

    public void SetMaxLifeForceUsage(int maxLifeFore)
    {
        _enemyCubeUsageText.text = maxLifeFore.ToString();
    }

    public void SetEnemyLifeForce(int lifeForce)
    {
        _enemyLifeForceText.text = lifeForce.ToString();
    }

    public void SetPlayerAvailableLifeForce(int lifeForce)
    {
        _playerAvailableLifeForceInt = lifeForce;
        _playerAvailableLifeForceText.text = $"{_playerAvailableLifeForceInt}";

    }

    public void SetPlayerLife(int life)
    {
        _playerLifeText.text = life.ToString();
    }

    public void HideActionCanvas()
    {
        _actionCanvas.enabled = false;
        _open = false;
        GameManager.Instance.UpdateState(GameManager.GameState.RUNNING);
    }



    public void SetAttack(int attack)
    {
        _attNumInt = attack;
        _attNumText.text = $"{_attNumInt}";

    }

    public void SetDefense(int defense)
    {
        _decNumInt = defense;
        _decNumText.text = $"{_decNumInt}";

    }

    public void AddAvailableLifeForce()
    {
        int _availableForceTemp = _playerAvailableLifeForceInt + 1;

        if ((_availableForceTemp > _playerLifeForce.CurrentLifeForce))
        {
            return;
        }
        _playerAvailableLifeForceInt += 1;
        SetPlayerAvailableLifeForce(_playerAvailableLifeForceInt);

    }

    public void SubAvailableLifeForce()
    {
        _playerAvailableLifeForceInt -= 1;
        if (_playerAvailableLifeForceInt < 0)
        {
            _playerAvailableLifeForceInt = 0;
        }
        SetPlayerAvailableLifeForce(_playerAvailableLifeForceInt);
    }

    public void AddAttack()
    {
        AudioManager.Instance.ButtonClick();
        StopWaitBeforeDissapear();
        int _attNumIntTemp = _attNumInt + 1;

        if ((_attNumIntTemp + _decNumInt) > _playerLifeForce.CurrentLifeForce)
        {
            return;
        }
        SubAvailableLifeForce();
        _attNumInt += 1;
        SetAttack(_attNumInt);
    }

    public void SubAttack()
    {
        AudioManager.Instance.ButtonClick();
        StopWaitBeforeDissapear();
        if(_attNumInt > 0) AddAvailableLifeForce();

        _attNumInt -= 1;
        if(_attNumInt < 0)
        {
            _attNumInt = 0;
        }
        
        SetAttack(_attNumInt);
    }

    public void AddDefense()
    {
        AudioManager.Instance.ButtonClick();
        StopWaitBeforeDissapear();
        int _decNumIntTemp = _decNumInt + 1;

        if ((_decNumIntTemp + _attNumInt) > _playerLifeForce.CurrentLifeForce)
        {
            return;
        }
        _decNumInt += 1;
        SubAvailableLifeForce();
        SetDefense(_decNumInt);
    }

    public void SubDefense()
    {
        AudioManager.Instance.ButtonClick();
        StopWaitBeforeDissapear();
        if (_decNumInt > 0) AddAvailableLifeForce();
        _decNumInt -= 1;
        if (_decNumInt <= 0)
        {
            _decNumInt = 0;

        }
        else
        {

        }
        SetDefense(_decNumInt);
    }

    public bool isActionCanvasOn()
    {
        return _actionCanvas.enabled;
    }



}
