using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Dictionary<string, string>> miniGameDialog;
    public int minigameCount;
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
        miniGameDialog = CSVReader<string>.Read("MinigameInfo");
        minigameCount = miniGameDialog.Count;
    }

    private void Update()
    {
        Debug.Log(SetMiniGameNumber());
    }

    public int SetMiniGameNumber()
    {
        return Random.Range(int.Parse(miniGameDialog.First()["Num"]),int.Parse(miniGameDialog.Last()["Num"]));
    }
}
