using Cinemachine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera playerCamera;
    public async UniTaskVoid CameraMoveAsync()
    {
        await UniTask.Delay(1000);
        transform.GetChild(0).gameObject.SetActive(true);
        await UniTask.Delay(9000);
        transform.GetChild(1).gameObject.SetActive(true);
        await UniTask.Delay(9000);
        playerCamera.gameObject.SetActive(true);
    }

    public void CameraMove()
    {
        CameraMoveAsync().Forget();
    }
}
