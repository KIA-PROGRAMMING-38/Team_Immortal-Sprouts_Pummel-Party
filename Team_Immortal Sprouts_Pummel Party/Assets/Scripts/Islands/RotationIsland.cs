using Cinemachine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationIsland : Island
{
    [SerializeField] CinemachineVirtualCamera virtualCam;
    [SerializeField] [Range(1f, 2f)] private float rotateTime = 1.5f;
    private Transform playerTransform; // �÷��̾ �Բ� ȸ�������ֱ� ���� ����
    private Quaternion defaultRotation;
    private bool isRotationFinished;

    void Start()
    {
        InitPositionSettings().Forget();
        SaveInitialRotation();
    }


    /// <summary>
    /// ���� �Բ� ȸ����ų �÷��̾ �����ϴ� �Լ�
    /// </summary>
    /// <param name="playerTransform"></param>
    public void SetPlayerTransform(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }

    /// <summary>
    /// ���� ���� ȭ��ǥ �������� ȸ����Ű�� �Լ�
    /// </summary>
    /// <param name="targetRotation"></param>
    public void ActivateRotatation(Quaternion targetRotation)
    {
        Rotate(targetRotation).Forget();
    }

    /// <summary>
    /// ���� �ٽ� ���󺹱� ȸ����Ű�� �Լ�
    /// </summary>
    public void ActivateResetRotation()
    {
        Rotate(defaultRotation).Forget();
    }

    /// <summary>
    /// ȸ���� ���Ῡ�θ� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public bool GetRotationStatus()
    {
        return isRotationFinished;
    }

    private async UniTaskVoid Rotate(Quaternion targetRotation)
    {
        Quaternion initialRotation;
        bool isResetting = false;

        if (targetRotation == defaultRotation) // �ٽ� �ǵ��� ��
        {
            SetNextPosition(Vector3.zero); // ���� �������� �����ش�
            ResetPlayerTransform(); // ���� ȸ����ų �÷��̾ �����ش�
            initialRotation = transform.rotation;
            isResetting = true;
        }
        else // ȭ��ǥ�� ������ ��
        {
            initialRotation = defaultRotation;
        }

        float elapsedTime = 0f;

        if (playerTransform == null) // �÷��̾ ������ ���ٸ�
        {
            while (elapsedTime <= rotateTime) // ���� ȸ����Ų��
            {
                transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / rotateTime);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }
        }
        else // �÷��̾ ������ �����Ѵٸ�
        {
            while (elapsedTime <= rotateTime) // �÷��̾�� ���� ���� ȸ����Ų��
            {
                transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / rotateTime);
                playerTransform.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / rotateTime);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }
        }
        

        if (isResetting == false) // ȭ��ǥ ������ ������, ȸ���� ������ isRotationFinished�� true�� ������ش�
        {
            isRotationFinished = true;
        }
        else // �ٽ� ���󺹱� ��Ű�� ȸ���̶�� ���ʿ� isRotationFinished�� ����� ���� ����
        {
            isRotationFinished = false;
        }
    }

    private void SaveInitialRotation()
    {
        defaultRotation = transform.rotation;
    }

    private void ResetPlayerTransform()
    {
        playerTransform = null;
    }
}
