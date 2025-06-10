using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    private float scanInterval = 0.5f;
    private float timer;
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine)
        : base(player, stateMachine) { }

    public override void Enter()
    {
        player.Animator.SetBool(player.PlayerAnimationData.IdleParameterHash, true);
        timer = scanInterval;
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
            player.FindNearestEnemy();
            timer = scanInterval;
        }

        if (player.HasTarget())
        {
            stateMachine.ChangeState(new PlayerMoveState(player, stateMachine));
        }
    }

    public override void Exit()
    {
        player.Animator.SetBool(player.PlayerAnimationData.IdleParameterHash, false);
    }
}
