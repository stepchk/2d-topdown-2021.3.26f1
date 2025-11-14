using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private string savePath;
    private string fileName = "gamedata.json";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Set save path (persistent data folder)
        savePath = Application.persistentDataPath + "/" + fileName;
        Debug.Log("Save path: " + savePath);
    }

    void Start()
    {
        // Auto-load on game start
        LoadGame();
    }

    void Update()
    {
        // Press P to save
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveGame();
        }

        // Press L to load
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
        }
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        // Gather player data
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        PlayerWeapon playerWeapon = FindObjectOfType<PlayerWeapon>();
        Transform playerTransform = FindObjectOfType<PlayerController>().transform;

        saveData.playerData = new PlayerSaveData
        {
            positionX = playerTransform.position.x,
            positionY = playerTransform.position.y,
            currentHealth = playerHealth.currentHealth,
            maxHealth = playerHealth.maxHealth,
            
        };

        // Gather inventory data
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        saveData.inventoryData = new InventorySaveData();

        for (int i = 0; i < inventoryManager.inventorySlots.Length; i++)
        {
            InventoryItem itemInSlot = inventoryManager.inventorySlots[i]
                .GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null)
            {
                saveData.inventoryData.items.Add(new InventoryItemData
                {
                    itemName = itemInSlot.item.name,
                    count = itemInSlot.count,
                    slotIndex = i
                });
            }
        }

        // Gather game state data
        saveData.gameStateData = new GameStateSaveData
        {
            playTime = Time.time,
            monstersKilled = 0, // Add your tracking logic
            lastSaveTime = System.DateTime.Now.Second
        };

        // Convert to JSON
        string json = JsonUtility.ToJson(saveData, true);

        // Write to file
        try
        {
            File.WriteAllText(savePath, json);
            Debug.Log("Game saved successfully to: " + savePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save game: " + e.Message);
        }
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("Save file not found at: " + savePath);
            return;
        }

        try
        {
            string json = File.ReadAllText(savePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            // Load player data
            PlayerController playerController = FindObjectOfType<PlayerController>();
            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();

            playerController.transform.position = new Vector3(
                saveData.playerData.positionX,
                saveData.playerData.positionY,
                0
            );

            playerHealth.currentHealth = saveData.playerData.currentHealth;
            playerHealth.healthBar.SetHealth(saveData.playerData.currentHealth);

            // Load inventory data
            LoadInventory(saveData.inventoryData);

            Debug.Log("Game loaded successfully from: " + savePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load game: " + e.Message);
        }
    }

    void LoadInventory(InventorySaveData inventoryData)
    {
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();

        // Clear existing inventory
        foreach (InventorySlot slot in inventoryManager.inventorySlots)
        {
            InventoryItem existingItem = slot.GetComponentInChildren<InventoryItem>();
            if (existingItem != null)
            {
                Destroy(existingItem.gameObject);
            }
        }

        // Load saved items
        foreach (InventoryItemData itemData in inventoryData.items)
        {
            // Find item asset by name
            Item itemAsset = Resources.Load<Item>("Items/" + itemData.itemName);

            if (itemAsset != null && itemData.slotIndex < inventoryManager.inventorySlots.Length)
            {
                InventorySlot slot = inventoryManager.inventorySlots[itemData.slotIndex];

                // Create inventory item
                GameObject itemGO = Instantiate(
                    inventoryManager.inventoryItemPrefab,
                    slot.transform
                );

                InventoryItem invItem = itemGO.GetComponent<InventoryItem>();
                invItem.item = itemAsset;
                invItem.count = itemData.count;
                invItem.RefreshCount();
            }
            else
            {
                Debug.LogWarning("Could not find item: " + itemData.itemName);
            }
        }
    }

    public bool HasSaveFile()
    {
        return File.Exists(savePath);
    }

    public void DeleteSaveFile()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Save file deleted: " + savePath);
        }
    }

    public string GetSaveFilePath()
    {
        return savePath;
    }
}

