using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private PoolDetail[] PoolSetups;

    private void Awake()
    {
        for (int i = 0; i < PoolSetups.Length; i++)
        {
            SimplePool.PreLoad(PoolSetups[i].prefab, PoolSetups[i].parent, PoolSetups[i].amount);
        }
    }
}
[System.Serializable]
public class PoolDetail
{
    public GameEntity prefab;
    public Transform parent;
    public int amount;
}
public enum PoolType
{
    None=0,
    Hammer = 1,
    Axe = 2,
    Uzi = 3,
    Knife = 4,
    Candy = 5,
    Boomerang = 6,
    ZShape = 7,
    SphereCandy = 8,
    Hit = 9
}
