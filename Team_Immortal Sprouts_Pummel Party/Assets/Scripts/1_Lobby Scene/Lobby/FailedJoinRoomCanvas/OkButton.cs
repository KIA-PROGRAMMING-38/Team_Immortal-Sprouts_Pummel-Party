using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkButton : MonoBehaviour
{
    [SerializeField] private GameObject MultiGameCanvas;
    public void OnClickOkButton()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
        MultiGameCanvas.gameObject.SetActive(true);
    }
}
