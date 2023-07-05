using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image countImage2;
    [SerializeField] private Image countImage1;
    [SerializeField] private Canvas informationCanvas;
   
    /// <summary>
    /// 타이머 2 작동
    /// </summary>
    public void Timer2()
    {
        countImage2.gameObject.SetActive(true);
    }

    /// <summary>
    /// 타이머 1 작동
    /// </summary>
    public void Timer1()
    {
        countImage2.gameObject.SetActive(false);
        countImage1.gameObject.SetActive(true);
    }

    /// <summary>
    /// 타이머 끝
    /// </summary>
    public void TimeOver()
    {
        countImage1.gameObject.SetActive(false);
        informationCanvas.gameObject.SetActive(false);
    }
}
