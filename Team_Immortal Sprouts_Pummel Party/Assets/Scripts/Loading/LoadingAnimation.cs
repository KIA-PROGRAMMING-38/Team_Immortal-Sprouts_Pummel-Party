using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class LoadingAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] loadingScene;
    private Image barImage;
    private Image SceneImage;



    private void OnEnable()
    {
        barImage = transform.GetChild(0).GetComponent<Image>();
        SceneImage = GetComponent<Image>();
    }

   // 애니메이션 이벤트 구독할 함수
    public void ChangeLoadingScene() 
    {
        barImage.fillAmount = 0;
        SceneImage.sprite = loadingScene[0];
    }

    public void InitLoadingScene()
    {
        SceneImage.sprite = loadingScene[2];
    }

    public void ChangeFlyingScene()
    {
        barImage.fillAmount = 0;
        SceneImage.sprite = loadingScene[1];
    }

    public void InitFlyingScene() 
    {
        SceneImage.sprite = loadingScene[3];
    }
}
