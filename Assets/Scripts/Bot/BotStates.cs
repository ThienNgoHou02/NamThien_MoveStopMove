using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotRevive : IState<BotController>
{
    private int IDLE = Animator.StringToHash("isIdling");
    private float untargetTimer;
    public void OnEnter(BotController t)
    {
        t.AnimatorSetBool(IDLE, true);
        t._characterModelTF.localScale = Vector3.one;
        t._characterCanvas.transform.localPosition = t._canvasStartPosition;
        t.projectileSize = Vector3.one;
        t._ranking = 0;
        t.SetRankText(0);
        untargetTimer = 0;
    }

    public void OnExecute(BotController t)
    {
        if (untargetTimer < 1.0f)
        {
            untargetTimer += Time.deltaTime;
        }
        else
            t.ChangeState(new BotIlde());
    }

    public void OnExit(BotController t)
    {
        t.AnimatorSetBool(IDLE, false);
        if (!t.Collider.enabled)
            t.Collider.enabled = true;
    }
}
public class BotIlde : IState<BotController>
{
    private int IDLE = Animator.StringToHash("isIdling");
    public void OnEnter(BotController t)
    {
        t.AnimatorSetBool(IDLE, true);
    }

    public void OnExecute(BotController t)
    {
        //Nếu chưa có mục tiêu, tiến hành tìm đường
        if (!t.IsAttacking)
        {
            t.Target = t.GetTarget();
            if (t.Target)
            {
                t.ChangeState(new BotAttack());
            }
            else 
            {
                t.ChangeState(new BotRun());
            }

        }
    }

    public void OnExit(BotController t)
    {
        t.AnimatorSetBool(IDLE, false);

    }
}
public class BotRun : IState<BotController>
{
    private int RUN = Animator.StringToHash("isRunning");
    private Transform _ToWard;
    public void OnEnter(BotController t)
    {
        t.AnimatorSetBool(RUN, true);
        _ToWard = t.ToWardTarget();
        t.Nav.isStopped = false;
    }

    public void OnExecute(BotController t)
    {
        if (!t.IsAttacking)
        {
            t.Target = t.GetTarget();
            if (t.Target)
            {
                t.ChangeState(new BotAttack());
            }
            else
            {
                if (_ToWard && _ToWard.gameObject.activeSelf)
                    t.Nav.SetDestination(_ToWard.position);
                else
                    _ToWard = t.ToWardTarget();
            }
        }
        else
        { 
            t.ChangeState(new BotIlde()); 
        }
    }

    public void OnExit(BotController t)
    {
        t.AnimatorSetBool(RUN, false);
        t.Nav.isStopped = true;

    }
}
public class BotAttack : IState<BotController>
{
    private int ATTACK = Animator.StringToHash("isAttacking");
    public void OnEnter(BotController t)
    {
        t.AnimatorSetBool(ATTACK, true);
        t.transform.LookAt(t.Target.transform);
    }

    public void OnExecute(BotController t)
    {
        AnimatorStateInfo stateInfo = t.AnimatorStateInfomation(0);

        if (stateInfo.IsName("Attack"))
        {
            if (!t.IsAttacking && stateInfo.normalizedTime >= 0.36f && stateInfo.normalizedTime < 1f)
            {
                t.IsAttacking = true;
                t.Throw();
            }
            else if (stateInfo.normalizedTime >= 1f)
            {
                t.ChangeState(new BotIlde());
            }
        }
    }

    public void OnExit(BotController t)
    {
        t.AnimatorSetBool(ATTACK, false);
    }
}
public class BotDead : IState<BotController>
{
    private int DEAD = Animator.StringToHash("isDead");
    private bool isRevive;
    public void OnEnter(BotController t)
    {
        t.AnimatorSetBool(DEAD, true);
        if (t.Collider.enabled)
            t.Collider.enabled = false;
        t.OnCharacterDead?.Invoke(t.Collider);
        isRevive = false;
    }

    public void OnExecute(BotController t)
    {
        AnimatorStateInfo stateInfo = t.AnimatorStateInfomation(0);
        if (!isRevive && stateInfo.IsName("Dead") && stateInfo.normalizedTime >= 1f)
        {
            isRevive = true;
            if (LevelManager.Instance.CanRevive())
            {
                LevelManager.Instance.CharacterRevive(t);
                t.ChangeState(new BotRevive());
            }
            else
            {
                t.gameObject.SetActive(false);
            }
        }
    }   

    public void OnExit(BotController t)
    {
        t.AnimatorSetBool(DEAD, false);
    }
}
