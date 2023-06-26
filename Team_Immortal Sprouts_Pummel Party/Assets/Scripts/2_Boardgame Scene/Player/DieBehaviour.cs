using UnityEngine;

public class DieBehaviour : StateMachineBehaviour
{
    [SerializeField] private Material dissolveMaterial;
    private Material[] originMaterials;

    private GameObject playerBody;
    private MeshRenderer[] bodyMeshes;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerBody = animator.transform.GetChild(0).gameObject;
        bodyMeshes = playerBody.GetComponentsInChildren<MeshRenderer>();

        originMaterials = new Material[bodyMeshes.Length];
        for(int i = 0; i < bodyMeshes.Length; ++i)
        {
            originMaterials[i] = bodyMeshes[i].material;
        }

        foreach(MeshRenderer meshRenderer in bodyMeshes)
        {
            Texture originTexture = meshRenderer.material.mainTexture;
            meshRenderer.material = dissolveMaterial;
            meshRenderer.material.SetTexture("_Texture2D", originTexture);
        }
    }

    private const float DISSOLVE_DURATION = 2.5f;
    private float elapsedTime;
    private float dissolveRate;
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dissolveRate = Mathf.Lerp(0f, 1f, elapsedTime / DISSOLVE_DURATION);
        foreach(MeshRenderer meshRenderer in bodyMeshes)
        {
            meshRenderer.material.SetFloat("_DissolveAmount", dissolveRate);
        }

        elapsedTime += Time.deltaTime;

        if(elapsedTime >= DISSOLVE_DURATION)
        {
            animator.SetTrigger(BoardgamePlayerAnimID.DISSOLVED);
        }
    }

    private int mateiralIndex = 0;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        elapsedTime = 0f;
        animator.SetBool(BoardgamePlayerAnimID.DIE, false);

        foreach (MeshRenderer meshRenderer in bodyMeshes)
        {
            meshRenderer.material = originMaterials[mateiralIndex++];
        }
    }
}
