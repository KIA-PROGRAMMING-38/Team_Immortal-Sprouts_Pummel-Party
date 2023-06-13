using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostArea : MonoBehaviour
{
    private float addedSpeed;
    private PlayerController playerBody;

    private void Start()
    {
        addedSpeed = 15;
    }

    private void OnTriggerEnter(Collider other)
    {
        playerBody = other.GetComponent<PlayerController>();

        playerBody.boostEffect.Play();
        playerBody.speed += addedSpeed;
    }
}
