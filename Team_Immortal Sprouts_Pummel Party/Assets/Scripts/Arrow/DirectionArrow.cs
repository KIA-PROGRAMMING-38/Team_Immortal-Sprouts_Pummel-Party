using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    [SerializeField] private ArrowSwitch arrowSwitch;
    [SerializeField] private Island nextIsland;
    [SerializeField] private RotationIsland curIsland;
    [SerializeField] [Range(1f, 3f)] private float appearTime = 2f;
    private Vector3 initialPosition;
    private Vector3 downPosition;
    private bool isTouchable;

    private void Awake()
    {
        initialPosition = transform.position;
        downPosition.Set(initialPosition.x, initialPosition.y - 3f, initialPosition.z);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ArrowAppear();
    }


    private void OnMouseUpAsButton() // ���Ӻ信�� �� ��ũ��Ʈ�� ��� �ִ� ���ӿ�����Ʈ�� ��ġ���� ��
    {
        if (isTouchable) // ��ġ�� ������ ������ ��
        {
            arrowSwitch.TurnOffSwitch(); // ���� ȭ��ǥ�� ���ش�

            // ���� �������� �����ش�
            Vector3 nextPosition = nextIsland.GetCurrentPosition();
            curIsland.SetNextPosition(nextPosition);

            // ȸ���� ������ ���ϰ� ȸ����Ų��
            Quaternion targetRotation = Quaternion.LookRotation(curIsland.GetCurrentPosition() - nextPosition);
            curIsland.ActivateRotatation(targetRotation);
        }
    }

    /// <summary>
    /// ���� ȭ��ǥ�� ���� �Ʒ����� �ö���� �Լ�
    /// </summary>
    public void ArrowAppear()
    {
        ArrowGoesUp().Forget();
    }

    /// <summary>
    /// ���� ȭ��ǥ�� ���� �Ʒ��� �������� �Լ�
    /// </summary>
    public void ArrowDisappear()
    {
        ArrowGoesDown().Forget();
    }

    private async UniTaskVoid ArrowGoesUp()
    {
        float elapsedTime = 0f;

        while (elapsedTime <= appearTime)
        {
            transform.position = Vector3.Lerp(downPosition, initialPosition, elapsedTime / appearTime);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        isTouchable = true; // �� �ö���� ��ġ�� ������ ���·� ������ش�
    }

    private async UniTaskVoid ArrowGoesDown()
    {
        float elapsedTime = 0f;

        isTouchable = false; // �������� ���� ��ġ�� ���� ���ϰ� ������ش�

        while (elapsedTime <= appearTime)
        {
            transform.position = Vector3.Lerp(initialPosition, downPosition, elapsedTime / appearTime);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        
        gameObject.SetActive(false);
    }
}
