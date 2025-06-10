using System;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public Animator Animator { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }
    [field: SerializeField] public PlayerData  PlayerStatusData { get; private set; }
    [field: SerializeField] public PlayerAnimationData  PlayerAnimationData { get; private set; }

    public Transform currentTarget;
    public ItemData itemData;
    public Action <ItemData>addItem;
    public bool IsDead { get; private set; }

    [SerializeField] private float detectionRadius = 100f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        PlayerAnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        StateMachine = new PlayerStateMachine();
        
        StateMachine.ChangeState(new PlayerIdleState(this, StateMachine));
    }

    void Update()
    {
        StateMachine.Update();
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

    public bool HasTarget()
    {
        return currentTarget != null;
    }

    public bool IsInAttackRange()
    {
        return currentTarget != null && Vector3.Distance(transform.position, currentTarget.position) <= 2.5f;
    }

    public void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < shortestDistance && dist <= detectionRadius)
            {
                shortestDistance = dist;
                nearest = enemy.transform;
            }
        }

        currentTarget = nearest;
    }
    public void AddReward(int gold, int experience,ItemData dropItem)
    {
        PlayerStatusData.gold += gold;
        PlayerStatusData.experience += experience;
        addItem?.Invoke(dropItem);
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        while (PlayerStatusData.experience >= PlayerStatusData.level * 100)
        {
            PlayerStatusData.level++;
        }
    }

    public void Heal(float mount)
    {
        PlayerStatusData.CurHP += mount;
        if (PlayerStatusData.CurHP > PlayerStatusData.MaxHP)
        {
            PlayerStatusData.CurHP = PlayerStatusData.MaxHP;
        }
    }
    public void Die()
    {
        if (!IsDead)
        {
            IsDead = true;
            StateMachine.ChangeState(new PlayerDieState(this, StateMachine));
        }
    }
}