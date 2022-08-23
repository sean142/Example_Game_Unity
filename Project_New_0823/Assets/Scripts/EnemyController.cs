using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates { PATROL, CHASE, DEAD }
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    private EnemyStates enemyStates;

    NavMeshAgent agent;
    Animator animator;
    CharacterStats characterStats;

    private GameObject attackTarget; //player����m

    private float speed;

    public float LookAtTime;
    private float remainLookAtTime;
    private float lastAttackTime;

    [Header("Basic Settings")]
    public float sighRadius;//FoundPlayer

    [Header("Patrol State�]���ު��A�]�m�^")]
    public float potralRange;//���ް骺�b�|�j�p
    Vector3 wayPoint;
    Vector3 guardPos;

    public bool isPatrol;
    bool iswalk;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();

        speed = agent.speed;
        guardPos = transform.position;
        remainLookAtTime = LookAtTime;
    }

    private void Start()
    {
        if (isPatrol)
        {
            enemyStates = EnemyStates.PATROL;
            GetNewWayPoint();
            // Debug.Log("test");
        }
    }

    // Update is called once per frame
   
    void Update()
    {
        SwitchStates();
        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
    }

    void SwitchAnimation()
    {
       // animator.SetBool("Walk", iswalk);
    }

    void SwitchStates()
    {
        if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
            //Debug.Log("���player");
        }
        switch (enemyStates)
        {
            case EnemyStates.PATROL:
                agent.speed = speed * 0.5f;

                if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
                {
                    iswalk = false;
                    if (remainLookAtTime > 0)
                        remainLookAtTime -= Time.deltaTime;
                    else
                        GetNewWayPoint();
                }
                else
                {
                    iswalk = true;
                    agent.destination = wayPoint;
                }
                break;
            case EnemyStates.CHASE:
                agent.speed = speed;

                if (!FoundPlayer())
                {
                    iswalk = false;
                    if (remainLookAtTime > 0)
                    {
                        agent.destination = transform.position; //�Ԧ��^��@�Ӫ��A
                        remainLookAtTime -= Time.deltaTime;
                    }

                    else if (isPatrol)
                        enemyStates = EnemyStates.PATROL;
                }
                else
                {
                    iswalk = true;
                    agent.isStopped = false;
                    agent.destination = attackTarget.transform.position;
                }
                if (TargetInAttackRange() || TargetInSkillRange())
                {
                    agent.isStopped = true;

                    if (lastAttackTime < 0)
                    {
                        lastAttackTime = characterStats.attackData.coolDown;

                        //�z���P�_
                        characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;
                        //�������
                        Attack();
                    }
                }

                break;
            case EnemyStates.DEAD:
                break;
        }
    }

    void Attack()
    {
        transform.LookAt(attackTarget.transform);

        if (TargetInAttackRange())
        {
            //TODO �񨭧����ʵe
        }

        if (TargetInSkillRange())
        {
             //TODO �ޯ�����ʵe
        }
    }

    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sighRadius);

        foreach (var target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }

    bool TargetInAttackRange()
    {
        if (attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.attackRange;
        }
        else
            return false;
    }

    bool TargetInSkillRange()
    {
        if (attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange;
        }
        else
            return false;
    }
    void GetNewWayPoint()
    {
        remainLookAtTime = LookAtTime;
        //����U�@�Ө��ޱo�H���ؼ��I        
        //��������d��[-potralRange,potralRange]
        float randomX = Random.Range(-potralRange, potralRange);
        float randomZ = Random.Range(-potralRange, potralRange);

        //�b�ĤH�ۤv�������y���I�W�i����H���I
        Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);

        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, potralRange, 1) ? hit.position : transform.position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sighRadius);
    }

    //�����ƥ�
    void Hit()
    {
        if (attackTarget !=null)
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            targetStats.TakeDamage(characterStats, targetStats);
        }
    }
}
