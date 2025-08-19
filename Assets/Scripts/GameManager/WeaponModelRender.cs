using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModelRender : MonoBehaviour
{
    private Dictionary<Weapon, Transform> _WeaponRenders = new Dictionary<Weapon, Transform>();
    private Weapon _renderring;

    [SerializeField] private Transform[] _weaponModelsTF;
    private void Awake()
    {
        _renderring = Weapon.Axe;
        _WeaponRenders[_renderring] = _weaponModelsTF[(int)_renderring];
        if (!_WeaponRenders[_renderring].gameObject.activeSelf)
        {
            _WeaponRenders[_renderring].gameObject.SetActive(true);
        }
    }
    public void WeaponChange(Weapon wp)
    {
        if (_renderring != wp)
        {
            if (!_WeaponRenders.ContainsKey(wp))
            {
                _WeaponRenders[wp] = _weaponModelsTF[(int)wp];
            }
            _WeaponRenders[_renderring].gameObject.SetActive(false);
            _renderring = wp;
            _WeaponRenders[_renderring].gameObject.SetActive(true);
        }
    }
}
