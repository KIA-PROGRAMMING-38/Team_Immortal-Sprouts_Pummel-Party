using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Dice : MonoBehaviour
{
    private float rotateSpeed = 1000f;
    private bool isClickedScreen = false;
    private PlayerInput playerInput;
    private InputAction touchPressAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        touchPressAction = playerInput.actions["ScreenTouch"];
    }

    private void OnTouchBoardGameScreen(InputAction.CallbackContext context)
    {
        Debug.Log("터치됨");
        isClickedScreen = true;
    }

    private void OnEnable()
    {
        touchPressAction.performed -= OnTouchBoardGameScreen;
        touchPressAction.performed += OnTouchBoardGameScreen;
    }

    private void OnDisable()
    {
        touchPressAction.performed -= OnTouchBoardGameScreen;
    }

    float elapsedTime = 0;
    int diceResult = 0;
    /// <summary>
    /// 임시 기능.. 화면 터치 입력 발생했을 때 주사위 굴릴 수 있도록..
    /// </summary>
    public int Roll()
    {
        elapsedTime += Time.deltaTime;
        Debug.Log("roll 함수 들어옴");
        Debug.Log($"elapsedTime = {elapsedTime}");

        // 주사위 회전
        // 임시 조건 지정
        while (elapsedTime > 5f)
        {
            Debug.Log("while문 들어옴");
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);

            if (/*isClickedScreen == true && */ elapsedTime > 10f)
            {
                Debug.Log("if문 들어옴");
                diceResult = Random.Range(-1, 8);
                gameObject.transform.GetChild(1).gameObject.transform.rotation = GetRotationValue(diceResult);
                Debug.Log($"주사위 결과: {diceResult}");
                gameObject.transform.GetChild(1).gameObject.SetActive(true);

                break;
            }

            // elapsedTime = 0;
        }

        return diceResult;
    }

    private void Update()
    {
        Roll();
    }

    Quaternion minusoneRotateValue = Quaternion.Euler(126, 425, 163);
    Quaternion zeroRotateValue = Quaternion.Euler(126, 72, 169);
    Quaternion oneRotateValue = Quaternion.Euler(19, -154, 136);
    Quaternion twoRotateValue = Quaternion.Euler(-32, 424, 190);
    Quaternion threeRotateValue = Quaternion.Euler(0, 353, 214);
    Quaternion fourRotateValue = Quaternion.Euler(327, 93, 93);
    Quaternion fiveRotateValue = Quaternion.Euler(31, 290, 194);
    Quaternion sixRotateValue = Quaternion.Euler(-48, 125, -29);
    Quaternion sevenRotateValue = Quaternion.Euler(0, 177, -48);
    Quaternion errorRotateValue = Quaternion.Euler(31, -745, 46); // 아무 숫자도 없는 면

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
    public void OnDisappearDice()
    {
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }

    /// <summary>
    /// 주사위를 비활성화해주는 함수입니다. 
    /// </summary>
    public void OnAppearDice()
    {
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
    }
}
