using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    private Transform followTF;
    
    [SerializeField] private Transform cameraTF;
    [SerializeField] private float followingSpeed;
    Vector3 _Camera;

    private void Awake()
    {
        instance = this;
    }
    private void LateUpdate()
    {
        Vector3 follow = Vector3.SmoothDamp(transform.position, followTF.position, ref _Camera, followingSpeed * Time.deltaTime);
        transform.position = follow;
    }
    public void CameraViewUp()
    {
        cameraTF.localScale += new Vector3(0.2f, 0.2f, 0.2f);
    }
    public Transform Follow
    {
        get { return followTF; }
        set { followTF = value; }
    }
}
