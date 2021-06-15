using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldItem : MonoBehaviour, IPickable
{

    int ID;
    int Amount;

    public void SetItem(int _ID, int _Amount)
    {
        ID = _ID;
        Amount = _Amount;
        GetComponent<MeshFilter>().mesh = GameplayManager.GetInstance().GetItemFromID(ID).mesh;
        GetComponent<Renderer>().material = GameplayManager.GetInstance().GetItemFromID(ID).worldMaterial;
    }

    Slot IPickable.PickUp()
    {
        return new Slot(ID, Amount);
    }
}
