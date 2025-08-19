using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : Character, IDamage
{
    IState<BotController> cState;
    NavMeshAgent _nmAgent;
    List<Character> _targetTowards = new List<Character>();

    Coroutine crtAttackReload;
    bool isAttacking;

    Quaternion fixedRotation;
    
    public Vector3 _canvasStartPosition;
    public Action<Character> OnDead;
    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _nmAgent = GetComponent<NavMeshAgent>();

        projectileSize = new Vector3(1, 1, 1);
        _canvasStartPosition = _characterCanvas.transform.localPosition;

        _attackAreaController.AttackAreaInit(this, _collider);
        _ringTF.gameObject.SetActive(false);

        fixedRotation = _characterCanvas.transform.rotation;
        _audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        _targetTowards = LevelManager.Instance.OnStage;
        ChangeState(new BotIlde());
    }
    private void Update()
    {
        cState?.OnExecute(this);
    }
    private void LateUpdate()
    {
        _characterCanvas.transform.rotation = fixedRotation;
    }
    private IEnumerator AttackReload(float timeCD)
    {
        yield return new WaitForSeconds(timeCD);
        isAttacking = false;
        weaponController.Reload();

    }
    public void ChangeState(IState<BotController> nState)
    {
        cState?.OnExit(this);
        cState = nState;
        cState?.OnEnter(this);
    }
    public void AnimatorSetBool(int hash, bool flag)
    {
        animator.SetBool(hash, flag);
    }
    public AnimatorStateInfo AnimatorStateInfomation(int layer)
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(layer);
        return state;
    }
    public Transform ToWardTarget()
    {
        Transform target = null;
        if (_targetTowards.Count > 1)
        {
            while (true)
            {
                int index = UnityEngine.Random.Range(0, _targetTowards.Count);
                if (_targetTowards[index] != this)
                {
                    target = _targetTowards[index].transform;
                    break;
                }
            }

        }
        return target;
    }
    public void Throw()
    {
        weaponController.Throw(transform.forward, projectileSize, muzzleTF);
        if (crtAttackReload != null)
        {
            StopCoroutine(crtAttackReload);
        }
        crtAttackReload = StartCoroutine(AttackReload(attackSpeed));
    }
    public Collider Collider 
    { 
        get { return _collider; } 
        set { _collider = value; }
    }
    public Character Target
    {
        get { return targetLocked; }
        set { targetLocked = value; }
    }
    public NavMeshAgent Nav
    {
        get { return _nmAgent; }
        set { _nmAgent = value; }
    }
    public bool IsAttacking
    {
        get { return isAttacking; }
        set { isAttacking = value; }
    }
    public void Damage(Character damager, Vector3 position)
    {
        OnDead.Invoke(this);
        damager.SizeUp();
        damager.ProjectSizeUp();
        damager.CanvasUp();
        _attackAreaController.AreaSizeUp();
        VFX blood = SimplePool.PopFromPool<VFX>(PoolType.Hit, position, Quaternion.identity);
        blood.SetColor(_color);
        ChangeState(new BotDead());
    }
}
