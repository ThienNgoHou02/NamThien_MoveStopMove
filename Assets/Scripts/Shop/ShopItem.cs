using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    private Button _button;

    public CanvasShop _canvasShop;

    public Transform _selectPointer;

    public Shop _itemTab;
    public eHairs _hairItem;
    public ePants _pantItem;
    public eShields _shieldItem;
    public eSkins _skinItem;

    public int _itemPrice;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ItemOnClick);

    }
    public void ItemOnClick()
    {
        switch (_itemTab)
        {
            case Shop.Hair:
                _canvasShop.HairItemOnclick(this, _hairItem, _itemPrice);
                break;
            case Shop.Pant:
                _canvasShop.PantItemOnclick(this, _pantItem, _itemPrice);
                break;
            case Shop.Shield:
                _canvasShop.ShieldItemOnclick(this, _shieldItem, _itemPrice);
                break;
            case Shop.Skin: 
                _canvasShop.SkinItemOnclick(_skinItem);
                break;
        }
    }
    public void EnablePointer()
    {
        _selectPointer?.gameObject.SetActive(true);
    }
    public void DisablePointer()
    {
        _selectPointer?.gameObject.SetActive(false);
    }
}
