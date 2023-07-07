using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TitleDuck : MonoBehaviour
{
    [SerializeField] private Transform totalTransform;

    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float moveSpeed = 2f;


    private Vector3 moveVector;
    private void Start()
    {
        moveVector = Vector3.forward;
        DuckMove().Forget();
    }

    private void OnBecameInvisible()
    {
        TurnBack().Forget();
    }

    private async UniTaskVoid DuckMove()
    {
        moveSpeed = Random.Range(minSpeed, maxSpeed);
        while (totalTransform != null)
        {
            totalTransform.Translate(moveVector * moveSpeed * Time.deltaTime);
            await UniTask.Yield();
        }
    }

    private async UniTaskVoid TurnBack()
    {
        await UniTask.Delay(1000);
        moveSpeed = Random.Range(minSpeed, maxSpeed);

        if (totalTransform != null)
        {
            Vector3 initialRotation = totalTransform.rotation.eulerAngles;
            initialRotation.y *= -1f;
            totalTransform.rotation = Quaternion.Euler(initialRotation);
        }
    }
}
