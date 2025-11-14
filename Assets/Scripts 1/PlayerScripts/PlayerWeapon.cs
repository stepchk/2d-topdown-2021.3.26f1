using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float lastFireTime = 0f;
    public int ammoPerShot = 1;
    public float projectileSpeed = 10f;

    [Header("Ammo Item")]
    public Item ammoItem; // Assign your ammo Item in inspector

    private InventoryManager inventoryManager;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (firePoint == null)
        {
            firePoint = transform;
        }
    }

    void Update()
    {
        // Fire on mouse click
        if (Input.GetMouseButtonDown(0))
        {
            FireWeapon();
        }
    }

    void FireWeapon()
    {
        // Check if enough time has passed (fire rate cooldown)
        if (Time.time - lastFireTime < fireRate)
        {
            return;
        }

        // Check if we have ammo in inventory
        if (!HasAmmo())
        {
            Debug.Log("No ammo!");
            return;
        }

        // Consume ammo
        bool ammoConsumed = inventoryManager.ConsumeItem(ammoItem, ammoPerShot);
        if (!ammoConsumed)
        {
            Debug.Log("Failed to consume ammo!");
            return;
        }

        // Create projectile
        CreateProjectile();
        lastFireTime = Time.time;
        Debug.Log("Fired! Ammo remaining: " + CountAmmo());

        StartCoroutine(FlashOnFire());

        IEnumerator FlashOnFire()
        {
            spriteRenderer.color = Color.yellow;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
        }
    }

    bool HasAmmo()
    {
        return CountAmmo() >= ammoPerShot;
    }

    int CountAmmo()
    {
        if (ammoItem == null || inventoryManager == null)
            return 0;

        int totalAmmo = 0;
        foreach (InventorySlot slot in inventoryManager.inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == ammoItem)
            {
                totalAmmo += itemInSlot.count;
            }
        }
        return totalAmmo;
    }

    void CreateProjectile()
    {
        // Get mouse position and direction
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 fireDirection = (worldMousePos - firePoint.position).normalized;

        // Instantiate projectile
        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Initialize(fireDirection, projectileSpeed);
        }

        // Rotate projectile to face fire direction
        float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
        projectileGO.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    
}

