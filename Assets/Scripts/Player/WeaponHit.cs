using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!(Player.Instance.StateMachine.CurrentState is PlayerAttackState))
        {
            return;
        }
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy == null || enemy.IsDead) return;
    
        enemy.TakeDamage(Player.Instance.PlayerStatusData.attack);
    }
    
}
