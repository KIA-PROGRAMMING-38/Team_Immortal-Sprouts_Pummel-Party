using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Dice : MonoBehaviour
{
    [SerializeField] private GameObject dice;
    private BoardgamePlayer player;
    private float rotateZSpeed = 800f; 
    private float rotateYSpeed = 800f;
    private bool isClickedBoardGameScreen = false;
    private bool isRollDice = false;

    private void Awake()
    {
        player = GetComponent<BoardgamePlayer>();
    }

    /// <summary>
    /// 플레이어의 터치 입력을 받아오는 함수입니다.
    /// </summary>
    public void OnTouchRollDicePanel()
    {
        isClickedBoardGameScreen = true;
        Debug.Log("터치됨");
    }

    /// <summary>
    /// 주사위의 값을 출력하는 함수입니다.
    /// </summary>
    public void Roll()
    {
        int diceResult = 0;

        if (isClickedBoardGameScreen == false)
        {
            OnAppearDice();
            dice.transform.Rotate(0, rotateYSpeed * Time.deltaTime, rotateZSpeed * Time.deltaTime);
        }

        else if (isClickedBoardGameScreen == true && isRollDice == false)
        {
            diceResult = Random.Range(-1, 0);
            dice.transform.rotation = GetRotationValue(diceResult);
            isRollDice = true;
            Debug.Log($"주사위 결과: {diceResult}");
        }

        player.SetMoveCount(diceResult);
        //return diceResult;
    }

    private void Update()
    {
        Roll();
    }

    Quaternion minusoneRotateValue = Quaternion.Euler(128, -24, 166);
    Quaternion zeroRotateValue = Quaternion.Euler(-21, 420, 142);
    Quaternion oneRotateValue = Quaternion.Euler(18, 116, 145);
    Quaternion twoRotateValue = Quaternion.Euler(-37, 338, 195);
    Quaternion threeRotateValue = Quaternion.Euler(1, 265, 219);
    Quaternion fourRotateValue = Quaternion.Euler(-25, 293, 40);
    Quaternion fiveRotateValue = Quaternion.Euler(27, 199, -168);
    Quaternion sixRotateValue = Quaternion.Euler(-37, 25, -18);
    Quaternion sevenRotateValue = Quaternion.Euler(0, 94, -38);
    Quaternion errorRotateValue = Quaternion.Euler(24, -124, 4); // 아무 숫자도 없는 면

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

    /// <summary>
    /// 주사위를 활성화해주는 함수입니다. 
    /// </summary>
    public void OnAppearDice()
    {
        dice.gameObject.SetActive(true);
    }

    /// <summary>
    /// 주사위를 비활성화해주는 함수입니다.
    /// </summary>
    public void OnDisappearDice()
    {
        dice.gameObject.SetActive(false);
    }
}
