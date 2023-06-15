using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CustomData : ScriptableObject
{
    public GameObject[] hats; // 8개(None 포함)

    public string[] hatTexts; // UI 모자 이름

    public Texture2D[] bodyColors; // 8개

    public Color32[] colors; // UI 배경색

    /// <summary>
    /// index에 따른 모자를 반환합니다
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public GameObject GetHatFromData(int index)
    {
        return hats[index];
    }

    /// <summary>
    /// index에 따른 texture2D를 반환합니다
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Texture2D GetBodyColorFromData(int index)
    {
        return bodyColors[index];   
    }
}
