using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardGameFrameWork : MonoBehaviour
{
    public UnityEvent OnFirtstStartBoardGame;

    public UnityEvent OnStartBoardGame;

    public UnityEvent TurnOnPlayer;

    public UnityEvent TurnOffPlayer;

    public UnityEvent OnAllPlayerTurnOff;
}
