using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour, IHitable
{

    [Header("Meteorites")]
    [SerializeField] GameObject meteoritePrefab;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] float spawnAltitude = 10f;
    [SerializeField] int meteoritesPerWave = 10;
    [SerializeField] float meteoritesTimeBetweenWaves = 1f;
    [SerializeField] float meteoriteWaveMaxSize = 1f;
    [SerializeField] int meteoritesWaves = 5;
    [SerializeField] float maxMeteoriteCooldown = 30f;
    float currentMeteoriteCooldown = -1;
    NavMeshAgent agent;

    Camera cam;
    Animator anim;
    PlayerStats stats;
    bool inventoryIsOpen = false;

    public float playerSpeed = 2.0f;
    float attackRadius = 3.0f;
    float pickUpRadius = 5.0f;

    public Action OnInventoryOpen;

    bool pausedInput = false;

    bool lockedAttack = false;

    private void Awake()
    {        
        anim = GetComponent<Animator>();
    }

    private void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        agent.speed = playerSpeed;
        cam = Camera.main;
        stats = gameObject.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        if (!pausedInput)
        {

            if (agent.remainingDistance <= 0.1)
            {
                Debug.Log("remaining distance = " + agent.remainingDistance);
                anim.SetFloat("MoveSpeed", 0);
            }
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, 100, groundLayer))
                {
                    MoveToPoint(hit.point);
                    anim.SetFloat("MoveSpeed", 1);
                }
            }

            if (lockedAttack) return;

            /*
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            controller.Move(move * Time.deltaTime * playerSpeed);
            anim.SetFloat("MoveSpeed", Mathf.Abs(move.x) + Mathf.Abs(move.z));

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }*/



            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUp();
            }
            if (Input.GetButtonDown("Fire2"))
            {
                if (currentMeteoriteCooldown < 0)
                {
                    currentMeteoriteCooldown = maxMeteoriteCooldown;
                    StartCoroutine(MeteorCast());
                }
            }
            if (currentMeteoriteCooldown > 0)
            {
                currentMeteoriteCooldown -= Time.deltaTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryIsOpen = !inventoryIsOpen;
            pausedInput = !pausedInput;
            OnInventoryOpen();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && inventoryIsOpen)
        {
            inventoryIsOpen = !inventoryIsOpen;
            pausedInput = !pausedInput;
            OnInventoryOpen();
        }
    }
    void Attack()
    {
        anim.SetTrigger("Attack");
        lockedAttack = true;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + (attackRadius * transform.forward), attackRadius);
        foreach (var hitCollider in hitColliders)
        {
            IHitable ihitable = hitCollider.GetComponent<IHitable>();
            if (ihitable != null)
            {
                ihitable.takeDamage(stats.damage.getValue());
            }
        }
    }

    void PickUp()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pickUpRadius);
        foreach (var hitCollider in hitColliders)
        {
            IPickable pickable = hitCollider.GetComponent<IPickable>();
            if (pickable != null)
            {
                Slot item = pickable.PickUp();
                if (GetComponent<Inventory>().AddNewItem(item.ID, item.amount))
                {
                    Destroy(hitCollider.gameObject);
                }
            }
        }
    }

    public void takeDamage(int damage)
    {
        damage = damage - stats.armor.getValue();
        if (damage > 0)
        {
            stats.currentHealth -= damage;
        }
    }
    IEnumerator MeteorCast()
    {
        anim.SetTrigger("Cast");
        lockedAttack = true;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 MeteoriteSpawnPos;
        if (Physics.Raycast(ray, out hit, float.MaxValue, groundLayer))
        {
            MeteoriteSpawnPos = new Vector3(hit.point.x, hit.point.y + spawnAltitude, hit.point.z);
            for (int i = 0; i < meteoritesWaves; i++)
            {
                yield return new WaitForSeconds(meteoritesTimeBetweenWaves);
                for (int j = 0; j < meteoritesPerWave; j++)
                {
                    Instantiate(meteoritePrefab, MeteoriteSpawnPos + UnityEngine.Random.insideUnitSphere * meteoriteWaveMaxSize, Quaternion.identity);
                }
            }
        }
    }
    void UnlockAttack()
    {
        lockedAttack = false;
    }

    public void MoveToPoint(Vector3 point)
    {
        agent.SetDestination(point);
    }
}

