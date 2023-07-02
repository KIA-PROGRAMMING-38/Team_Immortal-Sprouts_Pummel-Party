using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Dictionary<string, object>> miniGameDialogs;
    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        miniGameDialogs = CSVReader.Read("MiniGameInfo");
    }

    public void SellectMiniGameNumber()
    {
        PhotonNetwork.CurrentRoom.CustomProperties[Propertise.miniGameKey] =
            Random.Range((int)miniGameDialogs.First()["Num"], (int)miniGameDialogs.Last()["Num"] + 1);
    }
}
