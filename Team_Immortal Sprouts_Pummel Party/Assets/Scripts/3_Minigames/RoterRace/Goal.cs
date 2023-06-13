using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private PlayerController player;
    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<PlayerController>();

        player.goalInEffect.Play();
        player.goalInObj.transform.SetParent(null);
        player.gameObject.SetActive(false);
    }
}
