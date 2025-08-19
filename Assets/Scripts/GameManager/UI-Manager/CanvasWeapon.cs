using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Weapon
{
    Axe = 0,
    Boomerang = 1,
    Candy = 2,
    Hammer = 3,
    Knife = 4,
    SphereCandy = 5,
    Uzi = 6,
    ZShape = 7
}
public class CanvasWeapon : UICanvas
{
    WeaponModelRender _wpRenderer;
    ChangeEquipments _equipmentChanger;

    [Header("Buttons")]
    [SerializeField] private Button _preButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _purchaseButton;
    [SerializeField] private Button _equipButton;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _weaponName;
    [SerializeField] private TextMeshProUGUI _goldAmount;
    [SerializeField] private Transform _equippedText;

    [Header("Data")]
    [SerializeField] private PlayerData _playerData;

    private eWeapons _selectedWeapon;
    private int index = 0;
    private void Awake()
    {
        _wpRenderer = GameObject.FindObjectOfType<WeaponModelRender>();
        _equipmentChanger = GameObject.FindObjectOfType<ChangeEquipments>();

        _preButton.onClick.AddListener(() => ChangeWeapon(-1));
        _nextButton.onClick.AddListener(() => ChangeWeapon(1));

        _purchaseButton.onClick.AddListener(WeaponPurchase);
        _equipButton.onClick.AddListener(WeaponEquip);
    }
    private void OnEnable()
    {
        _goldAmount.text = _playerData._gold.ToString();
        _selectedWeapon = (eWeapons)index;

        UpdateButtons();
    }
    private void ChangeWeapon(int addIndex)
    {
        index += addIndex;
        if (index > 7)
            index = 0;
        if (index < 0)
            index = 7;
        _wpRenderer?.WeaponChange((Weapon)index);
        _selectedWeapon = (eWeapons)index;
        _equipmentChanger.ChangeWeapon(_selectedWeapon);
        _weaponName.text = _selectedWeapon.ToString();

        UpdateButtons();
    }
    public void WeaponPurchase()
    {
        if (_selectedWeapon != eWeapons.None)
        {
            if (!_playerData._purchasedWeapons.Contains(_selectedWeapon) && _playerData._gold >= 250)
            {
                _playerData._purchasedWeapons.Add(_selectedWeapon);
                _purchaseButton.gameObject.SetActive(false);
                _equipButton.gameObject.SetActive(true);
            }
        }
    }
    public void WeaponEquip()
    {
        if (_selectedWeapon != eWeapons.None)
        {
            if (_playerData._purchasedWeapons.Contains(_selectedWeapon) && _playerData._weapon != _selectedWeapon)
            {
                _playerData._weapon = _selectedWeapon;
                _equipButton.gameObject.SetActive(false);
                _equippedText.gameObject.SetActive(true);
            }
        }
    }
    public void UpdateButtons()
    {
        if (_selectedWeapon != eWeapons.None)
        {
            bool equipped = _playerData._weapon == _selectedWeapon;
            bool purchased = _playerData._purchasedWeapons.Contains(_selectedWeapon);

            _equippedText.gameObject.SetActive(equipped);
            _purchaseButton.gameObject.SetActive(!equipped && !purchased);
            _equipButton.gameObject.SetActive(!equipped && purchased);
        }
    }
    public void BackOnClick()
    {
        _equipmentChanger.ChangeEquipped();
        Close(0);
        UIManager.Instance.OpenUI<CanvasMenu>();
    }
}
