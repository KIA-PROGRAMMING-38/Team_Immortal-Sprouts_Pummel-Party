using UnityEngine;

public class DieBehaviour : StateMachineBehaviour
{
    [SerializeField] private Material dissolveMaterial;
    private Material originMaterial;

    private GameObject playerBody;
    private MeshRenderer[] bodyMeshes;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerBody = animator.transform.GetChild(0).gameObject;
        bodyMeshes = playerBody.GetComponentsInChildren<MeshRenderer>();

        originMaterial = bodyMeshes[0].material;

        foreach(MeshRenderer meshRenderer in bodyMeshes)
        {
            Texture originTexture = meshRenderer.material.mainTexture;
            meshRenderer.material = dissolveMaterial;
            meshRenderer.material.SetTexture("_Texture2D", originTexture);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
