using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DucklingManager : MonoBehaviour, IMiniGame
{
    [SerializeField] private GameObject scanButton;
    [SerializeField] private Image gameTimer;
    [SerializeField] private float playTime = 90f;
    [SerializeField] private DucklingCage[] ducklingCages;
    [SerializeField] private PointLightController[] pointLights;
    [SerializeField] private GameObject endLights;
    
    private void Start()
    {
        SetTimer();
        activateScanButton(true);
    }

    private void activateScanButton(bool isPlaying)
    {
        scanButton.SetActive(isPlaying);
    }

    private async UniTask setGameTimer()
    {
        float min = 0f;
        float max = 1f;

        float elapsedTime = 0f;

        while (elapsedTime <= playTime)
        {
            gameTimer.fillAmount = Mathf.Lerp(min, max, elapsedTime / playTime);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        await UniTask.WaitUntil(() => 0.99f <= gameTimer.fillAmount);
        stopGame();
    }

    private void stopGame()
    {
        Debug.Log("���ӳ���");
        activateScanButton(false);
        for (int i = 0; i < pointLights.Length ;++i)
        {
            pointLights[i].enabled = false;
        }
        endLights.SetActive(true);
    }

    private void getDucklingCountsPerCage()
    {
        foreach (DucklingCage cage in ducklingCages)
        {
            //cage.GetDucklingCount(); �̰� ���߿� rank �ý��� �����Ǹ� �����������
        }
    }

    // ���߿� ���� �����ϰ� ������ �͵� 
    public void SetPlayer() 
    {
        
    }

    public void SetRank()
    {
        
    }

    public void SetTimer()
    {
        setGameTimer().Forget();
    }
}
