using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class Title : MonoBehaviour
{
    [SerializeField] private GameObject MultiPlayerCanvas;

    public void OnTouchTitle()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
        MultiPlayerCanvas.SetActive(true);
        Debug.Log("Touch Title");
    }    
}
