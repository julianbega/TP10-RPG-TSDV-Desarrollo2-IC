using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int maxStamina = 100;
    public Stat armor;
    public Stat damage;
    public Stat attackSpeed;
    public Stat miningRate;

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        damage -= armor.getValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;
        Debug.Log(transform.name + "taken" + damage + "damage.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public virtual void Die()
    {
        Debug.Log(transform.name + "died.");
    }
}
