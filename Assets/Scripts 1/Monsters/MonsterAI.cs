using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    public Transform player;

    [Header("Detection")]
    public float detectionRange = 5f;
    public float stoppingDistance = 0.5f;

    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Combat")]
    public float attackRange = 0.8f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;
    public int attackDamage = 10;

    private Rigidbody2D rb;
    private MonsterHealth health;
    private SpriteRenderer spriteRenderer;
    private bool playerDetected = false;
    private bool canAttack = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<MonsterHealth>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (player == null)
        {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null)
                player = playerGO.transform;
        }
    }

    void Update()
    {
        if (health.IsDead || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            playerDetected = true;
            MoveTowardPlayer();

            if (distanceToPlayer < attackRange)
            {
                AttackPlayer();
            }
        }
        else
        {
            playerDetected = false;
            rb.velocity = Vector2.zero;
        }

        UpdateAttackCooldown();
    }

    void MoveTowardPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        // Flip sprite based on direction
        if (direction.x < 0 && spriteRenderer.flipX == false)
            spriteRenderer.flipX = true;
        else if (direction.x > 0 && spriteRenderer.flipX == true)
            spriteRenderer.flipX = false;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Stop when close enough
        if (distanceToPlayer > stoppingDistance)
        {
            
            rb.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void AttackPlayer()
    {
        if (canAttack)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log("Monster attacked player!");
            }

            canAttack = false;
            lastAttackTime = Time.time;
        }
    }

    void UpdateAttackCooldown()
    {
        if (!canAttack && Time.time - lastAttackTime >= attackCooldown)
        {
            canAttack = true;
        }
    }

    //Detection range in editor debug
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
