using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monsters : Common_Biological_Functions
{
    public LayerMask fixedTargetLayer;

    private Common_Biological_Functions fixedTarget;
    private NavMeshAgent navMeshAgent;

    public ParticleSystem hitEffect;
    public AudioClip deathSound;
    public AudioClip hitSound;

    private Animator monsterAnimator;
    private AudioSource monsterAudioPlayer;
    private Renderer monsterRenderer;

    private SkinnedMeshRenderer[] monsterSkin;

    public float damage = 0f;
    public float timeBetAttack = 1.0f;
    private float lastAttackTime;

    public float attackRange = 1.5f; // 공격 범위 설정
    public float chaseRange = 0.1f; //추적 범위 설정

    private bool hasTarget
    {
        get
        {
            if (fixedTarget != null && !fixedTarget.dead)
            {
                return true;
            }

            return false;
        }
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        monsterAnimator = GetComponent<Animator>();
        monsterAudioPlayer = GetComponent<AudioSource>();
        
        monsterSkin = GetComponentsInChildren<SkinnedMeshRenderer>();
        monsterRenderer = GetComponentInChildren<Renderer>();
    }

    private void OnEnable()
    {
        foreach (var mat in monsterSkin)
        {
            mat.material.color = new Color(1f, 1f, 1f, 0f);
        }
    }

    public void Setup(Monster_Data monster_Data)
    {
        startingHP = monster_Data.currentHP;
        currentHP = monster_Data.currentHP;

        damage = monster_Data.damage;

        navMeshAgent.speed = monster_Data.speed;

        monsterRenderer.material.color = monster_Data.skinColor;
    }

    private void Start()
    {
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        monsterAnimator.SetBool("HasTarget", hasTarget);
    }

    private IEnumerator UpdatePath()
    {
        while (!dead)
        {
            if (hasTarget)
            {
                float distanceToTarget = Vector3.Distance(transform.position, fixedTarget.transform.position);

                if (distanceToTarget <= attackRange) // 공격 범위 내일 때
                {
                    navMeshAgent.isStopped = true;
                    AttackTarget(); // 공격 시도
                }
                else if (distanceToTarget <= chaseRange) // 추적 범위 내일 때
                {
                    navMeshAgent.isStopped = false;
                    navMeshAgent.SetDestination(fixedTarget.transform.position); // 대상을 추적
                }
                else
                {
                    fixedTarget = null; // 대상이 추적 범위를 벗어나면 타겟을 잃음
                }
            }
            else
            {
                navMeshAgent.isStopped = true;
                FindTarget(); // 새로운 대상을 탐색
            }

            yield return new WaitForSeconds(0.25f);
        }
    }

    private void AttackTarget()
    {
        if (Time.time >= lastAttackTime + timeBetAttack)
        {
            lastAttackTime = Time.time;

            Vector3 hitPoint = fixedTarget.transform.position;
            Vector3 hitNormal = transform.position - fixedTarget.transform.position;

            fixedTarget.OnDamage(damage, hitPoint, hitNormal);
        }
    }

    private void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, chaseRange, fixedTargetLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            Common_Biological_Functions common_Biological_Functions = colliders[i].GetComponent<Common_Biological_Functions>();

            if (common_Biological_Functions != null && !common_Biological_Functions.dead)
            {
                fixedTarget = common_Biological_Functions;
                break;
            }
        }
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            //Debug.Log(monsterSkin[0].material.color);

            foreach (var mat in monsterSkin)
            {           
             mat.material.color = new Color(1f,1f,1f,1f);  
            }

            monsterAudioPlayer.PlayOneShot(hitSound);
        }
        else
        {
            foreach(var mat in monsterSkin)
            {
                mat.material.color = new Color(1f,1f,1f,0.1f);
            }
        }

        base.OnDamage(damage, hitPoint, hitNormal);
    }

    public override void Die()
    {
        base.Die();

        Collider[] monsterColliders = GetComponents<Collider>();
        for(int i = 0; i < monsterColliders.Length; i++)
        {
            monsterColliders[i].enabled = false;
        }

        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        monsterAnimator.SetTrigger("Die");
        monsterAudioPlayer.PlayOneShot(deathSound);
    }

    public void OnTriggerStay(Collider other)
    {
        if(!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            Common_Biological_Functions attackTarget = other.GetComponent<Common_Biological_Functions>();

            if(attackTarget != null && attackTarget == fixedTarget)
            {
                lastAttackTime = Time.time;

                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - other.transform.position;

                attackTarget.OnDamage(damage, hitPoint, hitNormal);
            }
        }
    }
}
