using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    private Transform _weaponModelTF;
    [SerializeField] private PoolType _pool;
    private Character _character;
    private Collider _collider;

    public void WeaponOwner(Character owner, Collider collider)
    {
        _character = owner;
        _collider = collider;
    }
    public void Throw(Vector3 direction, Vector3 size, Transform muzzle)
    {
        _weaponModelTF.gameObject.SetActive(false);
        Projectile prj = SimplePool.PopFromPool<Projectile>(_pool, muzzle.position, Quaternion.identity);
        prj.transform.localScale = size;
        prj.Throw(direction, _collider, _character);
    }
    public void Reload()
    {
        _weaponModelTF.gameObject.SetActive(true);
    }
}
