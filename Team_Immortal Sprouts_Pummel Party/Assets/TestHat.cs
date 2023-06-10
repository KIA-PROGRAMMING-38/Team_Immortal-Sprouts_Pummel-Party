using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHat : MonoBehaviour
{
    public GameObject[] hats;
    public Transform hatPosition;

    void Start()
    {

    }

    public int index = 0;
    public GameObject currentHat;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentHat != null)
            {
                currentHat.SetActive(false);
            }

            if (index == hats.Length)
            {
                index = 0;
            }

            
            currentHat = hats[index];

            if (currentHat.activeSelf == false)
            {
                currentHat.SetActive(true);
            }

            currentHat.transform.position = hatPosition.position;
            ++index;
        }
    }
}
