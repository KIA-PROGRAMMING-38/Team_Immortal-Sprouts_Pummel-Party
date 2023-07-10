using CsvHelper;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;


public class DataManager
{
    public List<ItemData> Items { get; private set; }
    public List<BodyData> Bodies { get; private set; }
    public List<HatData> Hats { get; private set; }
    public List<MiniGameData> MiniGames { get; private set; }
    public List<AwardData> Awards { get; private set; }


    private readonly string csvPath = Path.Combine("Assets", "Resources", "CSVs");
    public void Init()
    {
        Items = ParseToList<ItemData>(Path.Combine(csvPath, "ItemTable.csv"));
        Bodies = ParseToList<BodyData>(Path.Combine(csvPath, "BodyTable.csv"));
        Hats = ParseToList<HatData>(Path.Combine(csvPath, "HatTable.csv"));
        MiniGames = ParseToList<MiniGameData>(Path.Combine(csvPath, "MiniGameTable.csv"));
        Awards = ParseToList<AwardData>(Path.Combine(csvPath, "AwardTable.csv"));
    }

    public List<T> ParseToList<T>([NotNull] string path)
    {
        using (var reader = new StreamReader(path))
        {
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<T>().ToList();
            }
        }
    }

    public Dictionary<Key, Item> ParseToDict<Key, Item>([NotNull] string path, Func<Item, Key> keySelector)
    {
        using (var reader = new StreamReader(path))
        {
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<Item>().ToDictionary(keySelector);
            }
        }
    }
}
