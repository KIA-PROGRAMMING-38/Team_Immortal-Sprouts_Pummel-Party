using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Island : MonoBehaviour
{
    [SerializeField] Island nextPlayerSpot;
    [SerializeField] Island prevPlayerSpot;
    [SerializeField] Transform currentPlayerSpot;

    [SerializeField] private bool isPlayerPresent; // �׽�Ʈ ���� SerializeField �Է�
    [SerializeField] private Vector3 nextPosition; // �׽�Ʈ ���� SerializeField �Է�
    [SerializeField] private Vector3 prevPosition; // �׽�Ʈ ���� SerializeField �Է�
    [SerializeField] private Vector3 currentPosition; // �׽�Ʈ ���� SerializeField �Է�

    /// <summary>
    /// ���� ���� �÷��̾� ���翩�θ� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public bool GetPlayerPresence()
    {
        return isPlayerPresent;
    }

    /// <summary>
    /// ���� ���� �÷��̾� ���翩�θ� ������ �� �ִ� �Լ�
    /// </summary>
    /// <param name="_isPlayerOut"></param>
    public void SetPlayerPresence(bool _isPlayerOut)
    {
        isPlayerPresent = _isPlayerOut;
    }

    /// <summary>
    /// ���� ���� �÷��̾� ��ġ�� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCurrentPosition()
    {
        return currentPosition;
    }

    /// <summary>
    /// �÷��̾ ���� ���� ���� ��ġ�� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public Vector3 GetNextPosition()
    {
        Debug.Assert(nextPosition != null);
        return nextPosition;
    }

    /// <summary>
    /// �÷��̾ ���� ���� ���� ��ġ�� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPrevPosition()
    {
        Debug.Assert(prevPosition != null);
        return prevPosition;
    }

    /// <summary>
    /// ���� ���� �÷��̾� ��ġ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="_position"></param>
    public void SetCurrentPosition(Vector3 _position)
    {
        currentPosition = _position;
    }

    /// <summary>
    /// �÷��̾ ���� ���� ���� ��ġ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="_nextPosition"></param>
    public void SetNextPosition(Vector3 _nextPosition)
    {
        nextPosition = _nextPosition;
    }

    /// <summary>
    /// �÷��̾ ���� ���� ���� ��ġ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="_prevPosition"></param>
    public void SetPrevPosition(Vector3 _prevPosition)
    {
        prevPosition = _prevPosition;
    }

    /// <summary>
    /// ���缶�� ����, �������� ��ġ�� �ʱ�ȭ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    protected async UniTaskVoid InitPositionSettings()
    {
        if (currentPlayerSpot != null)
            currentPosition = currentPlayerSpot.position;

        await UniTask.Delay(TimeSpan.FromSeconds(2)); // ��� ������ currentPosition�� ����������� ��ٸ��� ���Ͽ� delay ��

        if (nextPlayerSpot != null)
            nextPosition = nextPlayerSpot.GetCurrentPosition();

        if (prevPlayerSpot != null)
            prevPosition = prevPlayerSpot.GetCurrentPosition();
    }

    

}
