using CsvHelper;
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
    public Dictionary<int, ItemData> Items { get; private set; }    
    public Dictionary<int, BodyData> Bodies { get; private set; }   
    public Dictionary<int, HatData> Hats { get; private set; }

    public void Init()
    {
        Items = ParseToDict<int, ItemData>("Assets/Resources/CSVs/ItemTable.csv", data => data.ID);
        Bodies = ParseToDict<int, BodyData>("Assets/Resources/CSVs/BodyTable.csv", data => data.ID);
        Hats = ParseToDict<int, HatData>("Assets/Resources/CSVs/HatTable.csv", data => data.ID);
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

    //private Loader LoadCSV<Loader, Key, Item>(string name) where Loader : ILoader<Key, Item >, new()
    //{
    //    List<Dictionary<string, object>> dialog = CSVReader.Read($"CSVs/{name}");

    //    IEnumerable<string> categoryKeys = dialog[0].Keys; // 카테고리 키값 가져옴

    //    Loader loader = new Loader(); // ItemDataLoader

    //    List<Item> itemList = new List<Item>();

    //    for (int i = 0; i < dialog.Count; ++i)
    //    {
    //        Item item = loader.CreateItem(); // ItemData 를 만든다
            
    //        List<object> values = new List<object>();

    //        foreach (string category in categoryKeys)
    //        {
    //            values.Add(dialog[i][category]);
    //        }

    //        SetPropertyValues<Item>(item, values);
    //        itemList.Add(item);
    //    }

    //    loader.SetDataList(itemList);

    //    return loader;
    //}

    //private void SetPropertyValues<T>(T instance, List<object> valueSet)
    //{
    //    Type type = instance.GetType();
    //    PropertyInfo[] properties = type.GetProperties();

    //    for (int i = 0; i < properties.Length ;++i)
    //    {
    //        object value = valueSet[i];
    //        PropertyInfo property = properties[i];
    //        Type propertyType = property.PropertyType;

    //        if (propertyType.IsEnum)
    //        {
    //            object enumValue = Enum.Parse(propertyType, (string)value);
    //            property.SetValue(instance, enumValue);
    //        }
    //        else if (propertyType == typeof(Material))
    //        {
    //            Material material = Managers.Resource.Load<Material>($"{value}");
    //            property.SetValue(instance, material);
    //        }
    //        else
    //        {
    //            property.SetValue(instance, Convert.ChangeType(value, properties[i].PropertyType));
    //        }
    //    }
    //}
}
