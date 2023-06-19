using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviourPunCallbacks
{
    private Image barImage;
    private Image sceneImage;
    private Animator loaderAnime;

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
        sceneImage.sprite = loadingScene[0];
    }

    public void InitLoadingScene()
    {
        sceneImage.sprite = loadingScene[2];
    }

    public void ChangeFlyingScene()
    {
        barImage.fillAmount = 0;
        sceneImage.sprite = loadingScene[1];
    }

    public void InitFlyingScene() 
    {
        sceneImage.sprite = loadingScene[3];
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
