using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private float scanInterval = 0.5f;
    private float timer;
    public EnemyIdleState(Enemy enemy, EnemyStateMachine stateMachine)
        : base(enemy, stateMachine) { }

    public override void Enter()
    {
        enemy.Animator.SetBool(enemy.EnemyAnimationData.IdleParameterHash, true);
        timer = scanInterval;
    }

    public override void Update()
    {
        if (enemy.IsDead)
        {
            stateMachine.ChangeState(new EnemyDieState(enemy, stateMachine));
            return;
        }

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = scanInterval;
        }

        if (enemy.HasTarget())
        {
            if (enemy.IsInAttackRange())
            {
                stateMachine.ChangeState(new EnemyAttackState(enemy, stateMachine));
            }
            else
            {
                stateMachine.ChangeState(new EnemyMoveState(enemy, stateMachine));
            }
        }
    }

    public override void Exit()
    {
        enemy.Animator.SetBool(enemy.EnemyAnimationData.IdleParameterHash, false);
    }
}
