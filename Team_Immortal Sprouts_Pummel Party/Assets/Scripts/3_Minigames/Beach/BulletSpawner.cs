using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] Bullet bullet;

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            Instantiate(bullet, transform);
        }
        BulletSpawn().Forget();
    }
    private async UniTaskVoid BulletSpawn()
    {
        while(true)
        {
            await UniTask.Delay(10000);
            for (int i = 0; i < 5; i++)
            {
                Instantiate(bullet,transform);
            }
        }
    }
}
