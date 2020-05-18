using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : Singleton<HUDManager>
{


    [SerializeField]
    Text _gameOver, _maxSpawnLane, _currentSpawnLane, _gameWon, _gameEnd;

    [SerializeField]
    Image _healthBar, _background;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowGameOver()
    {
        _background.enabled = true;
        _gameOver.enabled = true;
        HUDActionPanelManager.Instance.HideActionCanvas();
        GameManager.Instance.UpdateState(GameManager.GameState.PAUSED);
    }


    public void SetHealthBar(float value)
    {
        _healthBar.fillAmount = value;
    }


    public void SetMaxSpawnLane(int maxSpawnLane)
    {
        _maxSpawnLane.text = maxSpawnLane.ToString();
    }

    public void SetCurrentSpawnLane(int currentSpawnLane)
    {
        _currentSpawnLane.text = currentSpawnLane.ToString();
    }

    public void ShowWinGame()
    {
        _background.enabled = true;
        _gameWon.enabled = true;
        HUDActionPanelManager.Instance.HideActionCanvas();
    }

    public void ShowEndGame()
    {
        _background.enabled = true;
        _gameEnd.enabled = true;
        HUDActionPanelManager.Instance.HideActionCanvas();
    }
}
