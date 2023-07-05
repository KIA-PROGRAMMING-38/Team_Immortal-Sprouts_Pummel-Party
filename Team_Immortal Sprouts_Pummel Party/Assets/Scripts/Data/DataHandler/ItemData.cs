using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Pistol,
    HotAirBalloon,
    EmptyBowl
}

public class ItemData
{
    public List<Dictionary<string, object>> ItemDialog; // PrefabPool

    public void ReadCSV()
    {
        ItemDialog = CSVReader.Read("CSVs/ItemTable");
    }


}
