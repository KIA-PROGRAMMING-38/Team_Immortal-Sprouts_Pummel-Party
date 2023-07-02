using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MinigameInformation : MonoBehaviour
{
    [SerializeField] UIPresenter presenter;
    private void OnEnable()
    {
        presenter.Info.Invoke();
    }
}
