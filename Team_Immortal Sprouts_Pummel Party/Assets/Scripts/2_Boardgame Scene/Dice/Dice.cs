using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour
{
    [SerializeField] private GameObject diceTotal;
    [SerializeField] private BoardgamePlayer boardgamePlayer;
    public UnityEvent OnDiceStopped;
    private float rotateZSpeed = 800f; 
    private float rotateYSpeed = 800f;
    private bool isClickedBoardGameScreen = false;
    private bool isRollDice = false;

    private void OnEnable()
    {
        RollDice().Forget();
    }

    /// <summary>
    /// 플레이어의 터치 입력을 받아오는 함수입니다.
    /// </summary>
    public void OnTouchRollDicePanel()
    {
        isClickedBoardGameScreen = true;
    }

    /// <summary>
    /// 주사위의 값을 출력하는 함수입니다.
    /// </summary>
    public async UniTask RollDice()
    {
        int diceResult = 0;
        WaitFor1Seconds();
        OnAppearDice();

        while (true)
        {
            diceTotal.transform.Rotate(0, rotateYSpeed * Time.deltaTime, rotateZSpeed * Time.deltaTime);
            
            await UniTask.Yield();

            if (isClickedBoardGameScreen == true && isRollDice == false)
            {
                diceResult = Random.Range(-1, 8);
                diceTotal.transform.rotation = GetRotationValue(diceResult);
                isRollDice = true;
                Debug.Log($"주사위 결과: {diceResult}");
                await WaitFor1SecondsDisappearDice();
                boardgamePlayer.SetMoveCount(diceResult);
                OnDiceStopped?.Invoke();

                await UniTask.Yield();
            }
        }
    }

    Quaternion minusoneRotateValue = Quaternion.Euler(128, 156, 166);
    Quaternion zeroRotateValue = Quaternion.Euler(-21, 600, 142);
    Quaternion oneRotateValue = Quaternion.Euler(18, 296, 145);
    Quaternion twoRotateValue = Quaternion.Euler(-37, 518, 195);
    Quaternion threeRotateValue = Quaternion.Euler(1, 445, 219);
    Quaternion fourRotateValue = Quaternion.Euler(-25, 473, 40);
    Quaternion fiveRotateValue = Quaternion.Euler(27, 379, -168);
    Quaternion sixRotateValue = Quaternion.Euler(-37, 205, -18);
    Quaternion sevenRotateValue = Quaternion.Euler(0, 274, -38);
    Quaternion errorRotateValue = Quaternion.Euler(149, -109, 231); // 아무 숫자도 없는 면

    private Quaternion GetRotationValue(int diceResult)
    {
        switch (diceResult)
        {
            case -1:
                return minusoneRotateValue;
                break;
            case 0:
                return zeroRotateValue;
                break;
            case 1:
                return oneRotateValue;
                break;
            case 2:
                return twoRotateValue;
                break;
            case 3:
                return threeRotateValue;
                break;
            case 4:
                return fourRotateValue;
                break;
            case 5:
                return fiveRotateValue;
                break;
            case 6:
                return sixRotateValue;
                break;
            case 7:
                return sevenRotateValue;
                break;
            default:
                return errorRotateValue;
        }
    }

    private async UniTask WaitFor1SecondsDisappearDice()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        OnDisappearDice();
    }

    private async UniTask WaitFor1Seconds()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
    }

    /// <summary>
    /// 주사위를 활성화해주는 함수입니다. 
    /// </summary>
    public void OnAppearDice()
    {
        diceTotal.gameObject.SetActive(true);
        isClickedBoardGameScreen = false;
        isRollDice = false; 
    }

    /// <summary>
    /// 주사위를 비활성화해주는 함수입니다.
    /// </summary>
    public void OnDisappearDice()
    {
        diceTotal.gameObject.SetActive(false);
    }
}
