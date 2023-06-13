using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CustomData : ScriptableObject
{
    public GameObject[] hats; // 7��

    public Texture2D[] bodyColors; // 8��

    /// <summary>
    /// index�� ���� ���ڸ� ��ȯ�մϴ�
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public GameObject GetHatFromData(int index)
    {
        return hats[index];
    }

    /// <summary>
    /// index�� ���� texture2D�� ��ȯ�մϴ�
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Texture2D GetBodyColorFromData(int index)
    {
        return bodyColors[index];   
    }
}
