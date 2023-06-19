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
        }
        else
        {
            Destroy(gameObject);
        }

        boardGameLoader = transform.GetChild(0).gameObject;
        miniGameLoader = transform.GetChild(1).gameObject;
    }
    /// <summary>
    /// ������� �ε�ȭ��
    /// </summary>
    public void BoardGameLoadPlay()
    {
        boardGameLoader.SetActive(true);
    }

    /// <summary>
    /// �̴ϰ��� �ε�ȭ��
    /// </summary>
    public void MiniGameLoadPlay()
    {
        miniGameLoader.SetActive(true);
    }

    
}
