using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{

    [Header("Items")]
    [SerializeField] ItemList allItems;
    [SerializeField] GameObject dropTemplate;
    [Space(10)]
    [SerializeField] PlayerController playerController;
    [SerializeField] UiInventory uiInventory;

    static private GameplayManager instance;

    static public GameplayManager GetInstance() { return instance; }

    bool inventoryOpen = false;

    string savePath = "SaveFile.json";

    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadJson();
        playerController.OnInventoryOpen += ToggleInventory;
    }
    public int GetRandomItemID()
    {
        return Random.Range(0, allItems.List.Count);
    }

    public int GetRandomAmmountOfItem(int id)
    {
        return Random.Range(1, allItems.List[id].maxStack);
    }

    public Item GetItemFromID(int id)
    {
        return allItems.List[id];
    }

    public GameObject GetPlayer()
    {
        return playerController.gameObject;
    }

    public GameObject GetDropTemplate()
    {
        return dropTemplate;
    }

    void ToggleInventory()
    {
        inventoryOpen = !inventoryOpen;
        uiInventory.ToggleInventory(inventoryOpen);
    }

    public void CreateRandomWorldItem(Vector3 pos)
    {
        int itemID = GetRandomItemID();
        int amount = 1;
        if (GetItemFromID(itemID).maxStack > 1)
        {
            amount = UnityEngine.Random.Range(1, GetItemFromID(itemID).maxStack + 1);
        }
        GameObject go = Instantiate(GetDropTemplate(), pos, Quaternion.identity);
        go.GetComponent<worldItem>().SetItem(itemID, amount);
    }

    public void CreateWorldItem(Vector3 pos, Slot slot)
    {
        int itemID = slot.ID;
        int amount = slot.amount;
        GameObject go = Instantiate(GetDropTemplate(), pos, Quaternion.identity);
        go.GetComponent<worldItem>().SetItem(itemID, amount);
    }

    public void SaveJson()
    {
        List<Slot> playerItems = playerController.gameObject.GetComponent<Inventory>().GetSaveSlots();
        string json = "";
        for (int i = 0; i < playerItems.Count; i++)
        {
            json += JsonUtility.ToJson(playerItems[i]);
        }

        FileStream fs;

        if (!File.Exists(savePath))
            fs = File.Create(savePath);
        else
            fs = File.Open(savePath, FileMode.Truncate);
   
        BinaryWriter bw = new BinaryWriter(fs);
        bw.Write(json);
        fs.Close();
        bw.Close();
    }

    public void LoadJson()
    {
        string savedData;
        if (File.Exists(savePath))
        {
            FileStream fs;
            fs = File.Open(savePath, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            savedData = br.ReadString();
            fs.Close();
            br.Close();
        }
        else
        {
            return;
        }
        List<Slot> newList = new List<Slot>();
        for (int i = 0; i < savedData.Length; i++)
        {
            if(savedData[i] == '{')
            {
                string slotString = "";
                int aux = 0;
                while(savedData[i + aux] != '}')
                {
                    slotString += savedData[i + aux];
                    aux++;
                }
                slotString += '}';
                Slot newSlot = JsonUtility.FromJson<Slot>(slotString);
                newList.Add(newSlot);
            }
        }
        playerController.gameObject.GetComponent<Inventory>().SetSaveSlots(newList);
    }

    void OnDisable()
    {
        if(playerController)
        {
            SaveJson();
        }
    }
}