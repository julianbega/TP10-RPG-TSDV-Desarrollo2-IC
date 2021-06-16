using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    CharacterController controller;
    Animator anim;
    PlayerStats stats;
    Vector3 playerVelocity;

    float playerSpeed = 2.0f;
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
        controller = gameObject.GetComponent<CharacterController>();
        stats = gameObject.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (!pausedInput)
        {

            if (lockedAttack) return;

            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            controller.Move(move * Time.deltaTime * playerSpeed);
            anim.SetFloat("MoveSpeed", Mathf.Abs(move.x) + Mathf.Abs(move.z));

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUp();
            }
            if (Input.GetButtonDown("Fire1"))
            {
                Attack();
            }
            if (Input.GetButtonDown("Fire2"))
            {
                if(currentMeteoriteCooldown < 0)
                {
                    currentMeteoriteCooldown = maxMeteoriteCooldown;
                    StartCoroutine(MeteorCast());
                }
            }
            if(currentMeteoriteCooldown > 0)
            {
                currentMeteoriteCooldown -= Time.deltaTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            pausedInput = !pausedInput;
            OnInventoryOpen();
        }

    }
    void Attack()
    {
        anim.SetTrigger("Attack");
        lockedAttack = true;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + (attackRadius*transform.forward), attackRadius);
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
        foreach(var hitCollider in hitColliders)
        {
            IPickable pickable = hitCollider.GetComponent<IPickable>();
            if(pickable != null)
            {
                Slot item = pickable.PickUp();
                if(GetComponent<Inventory>().AddNewItem(item.ID, item.amount))
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
        if (Physics.Raycast(ray, out hit, float.MaxValue,groundLayer))
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
}

