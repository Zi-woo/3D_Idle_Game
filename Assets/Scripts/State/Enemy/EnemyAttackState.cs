using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private float attackCooldown = 1f;
    private float timer;

    public EnemyAttackState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        enemy.Animator.SetBool(enemy.EnemyAnimationData.AttackParameterHash, true);
        timer = attackCooldown;
    }

    public override void Update()
    {
        Vector3 direction = (enemy.transform.position - Player.Instance.transform.position).normalized;
        direction.y = 0f;
        Player.Instance.transform.rotation = Quaternion.LookRotation(direction);
        if (enemy.IsDead)
        {
            stateMachine.ChangeState(new EnemyDieState(enemy, stateMachine));
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            // 공격 타이밍 (ex: 데미지 처리 또는 애니메이션 이벤트 활용)
            timer = attackCooldown;
        }

        if (!enemy.IsInAttackRange())
        {
            stateMachine.ChangeState(new EnemyMoveState(enemy, stateMachine));
        }
        
    }

    public override void Exit()
    {
        enemy.Animator.SetBool(enemy.EnemyAnimationData.AttackParameterHash, false);
    }
}
