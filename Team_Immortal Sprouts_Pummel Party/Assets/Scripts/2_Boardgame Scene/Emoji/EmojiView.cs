using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EmojiView : MonoBehaviour
{
    [SerializeField] private EmojiPresenter presenter;
    [SerializeField] [Range(0.1f, 1f)] private float shrinkTime = 0.3f;
    [SerializeField] [Range(1f, 3f)] private float showTime = 2f;
    private const int LAUGH_EMOJI_INDEX = 0;
    private const int ANGER_EMOJI_INDEX = 1;
    private const int CRY_EMOJI_INDEX = 2;
    private const int TAUNT_EMOJI_INDEX = 3;


    #region OnClick Event 함수
    public void On_Click_Slot_Button()
    {
        if (presenter.IsSlotTouchable()) // 나중에 연출이 들어가면 이거 조정해야함
            presenter.EmojiSlotControl();
    }

    public void On_Click_Laugh_Button()
    {
        if (presenter.IsEmojiTouchable())
            ShowEmojiPopUp(LAUGH_EMOJI_INDEX).Forget();
    }

    public void On_Click_Anger_Button()
    {
        if (presenter.IsEmojiTouchable())
            ShowEmojiPopUp(ANGER_EMOJI_INDEX).Forget();
    }

    public void On_Click_Cry_Button()
    {
        if (presenter.IsEmojiTouchable())
            ShowEmojiPopUp(CRY_EMOJI_INDEX).Forget();
    }

    public void On_Click_Taunt_Button()
    {
        if (presenter.IsEmojiTouchable())
            ShowEmojiPopUp(TAUNT_EMOJI_INDEX).Forget();
    }

    #endregion

    private async UniTaskVoid ShowEmojiPopUp(int emojiIndex)
    {
        presenter.SetSlotTouchable(false); // 슬롯을 닫을 수 없게끔 만들어준다
        presenter.SetEmojiTouchable(false); // 이모티콘을 중복으로 표시할 수 없게끔 만들어준다
        Transform bubble = Instantiate(presenter.GetEmojiBubble(emojiIndex)); // 말풍선을 생성한다
        Transform playerTransform = presenter.GetPlayerPosition(); // 플레이어의 Transform을 가져온다
        await emojiPopStay(playerTransform, bubble); // 말풍선이 플레이어 머리위에 존재하도록 만들어준다
        presenter.SetSlotTouchable(true); // 다시 슬롯을 닫을 수 있게끔 만들어준다
        presenter.SetEmojiTouchable(true); // 이모티콘 사용이 다시 가능하게끔 만들어준다
    }

    private async UniTask emojiPopStay(Transform playerTransform, Transform bubble) // 말풍선이 플레이어 머리위에 띄워지는 함수
    {
        Vector3 initialSize = Vector3.zero;
        Vector3 endSize = Vector3.one;

        float entireTime = showTime + (shrinkTime * 2);

        bubbleFollowPlayer(entireTime, playerTransform, bubble).Forget();

        await controlSize(bubble, initialSize, endSize);

        await UniTask.Delay(TimeSpan.FromSeconds(showTime));

        await controlSize(bubble, endSize, initialSize);

        Destroy(bubble.gameObject); // 지금은 Destroy를 하고 있지만, 나중에 포톤적용하면 prefabPool.Destroy()로 바꿔주어야 함
    }

    private async UniTaskVoid bubbleFollowPlayer(float followTime, Transform playerTransform, Transform bubble) // 말풍선이 플레이어를 따라가는 함수
    {
        float elapsedTime = 0f;
        while (elapsedTime <= followTime)
        {
            bubble.position = playerTransform.position + Vector3.up; // 머리 위에 표시해야 함으로 +1
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    private async UniTask controlSize(Transform bubble, Vector3 startSize, Vector3 endSize) // 말풍선 크기 조절해주는 함수
    {
        float elapsedTime = 0f;
        while (elapsedTime <= shrinkTime)
        {
            bubble.localScale = Vector3.Lerp(startSize, endSize, elapsedTime / shrinkTime);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }

}
