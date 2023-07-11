using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LobbyData : MonoBehaviour
{
    public List<LobbyTextData> LobbyTexts { get; private set; }

    private void Awake()
    {
        LobbyTexts = Managers.Data.ParseToList<LobbyTextData>(Path.Combine("Assets", "Resources", "CSVs", "LobbyTextTable.csv"));
    }
}
