using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostArea : MonoBehaviour
{
    private float addedSpeed;
    private RacePlayerController player;

    private void Start()
    {
        addedSpeed = 15;
    }

    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<RacePlayerController>();

        player.boostEffect.Play();
        player.speed += addedSpeed;
    }
}
