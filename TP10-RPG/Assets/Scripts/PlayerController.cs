using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour, IHitable
{
    CharacterController controller;
    PlayerStats stats;
    Vector3 playerVelocity;

    float playerSpeed = 2.0f;
    float attackRadius = 3.0f;
    float pickUpRadius = 5.0f;
    
    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        stats = gameObject.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

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
            Debug.Log("ataca");
            Attack();
        }
    }

    void Attack()
    {
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
                // Add to Inventory;
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
}

