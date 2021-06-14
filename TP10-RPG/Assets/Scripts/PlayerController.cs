using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour, IHitable
{
    public GameObject cast;
    [SerializeField]private Camera mainCamera;
    [SerializeField] private LayerMask layer;
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
        if (Input.GetButtonDown("Fire2"))
        {
            StartCoroutine("MeteorCast");
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
    IEnumerator MeteorCast()
    {
        GameObject castspell;
        Debug.Log("castea lluvia de metoritos");
        castspell = Instantiate(cast);
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycasthit,float.MaxValue,layer))
        {
            raycasthit.point = new Vector3(raycasthit.point.x, raycasthit.point.y + 0.2f, raycasthit.point.z);
            castspell.transform.position = raycasthit.point;
        }
        yield return new WaitForSeconds(2);
        Destroy(castspell);
        //castea el meteorito
    }
}

