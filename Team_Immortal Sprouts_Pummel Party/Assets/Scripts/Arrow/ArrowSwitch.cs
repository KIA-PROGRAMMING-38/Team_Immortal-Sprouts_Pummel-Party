using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSwitch : MonoBehaviour
{
    [SerializeField] private DirectionArrow[] arrows;
    
    /// <summary>
    /// ȭ��ǥ�� �Ѵ� �Լ�
    /// </summary>
    public void TurnOnSwitch()
    {
        foreach (DirectionArrow arrow in arrows)
        {
            arrow.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// ȭ��ǥ�� ���� �Լ�
    /// </summary>
    public void TurnOffSwitch()
    {
        foreach (DirectionArrow arrow in arrows)
        {
            arrow.ArrowDisappear();
        }
    }

}
