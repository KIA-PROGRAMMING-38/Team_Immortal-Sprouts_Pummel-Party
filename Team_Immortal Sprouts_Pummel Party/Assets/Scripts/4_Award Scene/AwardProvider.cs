using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AwardProvider : MonoBehaviour
{
    [SerializeField] private Transform[] playerTransforms;
    [SerializeField] private AwardLightController[] lightControllers;

    public UnityEvent<Transform> OnGiveAward = new UnityEvent<Transform>();
    

    private void Awake()
    {

    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            giveAward();
        }
    }

    private void giveAward()
    {
        Transform winnerTransform = playerTransforms[1];
        OnGiveAward?.Invoke(winnerTransform);
    }


}
