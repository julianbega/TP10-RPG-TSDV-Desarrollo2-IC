using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IHitable
{
    [Header("Movement")]
    public float lookRadius = 10f;
    public float speed = 1;
    GameObject target;
    NavMeshAgent agent;

    [Header("Stats")]
    public int armor;
    public int hp;

    [Header("Drop")]
    [SerializeField] float dropRate = 25f;
    public GameObject drop;

    [Header("Animation")]
    [SerializeField] float deathTimer = 5f;
    Animator anim;

    Collider col;

    public Action<EnemyController> OnDeath;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.enabled)
        {
            float distance = Vector3.Distance(target.transform.position, this.transform.position);

            if (distance < lookRadius)
            {
                agent.SetDestination(target.transform.position);

                anim.SetFloat("MoveSpeed", 1);

                if (distance <= agent.stoppingDistance)
                {
                    FaceTarget();
                }
            }
            else
            {
                anim.SetFloat("MoveSpeed", 0);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    void FaceTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void takeDamage(int damage)
    {
        damage = damage - armor;
        if (damage > 0)
        {
            hp -= damage;
            if (hp <= 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        int chance = UnityEngine.Random.Range(1, 101);
        if (dropRate > chance)
        {
            GameplayManager gm = GameplayManager.GetInstance();
            gm.CreateRandomWorldItem(transform.position);
        }
        anim.SetTrigger("Die");
        agent.enabled = false;
        col.enabled = false;
        Destroy(gameObject, deathTimer);
        OnDeath(this);
    }
}
