using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class BodyData : ILoadble
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string FileName { get; set; }

    public string GetFileName() => FileName;
}