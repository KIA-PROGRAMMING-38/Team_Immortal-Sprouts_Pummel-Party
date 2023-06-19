using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class DucklingCollector : MonoBehaviour
{
    // �����ġ�Ⱑ ������ ������ �ʿ���
    // �ٸ� �ݷ��Ͱ� ����� �뽬�� ���ư��� ���� �ʿ���
    // ���ư��� �� ���� ������ �ݴ���� + ���� ��¦ �����ϰ� �ؾ��ҵ�?
    // �׸��� �ڱⰡ �¾Ҵٴ� �� �˰� �������
    // ��Ȯ�ϰ� �ڱⰡ �¾Ҵٴ°� ��� �˰� ���ٱ�.. ����,....
    // �����ġ�⿡ �¾����� ���� ���������� �� ������ ������ �ʿ���
    // �츮�� ������� �츮 �ȿ� �������� �ִ� ������ �ʿ���(�� ���� �츮���߸� ��)
    // ���� �츮�� �ƴ϶� �ٸ� �츮�� �ִ°͵� ���� ��հڴµ�..?
    [SerializeField] private Material playerMaterial;
    [SerializeField] private Transform ducklingPosition;
    [SerializeField] List<Duckling> ducklings = new List<Duckling>();
    [SerializeField] private MeshRenderer[] renderers;
    private Vector3 moveDirection;
    private float inputX; // �¿� ���
    private float inputY;  // �յ� ���
    private Rigidbody rigidbody;
    private BoxCollider collider;
    [SerializeField][Range(50f, 150f)] private float moveSpeed = 100f;

    private int playerLayerNum;
    private int ducklingLayerNum;


    

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        playerMaterial = GetComponentInChildren<MeshRenderer>().material; // body�� �ִ� ��Ƽ���� �����ò���
        SetPlayerColor(playerMaterial);
        playerLayerNum = LayerMask.NameToLayer("DucklingCollector");
        ducklingLayerNum = LayerMask.NameToLayer("Duckling");
    }


    private void FixedUpdate()
    {
        //moveDirection.Normalize(); ����ȭ�� �ϰ� �Ǹ�, ���̽�ƽ�� ���� �����̴�, ���� �����̴� �ӵ��� ������!����ȭ ���ؾ���!!!
        playerMovement(inputY, inputX);
    }

    /// <summary>
    /// �÷��̾��� ������ �����ִ� �Լ�
    /// </summary>
    /// <param name="colorMaterial"></param>
    public void SetPlayerColor(Material colorMaterial)
    {
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material = colorMaterial;
        }
    }

    private bool isMoving = true;
    /// <summary>
    /// PlayerInput�� ��������, ���̽�ƽ�� �����϶� ȣ���� �Լ�
    /// </summary>
    /// <param name="context"></param>
    public void OnJoyStickControl(InputAction.CallbackContext context)
    {
        if (isMoving)
        {
            Vector3 inputDirection = context.ReadValue<Vector2>();
            inputX = inputDirection.x;
            inputY = inputDirection.y;
        }
    }
    private void playerMovement(float inputY, float inputX)
    {
        if (isMoving)
        {
            moveDirection = transform.forward * inputY + transform.right * inputX;
            rigidbody.velocity = moveDirection * Time.fixedDeltaTime * moveSpeed;
            transform.Rotate(Vector3.up, inputX * Time.deltaTime * moveSpeed);
        }
    }

    private bool isTackable = true;
    /// <summary>
    /// PlayerInput�� ��������, ��Ŭ��ư�� ������ ȣ���� �Լ�
    /// </summary>
    /// <param name="context"></param>
    public void OnTackleButtonPressed(InputAction.CallbackContext context)
    {
        if (isTackable && context.started)
        {
            playerTackle().Forget();
        }
    }


    [SerializeField] float holdTime = 1f;
    [SerializeField] private float tackleForce = 10f;
    private bool isTackling = false;
    private async UniTaskVoid playerTackle()
    {
        isMoving = false; // ���̽�ƽ �̵��� ���´�
        isTackling = true;
        StartTackleTimer().Forget();

        setEmission(true);
        Physics.IgnoreLayerCollision(playerLayerNum, ducklingLayerNum, true); // �뽬�� �������� ���԰� ����

        rigidbody.velocity = Vector3.zero;

        await UniTask.Delay(TimeSpan.FromSeconds(holdTime));

        rigidbody.AddForce(transform.forward * tackleForce, ForceMode.Impulse);

        await UniTask.Delay(TimeSpan.FromSeconds(holdTime));
        Physics.IgnoreLayerCollision(playerLayerNum, ducklingLayerNum, false);
        setEmission(false);
        isMoving = true; // �ٽ� �̵��� �����ϰԲ� �Ѵ�
        isTackling = false;
    }

    private void setEmission(bool isTurnOn)
    {
        if (isTurnOn)
        {
            Color glowColor = playerMaterial.color;
            playerMaterial.SetColor("_EmissionColor", glowColor); // ���λ��� ������
            playerMaterial.EnableKeyword("_EMISSION"); // �߻��Ѵ�
        }
        else
        {
            playerMaterial.DisableKeyword("_EMISSION");
        }
    }

    [SerializeField] private float tackleWaitTime = 10f;
    private async UniTaskVoid StartTackleTimer()
    {
        isTackable = false;
        await UniTask.Delay(TimeSpan.FromSeconds(tackleWaitTime));
        isTackable = true;
    }

    /// <summary>
    /// �÷��̾��� �ݶ��̴��� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public BoxCollider GetPlayerCollider() => collider;

    /// <summary>
    /// ���������� ������ �÷��̾��� ����� �ٲ��ִ� �Լ�
    /// </summary>
    /// <param name="ducklingRenderers"></param>
    public void ChangeDucklingColor(MeshRenderer[] ducklingRenderers)
    {
        foreach (MeshRenderer element in ducklingRenderers)
        {
            element.material = playerMaterial;
        }
    }

    /// <summary>
    /// ���������� �Ծ��Ű�� �Լ�
    /// </summary>
    /// <param name="duckling"></param>
    public void AdoptDuckling(Duckling duckling) => ducklings.Add(duckling);

    /// <summary>
    /// �Ծ��� ���������� ���� ��ġ�� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public Transform GetFollowPosition()
    {
        if (ducklings.Count == 0) // ���������� ���ٸ�
        {
            return ducklingPosition; // �⺻ ����� ��ġ�� �����Ѵ�
        }
        else // �Ծ��� ���������� �����Ѵٸ�
        {
            return ducklings[ducklings.Count - 1].GetFollowPosition(); // ������ ���������� ��ġ�� �����Ѵ�
        }
    }

    private void loseDucklings()
    {
        if (ducklings.Count != 0)
        {
            foreach (Duckling duckling in ducklings)
            {
                duckling.GetLost();
            }

            ducklings.Clear();
        }
    }

    private void putDucklingsIntoTheCage(DucklingCage ducklingCage)
    {
        if (ducklings.Count != 0) // ���������� �����Ѵٸ� 
        {
            ducklingsJumpInOrder(ducklingCage).Forget();
        }
    }

    private async UniTaskVoid ducklingsJumpInOrder(DucklingCage cage)
    {
        cage.OpenCage(true); // ������ �����ش�

        Transform destinationTransform = cage.GetDucklingDestination(ducklings.Count);

        foreach (Duckling duckling in ducklings)
        {
            duckling.PutDucklingIntoTheCage(destinationTransform);
            await UniTask.Delay(500); // �������������� �浹������ 0.5�� ������ �߰�
        }

        await UniTask.Delay(TimeSpan.FromSeconds(3f)); // �������� �����ð��� 2�ʶ� �˳��ϰ� 3�ʷ� �ش� 
        ducklings.Clear(); // �������� ���� ���忡 �־����Ƿ�, ������ ������������ �Ҵ´�
        cage.OpenCage(false); // ������ ����ش�
    }

    /// <summary>
    /// �浹���� �÷��̾ ���������� �Լ�
    /// </summary>
    /// <param name="collisionDirection"></param>
    /// <returns></returns>
    public async UniTask CauseTackleCollision(Vector3 collisionDirection)
    {
        isMoving = false;
        isTackable = false;
        rigidbody.AddForce(collisionDirection * tackleForce * 0.5f, ForceMode.Impulse); // �ε����� ���ư���
        setEmission(true);
        loseDucklings(); // ������������ �Ҵ´�
        await UniTask.Delay(1000);
        setEmission(false);
        isMoving = true;
        isTackable = true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (isTackling && collision.gameObject.CompareTag("DucklingCollector"))
        {
            Vector3 tackleDirection = transform.forward + Vector3.up; // �浹���� + ���� 1 up
            collision.gameObject.GetComponent<DucklingCollector>().CauseTackleCollision(tackleDirection).Forget();
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BirdCage"))
        {
            DucklingCage ducklingCage = other.gameObject.GetComponent<DucklingCage>();
            putDucklingsIntoTheCage(ducklingCage);
        }
    }
}
