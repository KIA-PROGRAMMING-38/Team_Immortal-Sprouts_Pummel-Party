using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameData 
{
    public List<Dictionary<string, object>> MiniGameDialog; // PrefabPool

    public void ReadCSV()
    {
        MiniGameDialog = CSVReader.Read("CSVs/MiniGameTable");
    }
}
