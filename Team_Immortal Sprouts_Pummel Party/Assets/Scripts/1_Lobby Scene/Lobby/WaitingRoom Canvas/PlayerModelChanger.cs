using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelChanger : MonoBehaviourPunCallbacks
{
    private GameObject currentHat; 
    [SerializeField] private Transform hatTransform;
    [SerializeField] private MeshRenderer[] bodyMeshRenderer;
    [SerializeField] private CustomData customData;

    private void Awake()
    {
    }


    /// <summary>
    /// 플레이어의 모자위치를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public Vector3 GetHatPosition()
    {
        return hatTransform.position;
    }

    /// <summary>
    /// 플레이어의 현재 모자를 반환하는 함수
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
    /// 플레이어의 몸색을 직접 바꿔주는 함수
    /// </summary>
    /// <param name="bodyColorIndex"></param>
    [PunRPC]
    private void SetBodyColor(int bodyColorIndex)
    {
        string bodyMaterialPath = Managers.DataManager.Player.BodyDialog[bodyColorIndex]["Name"].ToString();
        Material newBodyMaterial = Resources.Load<Material>(bodyMaterialPath);
        foreach (MeshRenderer mesh in bodyMeshRenderer)
        {
            mesh.material = newBodyMaterial;
        }
    }

    /// <summary>
    /// 플레이어의 모자를 직접 바꿔주는 함수
    /// </summary>
    /// <param name="hatIndex"></param>
    [PunRPC]
    public void SetHatOnPlayer(int hatIndex)
    {
        if (currentHat != null)
        {
            Managers.PrefabManager.Destroy(currentHat);
        }

        string hatPath = Managers.DataManager.Player.HatDialog[hatIndex]["Name"].ToString();
        if (hatPath != "-")
        {
            GameObject newHat = Managers.PrefabManager.Instantiate(hatPath, GetHatPosition(), Quaternion.identity);
            newHat.transform.parent = hatTransform;
            newHat.SetActive(true);
            currentHat = newHat;
        }
    }

    /// <summary>
    /// 현재 모자를 삭제해주는 함수
    /// </summary>
    public void RemoveCurrentHat()
    {
        Managers.PrefabManager.Destroy(currentHat);
    }


}
