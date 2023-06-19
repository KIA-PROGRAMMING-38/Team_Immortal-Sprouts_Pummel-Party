using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DucklingCage : MonoBehaviour
{
    [SerializeField] private Transform ducklingDestination;
    private Animator animator;

    [SerializeField] private int ducklingCount = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public Transform GetDucklingDestination(int ducklingNumber)
    {
        ducklingCount += ducklingNumber;
        return ducklingDestination; // 0번째 지 자신은 제외
    }

    public int GetDucklingCount() => ducklingCount;
    

    public void OpenCage(bool isOpened)
    {
        animator.SetBool("isOpen", isOpened);
    }

}
