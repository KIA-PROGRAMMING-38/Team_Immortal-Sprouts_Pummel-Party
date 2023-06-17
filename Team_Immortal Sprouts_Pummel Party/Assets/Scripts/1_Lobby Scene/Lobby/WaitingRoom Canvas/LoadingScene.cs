using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    private GameObject loadingPanel;
    private void Awake()
    {
        var loadAnime = FindObjectOfType<LoadingScene>();
        if (loadAnime != null)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        loadingPanel = transform.GetChild(0).gameObject;
    }

    public void PlayLoadAnime()
    {
        loadingPanel.SetActive(true);
        
    }
}
