using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class Title : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction touchAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        touchAction = playerInput.actions["Touch"];
    }

    private void OnEnable()
    {
        touchAction.performed -= TouchTitle;
        touchAction.performed += TouchTitle;
    }

    private void OnDisable()
    {
        touchAction.performed -= TouchTitle;
    }

    private void TouchTitle(InputAction.CallbackContext value)
    {
        gameObject.transform.parent.gameObject.SetActive(false);
        Debug.Log("Touch Title");
    }
}
