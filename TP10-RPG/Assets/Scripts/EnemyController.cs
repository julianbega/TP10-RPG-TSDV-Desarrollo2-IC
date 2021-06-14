using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IHitable
{
    public float lookRadius = 10f;
    public float speed = 1;
    GameObject target;
    NavMeshAgent agent;

    public int armor;
    public int hp;

    public GameObject drop;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.transform.position, this.transform.position);

        if (distance < lookRadius)
        {
            agent.SetDestination(target.transform.position);

            if (distance <= agent.stoppingDistance)
            {
                FaceTarget();
            }
        }
        if (hp <= 0)
        {
            Die();
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
        }
    }

    public void Die()
    {
        Instantiate(drop, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
