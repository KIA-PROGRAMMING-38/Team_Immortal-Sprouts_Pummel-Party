using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody bulletBody;
    private Vector3 moveVector;
    private Vector3 randomVector;
    private Vector3 incomingVector;
    [SerializeField] private float bulletSpeed;

    private void Awake()
    {
        bulletBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        randomVector = UnityEngine.Random.insideUnitSphere;
        moveVector = new Vector3(randomVector.x, 0, randomVector.z);
    }

    private void Update()
    {
        bulletBody.velocity = moveVector * bulletSpeed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "HorizonSide":
                incomingVector = moveVector;
                moveVector = Vector3.Reflect(incomingVector, Vector3.back);
                break;
            case "VerticalSide":
                incomingVector = moveVector;
                moveVector = Vector3.Reflect(incomingVector, Vector3.left);
                break;
        }
    }
}
