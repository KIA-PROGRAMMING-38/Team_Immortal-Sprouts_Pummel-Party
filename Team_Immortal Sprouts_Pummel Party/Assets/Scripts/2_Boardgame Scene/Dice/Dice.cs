using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dice
{
    /// <summary>
    /// �ӽ� ���.. ȭ�� ��ġ �Է� �߻����� �� �ֻ��� ���� �� �ֵ���..
    /// </summary>
    public int Roll()
    {
        int num = Random.Range(-1, 8);
        Debug.Log($"�ֻ��� ���: {num}");

        return num;
    }
}
