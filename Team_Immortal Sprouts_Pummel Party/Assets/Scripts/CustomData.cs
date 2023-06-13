using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CustomData : ScriptableObject
{
    public GameObject[] hats; // 7개

    public Texture2D[] bodyColors; // 8개

    /// <summary>
    /// index에 따른 모자를 반환합니다
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public GameObject GetHatFromData(int index)
    {
        if (hats.Length <= index)
        {
            index = 0;
        }
        else if (index < 0)
        {
            index = hats.Length - 1;
        }

        return hats[index];
    }

    /// <summary>
    /// index에 따른 texture2D를 반환합니다
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Texture2D GetBodyColorFromData(int index)
    {
        if (hats.Length <= index)
        {
            index = 0;
        }
        else if (index < 0)
        {
            index = hats.Length - 1;
        }

        return bodyColors[index];   
    }
}
