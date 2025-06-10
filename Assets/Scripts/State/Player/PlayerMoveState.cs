using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine)
        : base(player, stateMachine) { }

    public override void Enter()
    {
        player.Animator.SetBool(player.PlayerAnimationData.MoveParameterHash, true);
    }

    public override void Update()
    {
        if (player.IsDead)
        {
            stateMachine.ChangeState(new PlayerDieState(player, stateMachine));
            return;
        }

        if (!player.HasTarget())
        {
            stateMachine.ChangeState(new PlayerIdleState(player, stateMachine));
            return;
        }

        player.MoveToTarget();

        if (player.IsInAttackRange())
        {
            stateMachine.ChangeState(new PlayerAttackState(player, stateMachine));
        }
    }

    public override void Exit()
    {
        player.Animator.SetBool(player.PlayerAnimationData.MoveParameterHash, false);
    }
}
