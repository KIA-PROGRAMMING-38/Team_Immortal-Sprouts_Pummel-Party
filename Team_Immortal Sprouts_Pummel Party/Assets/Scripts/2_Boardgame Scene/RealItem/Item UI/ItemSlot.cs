using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private int itemID;
    [SerializeField] private InventoryUIController inventoryController;

    private Image image;
    private Button button;

    private CancellationTokenSource playSource;
    private CancellationTokenSource cancelSource;
    private CancellationToken token;

    private const int nullID = -9999;

    public UnityEvent<int> OnTouchItemSlot = new UnityEvent<int>();

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        playSource = new CancellationTokenSource();
        cancelSource = new CancellationTokenSource();
        cancelSource.Cancel();
        token = playSource.Token;

        inventoryController = GetComponentInParent<InventoryUIController>();
    }

    private void OnEnable()
    {
        inventoryController.OnEnableItemSlot.RemoveListener(EnableButton);
        inventoryController.OnEnableItemSlot.AddListener(EnableButton);
    }

    private void Start()
    {
        
    }

    private void OnDisable()
    {
        inventoryController.OnEnableItemSlot.RemoveListener(EnableButton);
    }


    #region OnClick Event 함수

    [SerializeField] private bool isSelected = false;
    public void SelectItem()
    {
        isSelected = !isSelected;
        if (isSelected)
        {
            token = playSource.Token;
            OnTouchItemSlot?.Invoke(itemID);
            showSelected().Forget();
        }
        else
        {
            token = cancelSource.Token;
            OnTouchItemSlot?.Invoke(nullID);
            image.fillAmount = 1f;
        }
    }

    #endregion

    public void CancleSelectItem()
    {
        isSelected = false;
        token = cancelSource.Token;
        image.fillAmount = 1f;
    }

    [SerializeField] [Range(0.5f, 1.5f)] private float fillTime = 1f;
    private async UniTaskVoid showSelected()
    {
        while (true)
        {
            await fillImageAmount(1, 0, fillTime);

            await fillImageAmount(0, 1, fillTime);

            await UniTask.Yield(token);
        }
    }

    private async UniTask fillImageAmount(float start, float end, float duration)
    {
        float elapsedTime = 0f;
        float fillValue;
        while (elapsedTime <= duration)
        {
            fillValue = ExtensionMethod.Lerp(start, end, elapsedTime / duration);
            image.fillAmount = fillValue;
            elapsedTime += Time.deltaTime;
            await UniTask.Yield(token);
        }
        image.fillClockwise = !image.fillClockwise;
    }

    private void EnableButton(int[] itemCounts)
    {
        if (1 <= itemCounts[itemID])
            button.interactable = true;
        else
            button.interactable = false;
    }
}
