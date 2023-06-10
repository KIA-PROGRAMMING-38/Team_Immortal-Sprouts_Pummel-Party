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
    //private PlayerInput playerInput;
    //private InputAction touchAction;

    //private void Awake()
    //{
    //    playerInput = GetComponent<PlayerInput>();
    //    touchAction = playerInput.actions["Touch"];
    //}

    //private void OnEnable()
    //{
    //    touchAction.performed -= TouchTitle;
    //    touchAction.performed += TouchTitle;
    //}

    //private void OnDisable()
    //{
    //    touchAction.performed -= TouchTitle;
    //}

    //private void TouchTitle(InputAction.CallbackContext value)
    //{
    //    gameObject.transform.parent.gameObject.SetActive(false);
    //    MultiPlayerCanvas.SetActive(true);
    //    Debug.Log("Touch Title");
    //}

    public void OnTouchTitle()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
        MultiPlayerCanvas.SetActive(true);
        Debug.Log("Touch Title");
    }    
}
