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
    #region 정적 데이터
    public Dictionary<int, ItemData> Items { get; private set; }
    public Dictionary<int, BodyData> Bodies { get; private set; }
    public Dictionary<int, HatData> Hats { get; private set; }
    public Dictionary<int, MiniGameData> MiniGames { get; private set; }
    public Dictionary<int, AwardData> Awards { get; private set; }

    #endregion

    #region 동적 데이터
    public Dictionary<Player, PlayerData> Players { get; private set; } = new Dictionary<Player, PlayerData>();

    #endregion


    private const string csvPath = "Assets/Resources/CSVs/";
    public void Init()
    {
        Items = ParseToDict<int, ItemData>($"{csvPath}ItemTable.csv", data => data.ID);
        Bodies = ParseToDict<int, BodyData>($"{csvPath}BodyTable.csv", data => data.ID);
        Hats = ParseToDict<int, HatData>($"{csvPath}HatTable.csv", data => data.ID);
        MiniGames = ParseToDict<int, MiniGameData>($"{csvPath}MiniGameTable.csv", data => data.ID);
        Awards = ParseToDict<int, AwardData>($"{csvPath}AwardTable.csv", data => data.ID);
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
