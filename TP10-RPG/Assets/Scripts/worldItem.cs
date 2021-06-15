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
    }

    Slot IPickable.PickUp()
    {
        return new Slot(ID, Amount);
    }
}
