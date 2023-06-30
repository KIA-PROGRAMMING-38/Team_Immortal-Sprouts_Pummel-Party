using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BoardGamePresenter : MonoBehaviour
{
    [SerializeField] private TMP_Text miniGameName;
    [SerializeField] private TMP_Text miniGameInfo;


    public UnityEvent OnMiniGameStart;

   
    public void SetInfo()
    {
        miniGameName.text = GameManager.Instance minigameDialog[gameNumber]["Name"].ToString();
        miniGameInfo.text = minigameDialog[gameNumber]["Text"].ToString();
    }
}
