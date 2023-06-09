using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    [SerializeField] private Island nextIsland;
    [SerializeField] private RotationIsland curIsland;

    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnMouseUpAsButton() // ���Ӻ信�� �� ��ũ��Ʈ�� ��� �ִ� ���ӿ�����Ʈ�� ��ġ���� ��
    {
        // ���� �������� �����ش�
        Vector3 nextPosition = nextIsland.GetCurrentPosition();
        curIsland.SetNextPosition(nextPosition);
        
        // ȸ���� ������ ���ϰ� ȸ����Ų��
        Quaternion targetRotation = Quaternion.LookRotation(curIsland.GetCurrentPosition() - nextPosition);
        curIsland.ActivateRotatation(targetRotation);
    }
}
