using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGameStart : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.OpenUI<CanvasMenu>();
    }
}
