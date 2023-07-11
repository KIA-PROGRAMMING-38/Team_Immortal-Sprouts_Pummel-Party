using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatData : ILoadble
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string FileName { get; set; }

    public string GetFileName() => FileName;
}