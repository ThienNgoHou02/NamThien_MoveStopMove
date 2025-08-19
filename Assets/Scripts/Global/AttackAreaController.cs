using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaController : MonoBehaviour
{
    private Character _character;
    private Collider _collider;
    private string OBSTACLE = "Obstacle";
    
    public void AttackAreaInit(Character owner, Collider collider)
    {
        _character = owner;
        _collider = collider;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other != _collider && !other.CompareTag(OBSTACLE))
            _character.AddTarget(other);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other != _collider && !other.CompareTag(OBSTACLE))
            _character.RemoveTarget(other);
    }
    public void AreaSizeUp()
    {
        transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
    }
}
