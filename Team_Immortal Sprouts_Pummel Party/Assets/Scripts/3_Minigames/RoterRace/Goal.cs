using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private RacePlayerController player;
    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<RacePlayerController>();

        player.goalInEffect.Play();
        player.goalInObj.transform.SetParent(null);
        player.gameObject.SetActive(false);
    }
}
