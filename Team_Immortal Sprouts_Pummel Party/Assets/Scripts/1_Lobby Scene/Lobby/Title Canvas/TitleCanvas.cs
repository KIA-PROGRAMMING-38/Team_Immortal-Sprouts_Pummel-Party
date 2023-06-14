using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class TitleCanvas : MonoBehaviour
{
    [SerializeField] private Canvas MultiPlayerCanvas;

    /// <summary>
    /// 타이틀 캔버스에서 아무곳이나 터치를 했을때 작동하는 함수
    /// </summary>
    public void OnTouchTitle()
    {
        MultiPlayerCanvas.enabled = true;
        gameObject.SetActive(false);
        Debug.Log("Touch Title");
    }    
}
