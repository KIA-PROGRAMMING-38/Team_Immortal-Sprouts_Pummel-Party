using Cysharp.Threading.Tasks;
using UnityEngine;

public class DieBehaviour : StateMachineBehaviour
{
    [SerializeField] private Material dissolveMaterial;
    private MeshRenderer[] bodyMeshes;
    private Material originMaterial;

    private const string texture2DKey = "_Texture2D";
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject playerBody = animator.transform.GetChild(0).gameObject;
        bodyMeshes = playerBody.GetComponentsInChildren<MeshRenderer>();

        originMaterial = bodyMeshes[0].material;

        foreach(MeshRenderer meshRenderer in bodyMeshes)
        {
            Texture originTexture = meshRenderer.material.mainTexture;
            meshRenderer.material = dissolveMaterial;
            meshRenderer.material.SetTexture(texture2DKey, originTexture);
        }

        activateDissolve().Forget();
        test(animator).Forget();
    }

    [SerializeField] private float dissolveTime = 2.5f;
    private const float MIN_DISSOLVE_RATE = 0f;
    private const float MAX_DISSOLVE_RATE = 1f;
    private async UniTaskVoid activateDissolve()
    {
        float elapsedTime = 0f;
        float dissolveRate = 0f;
        while (elapsedTime <= dissolveTime)
        {
            dissolveRate = Mathf.Lerp(MIN_DISSOLVE_RATE, MAX_DISSOLVE_RATE, elapsedTime / dissolveTime);
            setDissolveRate(dissolveRate);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    private async UniTaskVoid test(Animator animator)
    {
        await UniTask.Delay(2500);
        animator.SetBool("Test", true);
    }

    private const string dissolveAmountKey = "_DissolveAmount";
    private void setDissolveRate(float dissolveRate)
    {
        foreach (MeshRenderer meshRenderer in bodyMeshes)
        {
            meshRenderer.material.SetFloat(dissolveAmountKey, dissolveRate);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(BoardgamePlayerAnimID.DIE, false);
        setDissolveRate(MIN_DISSOLVE_RATE);
        foreach (MeshRenderer meshRenderer in bodyMeshes)
        {
            meshRenderer.material = originMaterial;
        }
    }
}
