using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public PlayerSaveData playerData;
    public InventorySaveData inventoryData;
    public GameStateSaveData gameStateData;
}

[System.Serializable]
public class PlayerSaveData
{
    public float positionX;
    public float positionY;
    public int currentHealth;
    public int maxHealth;
    public string equippedWeapon;
}

[System.Serializable]
public class InventorySaveData
{
    public List<InventoryItemData> items = new List<InventoryItemData>();
}

[System.Serializable]
public class InventoryItemData
{
    public string itemName;
    public int count;
    public int slotIndex;
}

[System.Serializable]
public class GameStateSaveData
{
    public float playTime;
    public int monstersKilled;
    public int lastSaveTime;
}

