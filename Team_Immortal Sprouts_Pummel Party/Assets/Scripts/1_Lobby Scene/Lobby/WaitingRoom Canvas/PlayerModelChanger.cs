using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelChanger : MonoBehaviourPunCallbacks
{
    private GameObject currentHat; 
    [SerializeField] private Transform hatTransform;
    [SerializeField] private Material bodyMaterial;
    [SerializeField] private CustomData customData;
    public DefaultPool defaultPrefabPool { get; private set; } // ���濡�� �����ϴ� ������Ʈ Ǯ

    private void Awake()
    {
        defaultPrefabPool = PhotonNetwork.PrefabPool as DefaultPool;
    }


    /// <summary>
    /// �÷��̾��� ������ġ�� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public Vector3 GetHatPosition()
    {
        return hatTransform.position;
    }

    /// <summary>
    /// �÷��̾��� ���� ���ڸ� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public GameObject GetCurrentHat()
    {
        if (currentHat != null)
        {
            return currentHat;
        }

        return null;
    }

    /// <summary>
    /// �÷��̾��� ������ ���� �ٲ��ִ� �Լ�
    /// </summary>
    /// <param name="bodyColorIndex"></param>
    [PunRPC]
    private void SetBodyColor(int bodyColorIndex)
    {
        Texture2D bodyColor = customData.GetBodyColorFromData(bodyColorIndex);
        bodyMaterial.mainTexture = bodyColor;
    }

    /// <summary>
    /// �÷��̾��� ���ڸ� ���� �ٲ��ִ� �Լ�
    /// </summary>
    /// <param name="hatIndex"></param>
    [PunRPC]
    public void SetHatOnPlayer(int hatIndex)
    {
        if (currentHat != null)
        {
            defaultPrefabPool.Destroy(currentHat);
        }

        GameObject newHat = customData.GetHatFromData(hatIndex);
        
        if (newHat != null)
        {
            newHat = defaultPrefabPool.Instantiate(newHat.name, GetHatPosition(), Quaternion.identity);
            newHat.transform.parent = hatTransform;
            newHat.SetActive(true);
        }
        
        currentHat = newHat;
    }

    /// <summary>
    /// ���� ���ڸ� �������ִ� �Լ�
    /// </summary>
    public void RemoveCurrentHat()
    {
        defaultPrefabPool.Destroy(currentHat);
    }


}
