using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private string PLAYER = "Player";
    private Material _original;
    [SerializeField]
    private Material _faded;
    private MeshRenderer _mesh;
    private void Awake()
    {
        _mesh = GetComponent<MeshRenderer>();
        _original = _mesh.material;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER))
        {
            _mesh.material = _faded;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PLAYER))
        {
            _mesh.material = _original;
        }
    }
}
