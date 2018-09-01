using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class Steam : MonoBehaviour {

    public static Steam Instence;
   
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (!Instence)
        {
            Instence = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SteamUserStats.SetAchievement("Welcome");
        SteamUserStats.StoreStats();
        //StartCoroutine(TriggerDrop());
        //Generate();
    }

    public void GetAchievment(int index)
    {
        SteamUserStats.SetAchievement(index.ToString());
        SteamUserStats.StoreStats();
    }

    void Generate()
    {
        SteamInventoryResult_t InventoryResult;
        SteamInventory.GetAllItems(out InventoryResult);
        SteamItemDef_t[] itemDef_t = new SteamItemDef_t[1];
        itemDef_t[0].m_SteamItemDef = 5;
        SteamInventory.GenerateItems(out InventoryResult, itemDef_t, null, 1);
        Debug.Log(InventoryResult.m_SteamInventoryResult);
        SteamInventory.DestroyResult(InventoryResult);
    }
    IEnumerator TriggerDrop()
    {
        
        while (gameObject != null)
        {
            SteamInventoryResult_t InventoryResult;
            SteamItemDef_t Item;
            Item.m_SteamItemDef = 5;
            bool res = SteamInventory.TriggerItemDrop(out InventoryResult, Item);
            Debug.Log(InventoryResult + " " + res);
            SteamInventory.DestroyResult(InventoryResult);
            yield return new WaitForSeconds(5f);
        }
    }
}
