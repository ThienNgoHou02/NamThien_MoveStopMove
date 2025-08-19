using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGameplay : UICanvas
{
    [Header("Joystick")]
    [SerializeField] private Joystick _joystick;

    [Header("Buttons")]
    [SerializeField] private Button _settingButton;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _aliveAmountText;

    public void Setting()
    {
        Close(0);
        UIManager.Instance.OpenUI<CanvasSetting>();
    }
    public void SetAliveAmountText(int alive)
    {
        _aliveAmountText.text = $"Alive: {alive}";
    }
    public Joystick Joystick
    {
        get { return _joystick; }
        set { _joystick = value; }
    }
}
