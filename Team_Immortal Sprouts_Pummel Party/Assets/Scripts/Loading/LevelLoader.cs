using Cysharp.Threading.Tasks;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviourPunCallbacks
{
    private Image barImage;
    private Image sceneImage;
    private Animator loaderAnime;
    private const int NON_COMPLETE_BOARD_SCENE = 0;
    private const int NON_COMPLETE_MINI_SCENE = 1;
    private const int COMPLETE_BOARD_SCENE = 2;
    private const int COMPLETE_MINI_SCENE = 3;

    private void Awake()
    {
        barImage = transform.GetChild(0).GetComponent<Image>();
        sceneImage = GetComponent<Image>();
        loaderAnime = GetComponent<Animator>();
    }
    public override void OnEnable()
    {
        ViewProgress().Forget();
        ViewScene().Forget();
    }


    #region AnimationEvent

    [SerializeField] private Sprite[] loadingScene;

    public void ChangeLoadingScene() 
    {
        barImage.fillAmount = 0;
        sceneImage.sprite = loadingScene[NON_COMPLETE_BOARD_SCENE];
    }

    public void InitLoadingScene()
    {
        sceneImage.sprite = loadingScene[COMPLETE_BOARD_SCENE];
    }
    
    public void ChangeFlyingScene()
    {
        barImage.fillAmount = 0;
        sceneImage.sprite = loadingScene[NON_COMPLETE_MINI_SCENE];
    }

    public void InitFlyingScene() 
    {
        sceneImage.sprite = loadingScene[COMPLETE_MINI_SCENE];
    }

    public void DeActive()
    {
        gameObject.SetActive(false);
    }

    #endregion
    public async UniTaskVoid ViewProgress()
    {
        while (true)
        {
            await UniTask.Delay(1000);
            barImage.fillAmount = PhotonNetwork.LevelLoadingProgress * 100;
            
            if (PhotonNetwork.LevelLoadingProgress >= 1)
            {
                return;
            }
        }
    }

    public async UniTaskVoid ViewScene()
    {
        await UniTask.WaitUntil(() => PhotonNetwork.LevelLoadingProgress >= 1);
        await UniTask.Delay(1000);
        loaderAnime.SetTrigger("Complete");
    }
}
