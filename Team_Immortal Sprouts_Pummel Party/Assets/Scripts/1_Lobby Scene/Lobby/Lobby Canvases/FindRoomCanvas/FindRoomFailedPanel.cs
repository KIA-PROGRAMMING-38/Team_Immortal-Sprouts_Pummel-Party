using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindRoomFailedPanel : MonoBehaviour
{
    /// <summary>
    /// �� ã�� ���и� �˸��� UI Panel�� Ȱ��ȭ
    /// </summary>
    public void Active()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// �� ã�� ���и� �˸��� UI Panel�� ��Ȱ��ȭ
    /// </summary>
    public void Deactive()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// �� ã�� ���� Panel�� OK ��ư �̺�Ʈ
    /// </summary>
    public void OnClick_OK()
    {
        Deactive();
    }
}
