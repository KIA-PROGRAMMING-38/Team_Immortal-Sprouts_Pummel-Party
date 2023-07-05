using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    private GameObject boardGameLoader;
    private GameObject miniGameLoader;
    private void Awake()
    {
        var loadAnime = FindObjectsOfType<LoadingScene>();

        if (loadAnime.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
            WrapObject.SCENE_CHANGER = this;
        }
        else
        {
            Destroy(gameObject);
        }

        boardGameLoader = transform.GetChild(0).gameObject;
        miniGameLoader = transform.GetChild(1).gameObject;
    }
    /// <summary>
    /// 보드게임 로드화면
    /// </summary>
    public void BoardGameLoadPlay()
    {
        boardGameLoader.SetActive(true);
    }

    /// <summary>
    /// 미니게임 로드화면
    /// </summary>
    public void MiniGameLoadPlay()
    {
        miniGameLoader.SetActive(true);
    }

    
}
