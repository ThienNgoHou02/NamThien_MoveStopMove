using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeEquipments : MonoBehaviour
{
    [Header("Equipments")]
    [SerializeField] private SkinnedMeshRenderer _skinRender;
    [SerializeField] private SkinnedMeshRenderer _pantRender;
    [SerializeField] private Transform[] _hairsTF;
    [SerializeField] private Transform[] _weaponsTF;
    [SerializeField] private Transform[] _shieldsTF;
    [SerializeField] private Transform[] _wingsTF;
    [SerializeField] private Transform[] _tailsTF;

/*    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _goldAmount;*/

    [Header("Data")]
    [SerializeField] private EquipmentData _equipmentData;
    [SerializeField] private PlayerData _playerData;

    private Dictionary<eHairs, Transform> _hairTransforms = new Dictionary<eHairs, Transform>();
    private Dictionary<eWeapons, Transform> _weaponTransforms = new Dictionary<eWeapons, Transform>();
    private Dictionary<eShields, Transform> _shieldTransforms = new Dictionary<eShields, Transform>();

    private eHairs _selectedHair = eHairs.None;
    private eWeapons _selectedWeapon = eWeapons.None;
    private eShields _selectedShield = eShields.None;

    private void Awake()
    {
        ChangeEquipped();
    }
    public void ChangeSkin(Material skin)
    {
        _skinRender.material = skin;
    }
    public void ChangeHair(eHairs hair)
    {
        if (hair != eHairs.None)
        {
            if (!_hairTransforms.ContainsKey(hair))
            {
                _hairTransforms[hair] = _hairsTF[(int)hair];
            }    
            if (_selectedHair == eHairs.None)
            {
                _selectedHair = hair;
                _hairTransforms[hair].gameObject.SetActive(true);
            }
            if (_selectedHair != hair)
            {
                _hairTransforms[_selectedHair].gameObject.SetActive(false);
                _selectedHair = hair;
                _hairTransforms[_selectedHair].gameObject.SetActive(true);
            }
        }
        else
        {
            if (_selectedHair != eHairs.None)
            {
                _hairTransforms[_selectedHair].gameObject.SetActive(false);
                _selectedHair = eHairs.None;
            }
        }
    }
    public void ChangePant(Material pant)
    {
        _pantRender.material = pant;
    }
    public void ChangeWeapon(eWeapons weapon)
    {
        if (weapon != eWeapons.None)
        {
            if (!_weaponTransforms.ContainsKey(weapon))
            {
                _weaponTransforms[weapon] = _weaponsTF[(int)weapon];
            }
            if (_selectedWeapon == eWeapons.None)
            {
                _selectedWeapon = weapon;
                _weaponTransforms[weapon].gameObject.SetActive(true);
            }
            if (_selectedWeapon != weapon)
            {
                _weaponTransforms[_selectedWeapon].gameObject.SetActive(false);
                _selectedWeapon = weapon;
                _weaponTransforms[_selectedWeapon].gameObject.SetActive(true);
            }
        }
    }
    public void ChangeShield(eShields shield)
    {
        if (shield != eShields.None)
        {
            if (!_shieldTransforms.ContainsKey(shield))
            {
                _shieldTransforms[shield] = _shieldsTF[(int)shield];
            }
            if (_selectedShield == eShields.None)
            {
                _selectedShield = shield;
                _shieldTransforms[shield].gameObject.SetActive(true);
            }
            if (_selectedShield != shield)
            {
                _shieldTransforms[_selectedShield].gameObject.SetActive(false);
                _selectedShield = shield;
                _shieldTransforms[_selectedShield].gameObject.SetActive(true);
            }
        }
    }
    public void ChangeEquipped()
    {
        ChangeHair(_playerData._hair);
        ChangePant(_equipmentData.pantData[(int)_playerData._pant]);
        ChangeWeapon(_playerData._weapon);
        ChangeShield(_playerData._shield);
    }
}
