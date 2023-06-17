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
    [SerializeField] private GameObject diceTotal; // �ֻ���������Ʈ
    public UnityEvent OnDiceStopped;
    private float rotateZSpeed = 800f;
    private float rotateYSpeed = 800f;
    private bool isClickedBoardGameScreen = false;

    private void OnEnable()
    {
        OnAppearDice();
    }

    /// <summary>
    /// �÷��̾��� ��ġ �Է��� �޾ƿ��� �Լ��Դϴ�.
    /// </summary>
    public void OnTouchRollDicePanel() => isClickedBoardGameScreen = true;
    private const int minResult = -1;
    private const int maxResult = 8;
    private int diceResult; // �ֻ�����
    private async UniTaskVoid rotateDice() // �ֻ����� ȸ���ϴ� �Լ�
    {
        while (!isClickedBoardGameScreen) // ��ġ�� ������
        {
            diceTotal.transform.Rotate(0, rotateYSpeed * Time.deltaTime, rotateZSpeed * Time.deltaTime); // ��� ����
            await UniTask.Yield();
        }

        diceResult = Random.Range(minResult, maxResult);
        diceTotal.transform.rotation = returnRotationValue(diceResult);
        OnDiceStopped?.Invoke();
        waitFor1SecondsDisappearDice().Forget();
    }

    public int ConveyDiceReuslt() => diceResult;

    private Quaternion returnRotationValue(int diceValue) // �ֻ������� �ش��ϴ� ȸ������ ��ȯ�ϴ� �Լ�
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
    /// �ֻ����� Ȱ��ȭ���ִ� �Լ��Դϴ�.
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
        Quaternion.Euler(128, 156, 166), // �ֻ������� -1 �� ���������� ȸ����
        Quaternion.Euler(-21, 600, 142), // �ֻ������� 0 �� ���������� ȸ����
        Quaternion.Euler(18, 296, 145), // �ֻ������� 1 �� ���������� ȸ����
        Quaternion.Euler(-37, 518, 195), // �ֻ������� 2 �� ���������� ȸ����
        Quaternion.Euler(1, 445, 219), // �ֻ������� 3 �� ���������� ȸ����
        Quaternion.Euler(-25, 473, 40), // �ֻ������� 4 �� ���������� ȸ����
        Quaternion.Euler(27, 379, -168), // �ֻ������� 5 �� ���������� ȸ����
        Quaternion.Euler(-37, 205, -18), // �ֻ������� 6 �� ���������� ȸ����
        Quaternion.Euler(0, 274, -38), // �ֻ������� 7 �� ���������� ȸ����
    };
}