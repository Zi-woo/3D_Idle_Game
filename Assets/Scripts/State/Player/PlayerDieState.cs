using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieState : PlayerBaseState
{
    public PlayerDieState(Player player, PlayerStateMachine stateMachine)
        : base(player, stateMachine) { }

    public override void Enter()
    {
        player.Animator.SetBool(player.PlayerAnimationData.DieParameterHash, true);
        player.Agent.isStopped = true;
    }

    public override void Update() { }

    public override void Exit()
    {
        player.Animator.SetBool(player.PlayerAnimationData.DieParameterHash, false);
    }
}
