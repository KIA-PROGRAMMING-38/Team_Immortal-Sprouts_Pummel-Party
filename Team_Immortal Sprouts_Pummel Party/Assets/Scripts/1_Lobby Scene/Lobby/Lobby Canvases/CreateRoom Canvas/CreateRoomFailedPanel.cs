using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoomFailedPanel : MonoBehaviour
{
    /// <summary>
    /// �� ���� ���и� �˸��� UI Panel�� Ȱ��ȭ
    /// </summary>
    public void Active()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// �� ���� ���и� �˸��� UI Panel�� ��Ȱ��ȭ
    /// </summary>
    public void Deactive()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// �� ���� ���� Panel�� OK ��ư �̺�Ʈ
    /// </summary>
    public void OnClick_OK()
    {
        Deactive();
    }
}
