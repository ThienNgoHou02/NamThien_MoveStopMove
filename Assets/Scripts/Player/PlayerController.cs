using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : Character, IDamage
{
    CameraController cameraCtrl;
    Joystick joystick;
    private enum States
    {
        Idle = 0,
        Run = 1,
        Attack = 2,
        Dance = 3,
        Dead = 4
    }

    private int IDLE = Animator.StringToHash("isIdling");
    private int RUN = Animator.StringToHash("isRunning");
    private int ATTACK = Animator.StringToHash("isAttacking");
    private int DANCE = Animator.StringToHash("isDancing");
    private int DEAD = Animator.StringToHash("isDead");

    [Header("MOVEMENT")]
    [SerializeField] private Transform raycastTF;
    [SerializeField] private LayerMask planeLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float runSpeed;

    [Header("ROTATE")]
    [SerializeField] private float rotateSpeed;

    States cState;
    Quaternion fixedRotation;
    Vector3 _canvasStartPosition;

    Coroutine crtAttackReload;

    private bool isAttacking;
    private string killer;
    private void Awake()
    {
        _attackAreaController.AttackAreaInit(this, GetComponent<Collider>());
        projectileSize = new Vector3(1, 1, 1);
        _canvasStartPosition = _characterCanvas.transform.position;
        fixedRotation = _characterCanvas.transform.rotation;
        _audioSource = GetComponent<AudioSource>(); 
    }
    private void Start()
    {
        cameraCtrl = CameraController.instance;
        ChangeState(States.Idle);
    }
    private void Update()
    {
        UpdateState();
    }
    private void LateUpdate()
    {
        _characterCanvas.transform.rotation = fixedRotation; 
    }
    private void UpdateState()
    {
        switch (cState)
        {
            case States.Idle:
                OnUpdateIdle();
                break;
            case States.Run: 
                OnUpdateRun();
                break;
            case States.Attack: 
                OnUpdateAttack();
                break;
            case States.Dance:
                OnUpdateDance(); 
                break;
            case States.Dead:
                OnUpdateDead();
                break;
        }
    }
    #region IDLE
    private void OnEnterIdle()
    {
        animator.SetBool(IDLE, true);
    }
    private void OnUpdateIdle()
    {
        if (isJoystickInput())
        {
            ChangeState(States.Run);
        }
        else
        {
            if (!targetLocked)
            {
                targetLocked = GetTarget();
            }
            if (targetLocked && !isAttacking)
            {
                ChangeState(States.Attack);
            }
        }
    }
    private void OnExitIdle()
    {
        animator.SetBool(IDLE, false);
    }
    #endregion
    #region RUN
    private void OnEnterRun()
    {
        animator.SetBool(RUN, true);
    }
    private void OnUpdateRun()
    {
        if (!isJoystickInput())
        {
            if (!targetLocked)
            {
                targetLocked = GetTarget();
            }
            if (targetLocked && !isAttacking)
            {
                ChangeState(States.Attack);
            }
            else
            {
                ChangeState(States.Idle);
            }
        }
        else
        {
            RaycastHit obstacle;
            RaycastHit plane;
            if (Physics.Raycast(raycastTF.position, Vector3.down, out plane, 1f, planeLayer) && 
                !Physics.Raycast(new Vector3(raycastTF.position.x, raycastTF.position.y, raycastTF.position.z - .65f), transform.forward, out obstacle, 1f, obstacleLayer))
            {
                transform.position = Vector3.MoveTowards(transform.position, plane.point, runSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, plane.point) < 0.01f)
                {
                    transform.position = plane.point;
                }
            }
            Vector3 _Rotation = joystick.Horizontal * Vector3.right + joystick.Vertical * Vector3.forward;
            _Rotation.Normalize();

            if (_Rotation != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_Rotation, Vector3.up);
                Quaternion playerRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                transform.rotation = playerRotation;
            }
        }
    }
    private void OnExitRun()
    {
        animator.SetBool(RUN, false);
    }
    #endregion
    #region ATTACK
    private void OnEnterAttack()
    {
        animator.SetBool(ATTACK, true);
        transform.LookAt(targetLocked.transform);
    }    
    private void OnUpdateAttack()
    {
        if (isJoystickInput())
        {
            ChangeState(States.Run);
        }
        else
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Attack"))
            {
                if (!isAttacking && stateInfo.normalizedTime >= 0.36f && stateInfo.normalizedTime < 1f)
                {
                    isAttacking = true;
                    weaponController.Throw(transform.forward, projectileSize, muzzleTF);
                    if (crtAttackReload != null)
                    {
                        StopCoroutine(crtAttackReload);
                    }
                    crtAttackReload = StartCoroutine(AttackReload(attackSpeed));
                }
                else
                if (stateInfo.normalizedTime >= 1f)
                {
                    ChangeState(States.Idle);
                } 
            }              
        }
    }    
    private void OnExitAttack()
    {
        animator.SetBool(ATTACK, false);
    }
    #endregion
    #region DANCE
    private void OnEnterDance()
    {
        animator.SetBool(DANCE, true);
        //transform.position = new Vector3(0, 0, 0);
        transform.Rotate(0, 180, 0);
        _collider.enabled = false;
    }
    private void OnUpdateDance()
    {

    }
    private void OnExitDance()
    {
        animator.SetBool(DANCE, false);
    }
    #endregion
    #region DEAD
    private void OnEnterDead()
    {
        animator.SetBool(DEAD, true);
        _collider.enabled = false;
        OnCharacterDead?.Invoke(_collider);
    }
    private void OnUpdateDead()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Dead") && stateInfo.normalizedTime >= 1f)
        {
            LevelManager.Instance.Defead(killer, _goldReward);
        }
    }
    private void OnExitDead()
    {
        animator.SetBool(DEAD, false);
        _collider.enabled = true;
    }
    #endregion
    private void ChangeState(States nState)
    {
        if (cState != nState)
        {
            switch (cState)
            {
                case States.Idle:
                    OnExitIdle();
                    break;
                case States.Run:
                    OnExitRun();
                    break;
                case States.Attack:
                    OnExitAttack();
                    break;
                case States.Dance:
                    OnExitDance(); 
                    break;
                case States.Dead:
                    OnExitDead(); 
                    break;
            }
            cState = nState;
            switch (cState)
            {
                case States.Idle:
                    OnEnterIdle();
                    break;
                case States.Run:
                    OnEnterRun();
                    break;
                case States.Attack:
                    OnEnterAttack();
                    break;
                case States.Dance:
                    OnEnterDance();
                    break;
                case States.Dead:
                    OnEnterDead();
                    break;
            }
        }
    }
    private IEnumerator AttackReload(float timeCD)
    {
        yield return new WaitForSeconds(timeCD);
        isAttacking = false;
        weaponController.Reload();

    }
    private bool isJoystickInput()
    {
        return joystick.Vertical != 0 || joystick.Horizontal != 0;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(raycastTF.position, new Vector3(raycastTF.position.x, raycastTF.position.y - 1, raycastTF.position.z));
        Gizmos.DrawLine(raycastTF.position, new Vector3(raycastTF.position.x, raycastTF.position.y, raycastTF.position.z + 1));
    }
    public void Damage(Character damager, Vector3 position)
    {
        HitSound();
        killer = damager._name;
        damager.SizeUp();
        damager.ProjectSizeUp();
        damager.CanvasUp();
        _attackAreaController.AreaSizeUp();
        cameraCtrl.CameraViewUp();
        VFX blood = SimplePool.PopFromPool<VFX>(PoolType.Hit, position, Quaternion.identity);
        blood.SetColor(Color.white);
        ChangeState(States.Dead);
    }
    public void Victory()
    {
        ChangeState(States.Dance);
    }
    public Joystick JoyStick
    {
        get { return joystick; }
        set { joystick = value; }
    }
}