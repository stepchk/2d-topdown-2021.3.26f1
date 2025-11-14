using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;
    public HealthBar healthBar;

    public Item lootDropItem;
    public GameObject worldItemPrefab;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
            healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Monster died!");

        DropLoot();
        Destroy(gameObject);
    }

    void DropLoot()
    {
        if (lootDropItem != null && worldItemPrefab != null)
        {
            GameObject itemGO = Instantiate(worldItemPrefab, transform.position, Quaternion.identity);
            WorldItem worldItem = itemGO.GetComponent<WorldItem>();
            if (worldItem != null)
            {
                worldItem.item = lootDropItem;
                if (worldItem.spriteRenderer != null)
                    worldItem.spriteRenderer.sprite = lootDropItem.image;
            }
        }
    }

    public bool IsDead => isDead;
}
