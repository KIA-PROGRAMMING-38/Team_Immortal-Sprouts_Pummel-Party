using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
   public void SceneChange()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void CompleteChange()
    {
        gameObject.SetActive(false);
    }
}
