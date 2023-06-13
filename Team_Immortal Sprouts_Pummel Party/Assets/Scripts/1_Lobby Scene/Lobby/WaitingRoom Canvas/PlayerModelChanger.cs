using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelChanger : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject currentHat; // 테스트 위해 SerializeField 추가함
    [SerializeField] private Transform hatTransform;
    [SerializeField] private Material bodyMaterial;
    

    public Vector3 GetHatPosition()
    {
        return hatTransform.position;
    }

    public void SetBodyColor(Texture2D selectedTexture)
    {
        bodyMaterial.mainTexture = selectedTexture;
        
    }

    public GameObject GetCurrentHat()
    {
        if (currentHat != null)
        {
            return currentHat;
        }

        return null;
    }

    public void SetHatOnPlayer(GameObject selectedHat)
    {
        if (selectedHat != null)
        {
            selectedHat.transform.parent = hatTransform;
            selectedHat.SetActive(true);
        }
        
        currentHat = selectedHat;
    }
}
