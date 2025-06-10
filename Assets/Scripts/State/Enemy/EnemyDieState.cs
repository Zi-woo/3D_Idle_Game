using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : EnemyBaseState
{
    public EnemyDieState(Enemy enemy, EnemyStateMachine stateMachine)
        : base(enemy, stateMachine) { }

    public override void Enter()
    {
        enemy.Animator.SetBool(enemy.EnemyAnimationData.DieParameterHash, true);
        enemy.Agent.isStopped = true;
    }

    public override void Update() { }

    public override void Exit()
    {
        enemy.Animator.SetBool(enemy.EnemyAnimationData.DieParameterHash, false);
    }
}
