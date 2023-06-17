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
    [SerializeField] private GameObject diceTotal; // 주사위오브젝트
    public UnityEvent OnDiceStopped;
    private float rotateZSpeed = 800f;
    private float rotateYSpeed = 800f;
    private bool isClickedBoardGameScreen = false;

    private void OnEnable()
    {
        OnAppearDice();
    }

    /// <summary>
    /// 플레이어의 터치 입력을 받아오는 함수입니다.
    /// </summary>
    public void OnTouchRollDicePanel() => isClickedBoardGameScreen = true;
    private const int minResult = -1;
    private const int maxResult = 8;
    private int diceResult; // 주사위값
    private async UniTaskVoid rotateDice() // 주사위가 회전하는 함수
    {
        while (!isClickedBoardGameScreen) // 터치가 없으면
        {
            diceTotal.transform.Rotate(0, rotateYSpeed * Time.deltaTime, rotateZSpeed * Time.deltaTime); // 계속 돌아
            await UniTask.Yield();
        }

        diceResult = Random.Range(minResult, maxResult);
        diceTotal.transform.rotation = returnRotationValue(diceResult);
        OnDiceStopped?.Invoke();
        waitFor1SecondsDisappearDice().Forget();
    }

    public int ConveyDiceReuslt() => diceResult;

    private Quaternion returnRotationValue(int diceValue) // 주사위값에 해당하는 회전값을 반환하는 함수
    {
        int realIndex = diceValue + 1;
        return rotationValues[realIndex];
    }

    private async UniTaskVoid waitFor1SecondsDisappearDice()
    {
        await WaitForSeconds(1f);
        diceResult = 0;
        onDisappearDice();
    }

    private async UniTask WaitForSeconds(float waitTime) => await UniTask.Delay(TimeSpan.FromSeconds(waitTime));

    /// <summary>
    /// 주사위를 활성화해주는 함수입니다.
    /// </summary>
    public void OnAppearDice()
    {
        diceTotal.gameObject.SetActive(true);
        isClickedBoardGameScreen = false;
        rotateDice().Forget();
    }

    private void onDisappearDice() => diceTotal.gameObject.SetActive(false);

    private Quaternion[] rotationValues = new Quaternion[]
    {
        Quaternion.Euler(128, 156, 166), // 주사위값이 -1 이 나왔을때의 회전값
        Quaternion.Euler(-21, 600, 142), // 주사위값이 0 이 나왔을때의 회전값
        Quaternion.Euler(18, 296, 145), // 주사위값이 1 이 나왔을때의 회전값
        Quaternion.Euler(-37, 518, 195), // 주사위값이 2 이 나왔을때의 회전값
        Quaternion.Euler(1, 445, 219), // 주사위값이 3 이 나왔을때의 회전값
        Quaternion.Euler(-25, 473, 40), // 주사위값이 4 이 나왔을때의 회전값
        Quaternion.Euler(27, 379, -168), // 주사위값이 5 이 나왔을때의 회전값
        Quaternion.Euler(-37, 205, -18), // 주사위값이 6 이 나왔을때의 회전값
        Quaternion.Euler(0, 274, -38), // 주사위값이 7 이 나왔을때의 회전값
    };
}