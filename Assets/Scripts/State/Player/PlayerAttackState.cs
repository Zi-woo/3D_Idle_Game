using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private float attackCooldown = 1f;
    private float timer;

    public PlayerAttackState(Player player, PlayerStateMachine stateMachine)
        : base(player, stateMachine) { }

    public override void Enter()
    {
        player.Animator.SetBool(player.PlayerAnimationData.AttackParameterHash, true);
        timer = attackCooldown;
    }

    public override void Update()
    {
        if (player.IsDead)
        {
            stateMachine.ChangeState(new PlayerDieState(player, stateMachine));
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = attackCooldown;
        }

        if (!player.IsInAttackRange())
        {
            stateMachine.ChangeState(new PlayerMoveState(player, stateMachine));
        }
    }

    public override void Exit()
    {
        player.Animator.SetBool(player.PlayerAnimationData.AttackParameterHash, false);
    }
}
