using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePropellar : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }
}
