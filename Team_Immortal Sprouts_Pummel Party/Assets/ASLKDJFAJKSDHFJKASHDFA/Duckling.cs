using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duckling : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] renderers;
    [SerializeField] private Transform ducklingPosition;
    private BoxCollider collider;
    private Rigidbody rigid;
    private Material initialMaterial; // 기존 색상

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        collider= GetComponent<BoxCollider>();
        initialMaterial = renderers[0].material;
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (isFollowing == false && collision.gameObject.CompareTag("DucklingCollector"))
        {
            DucklingCollector playerCollector = collision.gameObject.GetComponent<DucklingCollector>();

            changeToCollectorColor(playerCollector); // 새끼오리의 색을 성체오리의 색으로 바꾼다
            ignoreCollisionWithPlayer(playerCollector); // 충돌처리를 무시하게 만든다
            isFollowing = true; // 누군가를 따라가고 있음을 표시해준다
            startFollowingCollector(playerCollector.GetFollowPosition()).Forget(); // 성체오리를 따라간다
            // 반드시 입양을 나중에 해줘야 함
            playerCollector.AdoptDuckling(this); // 자기자신을 성체오리에게 입양시킨다 => 참조를 연결하기 위하여 => 나중에 새끼오리가 나가떨어져야함
        }
    }

    private BoxCollider playerCollider;
    private void ignoreCollisionWithPlayer(DucklingCollector collector)
    {
        playerCollider = collector.GetPlayerCollider(); // 나중에 충돌처리를 다시 해주기위해 저장해줌
        Physics.IgnoreCollision(playerCollider, collider, true);
    }

    private void changeToCollectorColor(DucklingCollector collector) => collector.ChangeDucklingColor(renderers); // 새끼오리의 몸 색 변경
    

    [SerializeField] [Range(0.5f, 2f)] private float followTime = 1f;
    private Vector3 refVector = Vector3.zero;

    private bool isFollowing = false;
    private async UniTaskVoid startFollowingCollector(Transform followTransform) // 새끼오리가 성체오리를 따라다님
    {
        while (isFollowing)
        {
            Vector3 followPosition = followTransform.position;
            Vector3 lookDirection = followPosition - transform.position; // 가야할 방향
            transform.forward = lookDirection;
            transform.position = Vector3.SmoothDamp(transform.position, followPosition, ref refVector, followTime);
            
            await UniTask.Yield();
        }
    }

    /// <summary>
    /// 후발 오리가 따라갈 지점을 반환해주는 함수
    /// </summary>
    /// <returns></returns>
    public Transform GetFollowPosition() => ducklingPosition;


    
    [SerializeField] [Range(1f, 10f)] private float explosionForce = 7f;
    /// <summary>
    /// 성체 오리가 누군가에게 대쉬를 당해 새끼오리가 멀리 퍼지는 함수
    /// </summary>
    public void GetLost() 
    {
        resetStatus();

        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        Vector3 randomDirection = Vector3.zero;

        randomDirection.Set(randomX, 1f, randomZ); // 항상 위방향으로 날아갈꺼임
        rigid.velocity = randomDirection * explosionForce;
    }

    private void resetBodyColor()
    {
        foreach (MeshRenderer element in renderers)
        {
            element.material = initialMaterial;
        }
    }
    private void resetStatus()
    {
        isFollowing = false;
        resetBodyColor();
        Physics.IgnoreCollision(playerCollider, collider, false); // 다시 충돌처리가 가능하게끔 함
    }

    /// <summary>
    /// 새끼오리가 새장 안으로 뛰어드는 함수
    /// </summary>
    /// <param name="cageTransform"></param>
    public void PutDucklingIntoTheCage(Transform cageTransform) 
    {
        Vector3 cagePosition = cageTransform.position;
        isFollowing = false;
        jumpIntoBirdCage(cagePosition).Forget();
    }

    [SerializeField] private float jumpTime = 2f;
    private async UniTaskVoid jumpIntoBirdCage(Vector3 cagePosition) // 새끼오리가 새장안으로 포물선을 그리며 뛰어드는 함수
    {
        Vector3 currentPos = transform.position;
        Vector3 middlePosition = (cagePosition - currentPos) / 2f;
        middlePosition.Set(middlePosition.x, middlePosition.y + 3f, middlePosition.z); // 높게 뛰어오르게끔 +3f

        float elapsedTime = 0f;
        Vector3 m1;
        Vector3 m2;
        while (elapsedTime <= jumpTime)
        {
            float t = elapsedTime / jumpTime;
            m1 = Vector3.Lerp(currentPos, middlePosition, t);
            m2 = Vector3.Lerp(middlePosition, cagePosition , t);

            transform.position = Vector3.Lerp(m1, m2, t);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        await UniTask.Delay(500); // 새끼오리들의 충돌을 막기 위한 0.5초의 딜레이
    }
    
    
    

}
