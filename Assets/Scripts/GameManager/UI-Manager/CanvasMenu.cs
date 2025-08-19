using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasMenu : UICanvas
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _goldAmountText;
    [SerializeField] private TextMeshProUGUI _playButtonText;

    [Header("Data")]
    [SerializeField] private PlayerData _playerData;
    private void OnEnable()
    {
        _playButtonText.text = $"Play\n{_playerData._nextScene}";
        _goldAmountText.text = _playerData._gold.ToString();
    }
    public void PlayOnClick()
    {
        Close(0);
        SceneManager.LoadScene(_playerData._nextScene);
    }
    public void ShopOnClick()
    {
        Close(0);
        UIManager.Instance.OpenUI<CanvasShop>();
    }
    public void WeaponOnClick()
    {
        Close(0);
        UIManager.Instance.OpenUI<CanvasWeapon>();
    }
}
