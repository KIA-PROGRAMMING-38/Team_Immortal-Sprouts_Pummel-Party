using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dice
{
    /// <summary>
    /// 임시 기능.. 화면 터치 입력 발생했을 때 주사위 굴릴 수 있도록..
    /// </summary>
    public int Roll()
    {
        int num = Random.Range(-1, 8);
        Debug.Log($"주사위 결과: {num}");

        return num;
    }
}
