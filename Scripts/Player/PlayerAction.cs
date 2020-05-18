using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{

    LayerMask _actionableMask;
    ILife _playerLifeForce;

    // Start is called before the first frame update
    void Start()
    {
        _actionableMask = LayerMask.GetMask(Tags.LAYER_ACTIONALBE);
        _playerLifeForce = this.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShowActionPanel();
        }
    }


    public void ShowActionPanel()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(camRay.origin, camRay.direction, Mathf.Infinity, _actionableMask);

        if (hit)
        {
            if (!HUDActionPanelManager.Instance.isActionCanvasOn()) { 
                HUDActionPanelManager.Instance.ShowActionCanvasFor(this.gameObject, hit.transform.gameObject, false);
            }
        }
    }




}
