using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TackleButton : MonoBehaviour
{
    [SerializeField] private Image tackleButton;
    [SerializeField] private float coolTime = 10f;
    private bool isTackable = true;

    /// <summary>
    /// PlayerInput에 구독한, 태클버큰이 눌렸을때 호출될 함수
    /// </summary>
    /// <param name="context"></param>
    public void OnTackleButtonPressed(InputAction.CallbackContext context)
    {
        if (isTackable && context.started)
        {
            startTimer().Forget();
        }
    }

    private async UniTaskVoid startTimer()
    {
        isTackable = false;
        float min = 0f;
        float max = 1f;
        float elapsedTime = 0f;

        while (elapsedTime <= coolTime)
        {
            float value = Mathf.Lerp(min, max, elapsedTime / coolTime);
            tackleButton.fillAmount = value;
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        isTackable = true;
    }

}
