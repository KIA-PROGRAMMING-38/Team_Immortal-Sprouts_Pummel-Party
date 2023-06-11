using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlot : MonoBehaviourPunCallbacks
{
    [SerializeField] Image statusBar;
    [SerializeField] SelectCanvas selectCanvas;
    [SerializeField] CustomizeCanvas customizeCanvas;



    [PunRPC]
    public void EnableSelectCanvasButtons()
    {
        selectCanvas.EnableButtons();
    }

    public CustomizeCanvas GetCustomizeCanvas()
    {
        return customizeCanvas;
    }

    private bool isReady;
    
    private Color readyColor = Color.green;
    private Color notReadyColor = Color.red;
    public void ChangeColor(bool isReady)
    {
        if (isReady) // ready�� ���¶��
        {
            statusBar.color = readyColor;
        }
        else // ready�� �Ǿ����� �ʴٸ�
        {
            statusBar.color = notReadyColor;
        }
        
    }
}
