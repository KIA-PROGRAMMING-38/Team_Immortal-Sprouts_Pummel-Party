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
    public DefaultPool defaultPrefabPool { get; private set; } // 포톤에서 제공하는 오브젝트 풀

    private void Awake()
    {
        defaultPrefabPool = PhotonNetwork.PrefabPool as DefaultPool;
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
        //Texture2D bodyColor = customData.GetBodyColorFromData(bodyColorIndex);
        string bodyTexturePath = RootManager.DataManager.Player.BodyDialog[bodyColorIndex]["Name"].ToString();
        Texture2D bodyColor = Resources.Load<Texture2D>(bodyTexturePath);
        bodyMaterial.mainTexture = bodyColor;
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
            //defaultPrefabPool.Destroy(currentHat);
            RootManager.PrefabManager.Destroy(currentHat);
        }

        GameObject newHat = customData.GetHatFromData(hatIndex);
        
        if (newHat != null)
        {
            //newHat = defaultPrefabPool.Instantiate(newHat.name, GetHatPosition(), Quaternion.identity);
            string hatPath = RootManager.DataManager.Player.HatDialog[hatIndex]["Name"].ToString(); 
            newHat = RootManager.PrefabManager.Instantiate(hatPath, GetHatPosition(), Quaternion.identity);
            newHat.transform.parent = hatTransform;
            newHat.SetActive(true);
        }
        
        currentHat = newHat;
    }

    /// <summary>
    /// 현재 모자를 삭제해주는 함수
    /// </summary>
    public void RemoveCurrentHat()
    {
        defaultPrefabPool.Destroy(currentHat);
    }


}
