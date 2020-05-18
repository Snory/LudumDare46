using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{

    [SerializeField]
    GameObject _menuImage, _infoImage;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ShowInfo()
    {

        _menuImage.SetActive(false);
        _infoImage.SetActive(true);
    }

    public void ShowMenu()
    {
        _menuImage.SetActive(true);
        _infoImage.SetActive(false);
    }

    public void StartLevel(string levelname)
    {
        GameManager.Instance.StartLevel(levelname);
    }
}
