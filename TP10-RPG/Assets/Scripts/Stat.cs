using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Stat
{
    [SerializeField]
    private int baseValue = 0;
    private List<int> mods = new List<int>();
    public int getValue()
    {
        int finalValue = baseValue;
        foreach (int element in mods)
        {
            finalValue += element;
        }
        return finalValue;
    }
    public void addModifier(int modifier)
    {
        if (modifier != 0)
        {
            mods.Add(modifier);
        }
    }
    public void RemoveModifier(int modifier)
    {
        if (modifier != 0)
        {
            mods.Remove(modifier);
        }
    }
}
