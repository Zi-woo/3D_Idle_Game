using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : EnemyBaseState
{
    public EnemyMoveState(Enemy enemy, EnemyStateMachine stateMachine)
        : base(enemy, stateMachine) { }

    public override void Enter()
    {
        enemy.Animator.SetBool(enemy.EnemyAnimationData.MoveParameterHash, true);
    }

    public override void Update()
    {
        if (enemy.IsDead)
        {
            stateMachine.ChangeState(new EnemyDieState(enemy, stateMachine));
            return;
        }

        if (!enemy.HasTarget())
        {
            stateMachine.ChangeState(new EnemyIdleState(enemy, stateMachine));
            return;
        }
        
        if (enemy.IsInAttackRange())
        {
            stateMachine.ChangeState(new EnemyAttackState(enemy, stateMachine));
        }
        
        enemy.MoveToTarget();

        
    }

    public override void Exit()
    {
        enemy.Animator.SetBool(enemy.EnemyAnimationData.MoveParameterHash, false);
    }
}
