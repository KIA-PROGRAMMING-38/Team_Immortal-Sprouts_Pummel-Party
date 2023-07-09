using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public interface ILoader<Key, Item>
{
    Item CreateItem();
    void SetDataList(List<Item> dataSet = null);
    Dictionary<Key, Item> MakeDic();
}

public class DataManager
{
    public Dictionary<int, ItemData> Items { get; private set; }    
    public Dictionary<int, BodyData> Bodies { get; private set; }   

    public void Init()
    {
        //Items = LoadCSV<ItemDataLoader, int, ItemData>("ItemTable").MakeDic();
        Bodies = LoadCSV<BodyDataLoader, int, BodyData>("BodyTable").MakeDic();
    }

    private Loader LoadCSV<Loader, Key, Item>(string name) where Loader : ILoader<Key, Item >, new()
    {
        List<Dictionary<string, object>> dialog = CSVReader.Read($"CSVs/{name}");

        IEnumerable<string> categoryKeys = dialog[0].Keys; // 카테고리 키값 가져옴

        Loader loader = new Loader(); // ItemDataLoader

        List<Item> itemList = new List<Item>();

        for (int i = 0; i < dialog.Count; ++i)
        {
            Item item = loader.CreateItem(); // ItemData 를 만든다
            
            List<object> values = new List<object>();

            foreach (string category in categoryKeys)
            {
                values.Add(dialog[i][category]);
            }

            SetPropertyValues<Item>(item, values);
            itemList.Add(item);
        }

        loader.SetDataList(itemList);

        return loader;
    }

    private void SetPropertyValues<T>(T instance, List<object> valueSet)
    {
        Type type = instance.GetType();
        PropertyInfo[] properties = type.GetProperties();

        for (int i = 0; i < properties.Length ;++i)
        {
            object value = valueSet[i];
            PropertyInfo property = properties[i];
            Type propertyType = property.PropertyType;

            if (propertyType.IsEnum)
            {
                object enumValue = Enum.Parse(propertyType, (string)value);
                property.SetValue(instance, enumValue);
            }
            else if (propertyType == typeof(Material))
            {
                Material material = Managers.Resource.Load<Material>($"{value}");
                property.SetValue(instance, material);
            }
            else
            {
                property.SetValue(instance, Convert.ChangeType(value, properties[i].PropertyType));
            }
        }
    }
}
