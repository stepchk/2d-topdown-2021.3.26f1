using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int projectileDamage = 20;
    public float lifetime = 5f;

    private Vector2 direction;
    private float speed;
    private float spawnTime;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnTime = Time.time;
    }

    void Update()
    {
        // Destroy projectile after lifetime expires
        if (Time.time - spawnTime > lifetime)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.velocity = direction * speed;
        }
    }

    public void Initialize(Vector2 fireDirection, float fireSpeed)
    {
        direction = fireDirection;
        speed = fireSpeed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterHealth monsterHealth = collision.GetComponent<MonsterHealth>();

        if (monsterHealth != null)
        {
            monsterHealth.TakeDamage(projectileDamage);
            Debug.Log("Hit monster! Damage: " + projectileDamage);
            Destroy(gameObject); // Destroy projectile on hit
        }
    }
}

