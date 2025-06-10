using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float currentHP;
    public Animator Animator { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public EnemyStateMachine StateMachine { get; private set; }
    [field: SerializeField] public EnemyData EnemyData { get; private set; }
    [field: SerializeField] public EnemyAnimationData EnemyAnimationData { get; private set; }
    public Transform currentTarget;
    public bool IsDead => currentHP <= 0;

    private void Start()
    {
        EnemyAnimationData.Initialize();
        currentHP = EnemyData.CurHP;
        currentTarget = null;
        Animator = GetComponentInChildren<Animator>();
        StateMachine = new EnemyStateMachine();
        Agent = GetComponent<NavMeshAgent>();
        
        StateMachine.ChangeState(new EnemyIdleState(this, StateMachine));
    }
    void Update()
    {
        StateMachine.Update();
    }

    public void TakeDamage(float damage)
    {
        currentTarget = Player.Instance.transform;
        // 고개 돌리기
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        direction.y = 0f; // y축 회전 방지 (바닥면만 바라보게)
        transform.rotation = Quaternion.LookRotation(direction);


        if (IsDead) return;
        
        if (damage > EnemyData.defense)
        {
            currentHP -= damage;
        }

        if (IsDead)
        {
            Die();
        }
    }
    public void MoveToTarget()
    {
        if (currentTarget != null)
            Agent.SetDestination(currentTarget.position);
    }

    public bool HasTarget()
    {
        return currentTarget != null;
    }
    public bool IsInAttackRange()
    {
        return currentTarget != null && Vector3.Distance(transform.position, currentTarget.position) < 2.5f;
    }
   
    private void Die()
    {
        Player.Instance.AddReward(EnemyData.rewardGold, EnemyData.rewardExperience, EnemyData.dropItem);
        Destroy(gameObject);
    }
}