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
    private EnemySpawner spawner;
    public bool IsDead => currentHP <= 0;

    private void Start()
    {
        EnemyAnimationData.Initialize();
        currentHP = EnemyData.MaxHP;
        currentTarget = null;
        Animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        StateMachine = new EnemyStateMachine();
        StateMachine.ChangeState(new EnemyIdleState(this, StateMachine));
    }

    void Update()
    {
        StateMachine.Update();
    }

    public void Init(EnemySpawner spawner, Vector3 spawnPosition)
    {
        this.spawner = spawner;
        currentHP = EnemyData.MaxHP;
        EnemyData.CurHP = EnemyData.MaxHP;
        currentTarget = null;

        if (Agent == null)
            Agent = GetComponent<NavMeshAgent>();

        if (Animator == null)
            Animator = GetComponentInChildren<Animator>();

        if (StateMachine == null)
        {
            StateMachine = new EnemyStateMachine();
        }

        Agent.Warp(spawnPosition);
        StateMachine.ChangeState(new EnemyIdleState(this, StateMachine));
        gameObject.SetActive(true);
    }

    public void TakeDamage(float damage)
    {
        currentTarget = Player.Instance.transform;

        Vector3 direction = (currentTarget.position - transform.position).normalized;
        direction.y = 0f;
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
        {
            if (IsInAttackRange())
            {
                Agent.isStopped = true;
            }
            else
            {
                Agent.isStopped = false;
                Agent.SetDestination(currentTarget.position);
            }
        }
    }

    public bool HasTarget() => currentTarget != null;

    public bool IsInAttackRange()
    {
        return currentTarget != null && Vector3.Distance(transform.position, currentTarget.position) <= 2.5f;
    }

    private void Die()
    {
        Player.Instance.AddReward(EnemyData.rewardGold, EnemyData.rewardExperience, EnemyData.dropItem);
        Player.Instance.currentTarget = null;
        gameObject.SetActive(false);
        spawner.ReturnToPool(this);
    }
}
