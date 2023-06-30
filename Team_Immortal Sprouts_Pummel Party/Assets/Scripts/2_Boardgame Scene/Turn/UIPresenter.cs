using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BoardGamePresenter : MonoBehaviour
{
    [SerializeField] private TMP_Text miniGameName;
    [SerializeField] private TMP_Text miniGameInfo;

  //  public UnityEvent MiniGameStart;

    public void SetInfo()
    {
        miniGameName.text = GameManager.Instance.miniGameDialog[GameManager.Instance.minigameCount]["Name"].ToString();
        miniGameInfo.text = GameManager.Instance.miniGameDialog[GameManager.Instance.minigameCount]["Text"].ToString();
    }
}
