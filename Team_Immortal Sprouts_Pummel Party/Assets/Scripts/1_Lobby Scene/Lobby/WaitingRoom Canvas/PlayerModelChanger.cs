using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelChanger : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject currentHat; // 테스트 위해 SerializeField 추가함
    [SerializeField] private Transform hatTransform;
    [SerializeField] private Material bodyMaterial;
    [SerializeField] private CustomData customData;
    public DefaultPool defaultPrefabPool { get; private set; }

    private void Awake()
    {
        defaultPrefabPool = PhotonNetwork.PrefabPool as DefaultPool;
    }


    public Vector3 GetHatPosition()
    {
        return hatTransform.position;
    }

    public GameObject GetCurrentHat()
    {
        if (currentHat != null)
        {
            return currentHat;
        }

        return null;
    }

    [PunRPC]
    private void SetBodyColor(int bodyColorIndex)
    {
        Texture2D bodyColor = customData.GetBodyColorFromData(bodyColorIndex);
        bodyMaterial.mainTexture = bodyColor;
    }

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



}
