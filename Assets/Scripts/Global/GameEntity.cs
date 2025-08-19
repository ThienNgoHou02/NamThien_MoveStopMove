using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntity : MonoBehaviour
{
    public PoolType poolType;

    private Transform tf;
    public Transform TF
    {
        get 
        {
            if (!tf)
                tf = transform;
            return tf;
        }
    }
}
