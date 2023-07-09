using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BodyData
{
    public int ID { get; private set; }
    public string Name { get; private set; }
    public Material Material { get; private set; }
}

public class BodyDataLoader : ILoader<int, BodyData>
{
    public List<BodyData> _bodyData = new List<BodyData>();

    public BodyData CreateItem()
    {
        BodyData newBodyData = new BodyData();
        return newBodyData;
    }

    public void SetDataList(List<BodyData> dataSet = null) => _bodyData = dataSet;

    public Dictionary<int, BodyData> MakeDic()
    {
        Dictionary<int, BodyData> dict = new Dictionary<int, BodyData>();

        foreach (BodyData data in _bodyData)
        {
            dict.Add(data.ID, data);
        }

        return dict;
    }

    
}
