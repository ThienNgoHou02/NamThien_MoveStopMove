using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Shop
{
    Hair = 0,
    Pant = 1,
    Shield = 2,
    Skin = 3
}
public class CanvasShop : UICanvas
{
    ChangeEquipments _equipmentChanger;

    private Shop tab;
    private Dictionary<Shop, Transform> _ShopTabs = new Dictionary<Shop, Transform>();
    private Dictionary<Shop, Image> _ButtonImages = new Dictionary<Shop, Image>();

    [Header("Tabs")]
    [SerializeField] private Transform[] _shopTabsTF;

    [Header("Buttons")]
    [SerializeField] private Button _hairShopButton;
    [SerializeField] private Button _pantShopButton;
    [SerializeField] private Button _shieldShopButton;
    [SerializeField] private Button _skinShopButton;
    [Space]
    [SerializeField] private Button _purchaseButton;
    [SerializeField] private Button _equipButton;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _goldAmount;
    [SerializeField] private Transform _equippedText;

    [Header("Colors")]
    [SerializeField] private Color _clickedColor;
    [SerializeField] private Color _unclickedColor;

    [Header("Data")]
    [SerializeField] private EquipmentData _equipmentData;
    [SerializeField] private PlayerData _playerData;

    private ShopItem _currentItem;

    private void Awake()
    {
        _equipmentChanger = GameObject.FindObjectOfType<ChangeEquipments>();

        _hairShopButton.onClick.AddListener(() => ShopSelection(Shop.Hair));
        _ButtonImages[Shop.Hair] = _hairShopButton.GetComponent<Image>();

        _pantShopButton.onClick.AddListener(() => ShopSelection(Shop.Pant));
        _ButtonImages[Shop.Pant] = _pantShopButton.GetComponent<Image>();
        
        _shieldShopButton.onClick.AddListener(() => ShopSelection(Shop.Shield));
        _ButtonImages[Shop.Shield] = _shieldShopButton.GetComponent<Image>();
        
        _skinShopButton.onClick.AddListener(() => ShopSelection(Shop.Skin));
        _ButtonImages[Shop.Skin] = _skinShopButton.GetComponent<Image>();

        _purchaseButton.onClick.AddListener(ItemPurchase);
        _equipButton.onClick.AddListener(ItemEquip);
    }
    private void OnEnable()
    {
        _goldAmount.text = _playerData._gold.ToString();
        tab = Shop.Hair;
        if (!_ShopTabs.ContainsKey(tab))
        {
            _ShopTabs[tab] = _shopTabsTF[(int)tab];
        }
        _ShopTabs[tab].gameObject.SetActive(true);
        _ButtonImages[tab].color = _clickedColor;
    }
    private void OnDisable()
    {
        if (!_ShopTabs.ContainsKey(tab))
        {
            _ShopTabs[tab] = _shopTabsTF[(int)tab];
        }
        _ShopTabs[tab].gameObject.SetActive(false);
        _ButtonImages[tab].color = _unclickedColor;
    }
    public void ShopSelection(Shop shop)
    {
        if (tab != shop)
        {
            if (!_ShopTabs.ContainsKey(shop))
            {
                _ShopTabs[shop] = _shopTabsTF[(int)shop];
            }
            _ShopTabs[tab].gameObject.SetActive(false);
            _ButtonImages[tab].color = _unclickedColor;
            tab = shop;
            _ShopTabs[tab].gameObject.SetActive(true);
            _ButtonImages[tab].color = _clickedColor;

            if (_purchaseButton.gameObject.activeSelf)
            {
                _purchaseButton.gameObject.SetActive(false);
            }
        }
    }
    public void HairItemOnclick(ShopItem item, eHairs hair, int price)
    {
        if (!_currentItem)
        {
            _currentItem = item;
            _currentItem.EnablePointer();
        }
        if (_currentItem != item)
        {
            _currentItem.DisablePointer();
            _currentItem = item;
            _currentItem.EnablePointer();
        }
        _equipmentChanger.ChangeHair(hair);

        bool equipped = _playerData._hair == hair;
        bool purchased = _playerData._purchasedHairs.Contains(hair);

        if (!purchased)
        {
            _priceText.text = price.ToString();
        }
        _purchaseButton.gameObject.SetActive(!equipped && !purchased);
        _equipButton.gameObject.SetActive(!equipped && purchased);
        _equippedText.gameObject.SetActive(equipped);

    }    
    public void PantItemOnclick(ShopItem item, ePants pant, int price)
    {
        if (!_currentItem)
        {
            _currentItem = item;
            _currentItem.EnablePointer();
        }
        if (_currentItem != item)
        {
            _currentItem.DisablePointer();
            _currentItem = item;
            _currentItem.EnablePointer();
        }
        _equipmentChanger.ChangePant(_equipmentData.pantData[(int)pant]);

        bool equipped = _playerData._pant == pant;
        bool purchased = _playerData._purchasedPants.Contains(pant);

        if (!purchased)
        {
            _priceText.text = price.ToString();
        }
        _purchaseButton.gameObject.SetActive(!equipped && !purchased);
        _equipButton.gameObject.SetActive(!equipped && purchased);
        _equippedText.gameObject.SetActive(equipped);
    }    
    public void ShieldItemOnclick(ShopItem item, eShields shield, int price)
    {
        if (!_currentItem)
        {
            _currentItem = item;
            _currentItem.EnablePointer();
        }
        if (_currentItem != item)
        {
            _currentItem.DisablePointer();
            _currentItem = item;
            _currentItem.EnablePointer();
        }
        _equipmentChanger.ChangeShield(shield);

        bool equipped = _playerData._shield == shield;
        bool purchased = _playerData._purchasedShields.Contains(shield);

        if (!purchased)
        {
            _priceText.text = price.ToString();
        }
        _purchaseButton.gameObject.SetActive(!equipped && !purchased);
        _equipButton.gameObject.SetActive(!equipped && purchased);
        _equippedText.gameObject.SetActive(equipped);

    }    
    public void SkinItemOnclick(eSkins skin)
    {
        
    }
    public void ItemPurchase()
    {
        if (_currentItem)
        {
            if (_playerData._gold >= _currentItem._itemPrice)
            {
                bool _canPurchase = false;
                switch (_currentItem._itemTab)
                {
                    case Shop.Hair:
                        if (!_playerData._purchasedHairs.Contains(_currentItem._hairItem))
                        {
                            _playerData._hair = _currentItem._hairItem;
                            _playerData._purchasedHairs.Add(_currentItem._hairItem);
                            _canPurchase = true;    
                        }
                        break;
                    case Shop.Pant:
                        if (!_playerData._purchasedPants.Contains(_currentItem._pantItem))
                        {
                            _playerData._pant = _currentItem._pantItem;
                            _playerData._purchasedPants.Add(_currentItem._pantItem);
                            _canPurchase = true;
                        }
                        break;
                    case Shop.Shield:
                        if (!_playerData._purchasedShields.Contains(_currentItem._shieldItem))
                        {
                            _playerData._shield = _currentItem._shieldItem;
                            _playerData._purchasedShields.Add(_currentItem._shieldItem);
                        }
                        break;
                    case Shop.Skin:
                        break;
                }
                if (_canPurchase)
                {
                    _playerData._gold -= _currentItem._itemPrice;
                    _goldAmount.text = _playerData._gold.ToString();
                    _purchaseButton.gameObject.SetActive(false);
                    _equipButton.gameObject.SetActive(true);
                }
            }
        }
    }
    public void ItemEquip()
    {
        if (_currentItem)
        {
            bool _equipped = false;
            switch (_currentItem._itemTab)
            {
                case Shop.Hair:
                    if (_playerData._purchasedHairs.Contains(_currentItem._hairItem))
                    {
                        _playerData._hair = _currentItem._hairItem;
                        _equipmentChanger.ChangeHair(_playerData._hair);
                        _equipped = true;
                    }
                    break;
                case Shop.Pant:
                    if (_playerData._purchasedPants.Contains(_currentItem._pantItem))
                    {
                        _playerData._pant = _currentItem._pantItem;
                        _equipmentChanger.ChangePant(_equipmentData.pantData[(int)_playerData._pant]);
                        _equipped = true;
                    }
                    break;
                case Shop.Shield:
                    if (_playerData._purchasedShields.Contains(_currentItem._shieldItem))
                    {
                        _playerData._shield = _currentItem._shieldItem;
                        _equipmentChanger.ChangeShield(_playerData._shield);
                        _equipped = true;
                    }
                    break;
                case Shop.Skin:
                    break;
            }
            if (_equipped)
            {
                _equipButton.gameObject.SetActive(false);
                _equippedText.gameObject.SetActive(true);
            }
        }
    }
    public void BackOnClick()
    {
        if (_currentItem)
        {
            _currentItem.DisablePointer();
        }    
        _equipmentChanger.ChangeEquipped();
        Close(0);
        UIManager.Instance.OpenUI<CanvasMenu>();
    }
}
