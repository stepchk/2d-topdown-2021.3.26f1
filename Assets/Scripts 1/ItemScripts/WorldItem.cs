using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public Item item;
    public SpriteRenderer spriteRenderer;
    public CircleCollider2D pickupCollider;
    public float pickupRange = 1.5f;

    private Transform player;
    private bool playerInRange = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (pickupCollider == null)
            pickupCollider = GetComponent<CircleCollider2D>();

        if (pickupCollider != null)
        {
            pickupCollider.radius = pickupRange;
            pickupCollider.isTrigger = true;
        }

        if (item != null && spriteRenderer != null)
            spriteRenderer.sprite = item.image;
    }

    void Update()
    {
        if (playerInRange)
        {
            PickUp();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            OnPlayerEnterRange();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            OnPlayerExitRange();
        }
    }

    public void PickUp()
    {
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager != null && item != null)
        {
            bool success = inventoryManager.AddItem(item);
            if (success)
            {
                Debug.Log("Picked up: " + item.name);
                Destroy(gameObject);
            }
        }
    }

    void OnPlayerEnterRange()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f); // Full brightness
        Debug.Log("Item in range");
    }

    void OnPlayerExitRange()
    {
        spriteRenderer.color = new Color(0.7f, 0.7f, 0.7f, 1f); // Dim
    }
}

