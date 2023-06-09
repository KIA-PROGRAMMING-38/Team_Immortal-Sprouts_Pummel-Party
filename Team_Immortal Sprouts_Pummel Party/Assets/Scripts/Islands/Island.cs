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


    [SerializeField] private Vector3 nextPosition; // �׽�Ʈ ���� SerializeField �Է�
    [SerializeField] private Vector3 prevPosition; // �׽�Ʈ ���� SerializeField �Է�
    [SerializeField] private Vector3 currentPosition; // �׽�Ʈ ���� SerializeField �Է�



    public Vector3 GetCurrentPosition()
    {
        Debug.Assert(currentPosition != null);
        return currentPosition;
    }

    public Vector3 GetNextPosition()
    {
        Debug.Assert(nextPosition != null);
        return nextPosition;
    }

    public Vector3 GetPrevPosition()
    {
        Debug.Assert(prevPosition != null);
        return prevPosition;
    }

    public void SetNextPosition()
    {

    }

    public void SetPrevPosition()
    {

    }

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
