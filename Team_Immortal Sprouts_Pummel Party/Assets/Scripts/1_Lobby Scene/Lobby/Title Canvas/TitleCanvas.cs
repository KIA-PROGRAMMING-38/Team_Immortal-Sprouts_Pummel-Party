using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class TitleCanvas : MonoBehaviour
{
    [SerializeField] private Canvas multiPlayerCanvas;
    [SerializeField] private GameObject touchGuide;

    /// <summary>
    /// Ÿ��Ʋ ĵ�������� �ƹ����̳� ��ġ�� ������ �۵��ϴ� �Լ�
    /// </summary>
    public void OnTouchTitle()
    {
        if (touchGuide.activeSelf)
        {
            multiPlayerCanvas.enabled = true;
            gameObject.SetActive(false);
        }
    }    
}
