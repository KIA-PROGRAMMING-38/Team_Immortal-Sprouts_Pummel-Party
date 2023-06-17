using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishRotate : MonoBehaviour
{
    [SerializeField] private Transform targetIsland;
    [SerializeField] private float rotateSpeed = 15f;
    private Vector3 rotateAxis = Vector3.up;

    private void Start()
    {
        fishRotation().Forget();
    }

    

    private async UniTaskVoid fishRotation()
    {
        while (this != null)
        {
            transform.RotateAround(targetIsland.position, rotateAxis, rotateSpeed * Time.deltaTime);
            await UniTask.Yield();
        }
    }
}
