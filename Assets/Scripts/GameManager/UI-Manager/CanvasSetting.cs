using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasSetting : UICanvas
{
    [Header("Button")]
    [SerializeField] private Button _returnButton;
    [SerializeField] private Button _menuButton;

    private void Awake()
    {
        _returnButton.onClick.AddListener(ReturnGame);
        _menuButton.onClick.AddListener(ReturnMenu);
    }
    private void OnEnable()
    {
        if (Time.timeScale > 0)
        {
            Time.timeScale = 0;
        }
    }
    public void ReturnGame()
    {
        if (Time.timeScale < 1)
        {
            Time.timeScale = 1;
        }
        Close(0);
        UIManager.Instance.OpenUI<CanvasGameplay>();
    }
    public void ReturnMenu()
    {
        Close(0);
        SceneManager.LoadScene("StartScene");
    }
}
