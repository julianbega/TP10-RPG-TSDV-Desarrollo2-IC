using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IHitable>() != null)
        {
            other.GetComponent<IHitable>().takeDamage(damage);
        }
        Destroy(gameObject);
    }
}
