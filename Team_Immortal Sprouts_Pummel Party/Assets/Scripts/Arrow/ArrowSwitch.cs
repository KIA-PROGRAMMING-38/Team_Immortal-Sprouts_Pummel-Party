using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSwitch : MonoBehaviour
{
    [SerializeField] private DirectionArrow[] arrows;
    
    /// <summary>
    /// 화살표를 켜는 함수
    /// </summary>
    public void TurnOnSwitch()
    {
        foreach (DirectionArrow arrow in arrows)
        {
            arrow.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 화살표를 끄는 함수
    /// </summary>
    public void TurnOffSwitch()
    {
        foreach (DirectionArrow arrow in arrows)
        {
            arrow.ArrowDisappear();
        }
    }

}
