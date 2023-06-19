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
    private Material initialMaterial; // ���� ����

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

            changeToCollectorColor(playerCollector); // ���������� ���� ��ü������ ������ �ٲ۴�
            ignoreCollisionWithPlayer(playerCollector); // �浹ó���� �����ϰ� �����
            isFollowing = true; // �������� ���󰡰� ������ ǥ�����ش�
            startFollowingCollector(playerCollector.GetFollowPosition()).Forget(); // ��ü������ ���󰣴�
            // �ݵ�� �Ծ��� ���߿� ����� ��
            playerCollector.AdoptDuckling(this); // �ڱ��ڽ��� ��ü�������� �Ծ��Ų�� => ������ �����ϱ� ���Ͽ� => ���߿� ���������� ��������������
        }
    }

    private BoxCollider playerCollider;
    private void ignoreCollisionWithPlayer(DucklingCollector collector)
    {
        playerCollider = collector.GetPlayerCollider(); // ���߿� �浹ó���� �ٽ� ���ֱ����� ��������
        Physics.IgnoreCollision(playerCollider, collider, true);
    }

    private void changeToCollectorColor(DucklingCollector collector) => collector.ChangeDucklingColor(renderers); // ���������� �� �� ����
    

    [SerializeField] [Range(0.5f, 2f)] private float followTime = 1f;
    private Vector3 refVector = Vector3.zero;

    private bool isFollowing = false;
    private async UniTaskVoid startFollowingCollector(Transform followTransform) // ���������� ��ü������ ����ٴ�
    {
        while (isFollowing)
        {
            Vector3 followPosition = followTransform.position;
            Vector3 lookDirection = followPosition - transform.position; // ������ ����
            transform.forward = lookDirection;
            transform.position = Vector3.SmoothDamp(transform.position, followPosition, ref refVector, followTime);
            
            await UniTask.Yield();
        }
    }

    /// <summary>
    /// �Ĺ� ������ ���� ������ ��ȯ���ִ� �Լ�
    /// </summary>
    /// <returns></returns>
    public Transform GetFollowPosition() => ducklingPosition;


    
    [SerializeField] [Range(1f, 10f)] private float explosionForce = 7f;
    /// <summary>
    /// ��ü ������ ���������� �뽬�� ���� ���������� �ָ� ������ �Լ�
    /// </summary>
    public void GetLost() 
    {
        resetStatus();

        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        Vector3 randomDirection = Vector3.zero;

        randomDirection.Set(randomX, 1f, randomZ); // �׻� ���������� ���ư�����
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
        Physics.IgnoreCollision(playerCollider, collider, false); // �ٽ� �浹ó���� �����ϰԲ� ��
    }

    /// <summary>
    /// ���������� ���� ������ �پ��� �Լ�
    /// </summary>
    /// <param name="cageTransform"></param>
    public void PutDucklingIntoTheCage(Transform cageTransform) 
    {
        Vector3 cagePosition = cageTransform.position;
        isFollowing = false;
        jumpIntoBirdCage(cagePosition).Forget();
    }

    [SerializeField] private float jumpTime = 2f;
    private async UniTaskVoid jumpIntoBirdCage(Vector3 cagePosition) // ���������� ��������� �������� �׸��� �پ��� �Լ�
    {
        Vector3 currentPos = transform.position;
        Vector3 middlePosition = (cagePosition - currentPos) / 2f;
        middlePosition.Set(middlePosition.x, middlePosition.y + 3f, middlePosition.z); // ���� �پ�����Բ� +3f

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

        await UniTask.Delay(500); // ������������ �浹�� ���� ���� 0.5���� ������
    }
    
    
    

}
