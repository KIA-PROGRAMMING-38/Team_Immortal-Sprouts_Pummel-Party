using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : DamagedState
{
    public DieState(BoardPlayerController control, StateMachine machine, Animator anim, Rigidbody rigid, int animName) : base(control, machine, anim, rigid, animName)
    {
        playerController.OnSetPlayerMaterial.RemoveListener(getPlayerMaterials);
        playerController.OnSetPlayerMaterial.AddListener(getPlayerMaterials);
    }

    private Material[] playerMaterials;
    
    public override async void Enter()
    {
        base.Enter();
        await playerDissolve();
    }

    public override void Exit()
    {
        base.Exit();
        resetPlayerMaterial();
    }

    private void getPlayerMaterials(Material[] playerBodyMaterials) => playerMaterials = playerBodyMaterials;


    private const string dissolveAmountKey = "_DissolveAmount";
    [SerializeField] private float dissolveTime = 2.5f;
    private const float MIN_DISSOLVE_RATE = 0f;
    private const float MAX_DISSOLVE_RATE = 1f;
    private async UniTask playerDissolve()
    {
        float elapsedTime = 0f;
        float dissolveRate = 0f;
        while (elapsedTime <= dissolveTime)
        {
            dissolveRate = Mathf.Lerp(MIN_DISSOLVE_RATE, MAX_DISSOLVE_RATE, elapsedTime / dissolveTime);

            foreach (Material element in playerMaterials)
            {
                element.SetFloat(dissolveAmountKey, dissolveRate);
            }

            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    private void resetPlayerMaterial()
    {
        foreach (Material element in playerMaterials)
        {
            element.SetFloat(dissolveAmountKey, MAX_DISSOLVE_RATE);
        }
    }
}
