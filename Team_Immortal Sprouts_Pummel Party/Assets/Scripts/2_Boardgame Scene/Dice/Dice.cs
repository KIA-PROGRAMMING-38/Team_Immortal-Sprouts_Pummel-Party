using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Dice : MonoBehaviour
{
    [SerializeField] private GameObject dice;
    private float rotateZSpeed = 800f;
    private float rotateYSpeed = 800f;
    private bool isClickedBoardGameScreen = false;
    private PlayerInput playerInput;
    private InputAction touchPressAction;

    private void Awake()
    {
        //playerInput = GetComponent<PlayerInput>();
        //touchPressAction = playerInput.actions["ScreenTouch"];
    }

    public void OnTouchRollDicePanel()
    {
        isClickedBoardGameScreen = true;
        Debug.Log("��ġ��");
    }

    //private void OnEnable()
    //{
    //    touchPressAction.performed -= OnTouchBoardGameScreen;
    //    touchPressAction.performed += OnTouchBoardGameScreen;
    //}

    //private void OnDisable()
    //{
    //    touchPressAction.performed -= OnTouchBoardGameScreen;
    //}

    float elapsedTime = 0;
    float rotationTime = 10f;

    /// <summary>
    /// �ӽ� ���.. ȭ�� ��ġ �Է� �߻����� �� �ֻ��� ���� �� �ֵ���..
    /// </summary>
    public int Roll()
    {
        //int diceResult = 0;
        //elapsedTime += Time.deltaTime;

        //// �ֻ��� ȸ��
        //// �ӽ� ���� ����
        //while (elapsedTime <= rotationTime)
        //{
        //    OnAppearDice();
        //    dice.transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);

        //    if (isClickedBoardGameScreen == true)
        //    {
        //        Debug.Log("if�� ����");
        //        diceResult = Random.Range(-1, 8);
        //        dice.transform.rotation = GetRotationValue(diceResult);
        //        Debug.Log($"�ֻ��� ���: {diceResult}");

        //        break;
        //    }
        //}
        int diceResult = 0;

        if (isClickedBoardGameScreen == false)
        {
            OnAppearDice();
            dice.transform.Rotate(0, rotateYSpeed * Time.deltaTime, rotateZSpeed * Time.deltaTime);
        }

        else
        {
            diceResult = Random.Range(-1, 8);
            dice.transform.rotation = GetRotationValue(diceResult);
            Debug.Log($"�ֻ��� ���: {diceResult}");
        }

        //OnDisappearDice();

        // elapsedTime = 0;
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
    Quaternion errorRotateValue = Quaternion.Euler(31, -745, 46); // �ƹ� ���ڵ� ���� ��

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
    /// �ֻ����� ��Ȱ��ȭ���ִ� �Լ��Դϴ�.
    /// </summary>
    public void OnDisappearDice()
    {
        dice.gameObject.SetActive(false);
    }

    /// <summary>
    /// �ֻ����� Ȱ��ȭ���ִ� �Լ��Դϴ�. 
    /// </summary>
    public void OnAppearDice()
    {
        dice.gameObject.SetActive(true);
    }
}
