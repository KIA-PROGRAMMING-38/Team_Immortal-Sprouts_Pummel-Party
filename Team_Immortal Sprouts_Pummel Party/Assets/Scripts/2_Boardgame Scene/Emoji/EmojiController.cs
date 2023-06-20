using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EmojiController : MonoBehaviour
{
    [SerializeField] private RectTransform[] emojiButtonTransform;
    [SerializeField][Range(50f, 80f)] private float gapDistance = 70f;
    [SerializeField][Range(0.1f, 1f)] private float downTime = 0.3f;

    // 이모티콘은 1~4 까지
    private Vector3 initialPosition;
    private Vector3 lastPosition;
    private const int startIndex = 1;
    private const int lastIndex = 4;
    [SerializeField] private bool isEmojiDown = false;
    private bool isTouchable = true;

    [SerializeField] Transform currentPlayerTransform;

    private const int LAUGH_EMOJI_NUM = 1;
    private const int ANGRY_EMOJI_NUM = 2;
    private const int CRY_EMOJI_NUM = 3;
    private const int TAUNT_EMOJI_NUM = 4;

    [SerializeField] Transform[] emojis;
    private Vector3 emojiSummonPosition;
    private bool isEmojable = true;

    // 포톤 적용시 해줘야할 것들
    // 1) currentPlayerTransform을 어딘가에서부터 동적으로 받아와서 set 해줘야한다
    // 2) 인덱스에 따른 이모지 prefabPool.Instantiate() 해주는 RPC함수를 구현한다
    // 3) prefabPool.Destory()로 풀 화 해주어야한다
    // moveUp과 moveDown쪽 코드 리팩토링이 필요하긴 한데, 현재 시간이 모자라서 그건 나중에 최적화기간때 하겠음


    private void Start()
    {
        initialPosition = transform.position;
        emojiSummonPosition = currentPlayerTransform.position + Vector3.up;
    }


    #region OnClick 이벤트 함수
    public void ListDownEmojiButtons()
    {
        if (isTouchable)
        {
            isTouchable = false;
            if (!isEmojiDown)
            {
                moveDown(initialPosition, startIndex, lastIndex).Forget();
            }
            else
            {
                moveUp(lastPosition, startIndex, lastIndex).Forget();
            }
        }
    }

    

    public async void OnClickLaughEmoji()
    {
        if (isEmojable)
        {
            Transform laughEmoji = Instantiate(emojis[LAUGH_EMOJI_NUM], emojiSummonPosition, Quaternion.identity);
            await manipulateSize(laughEmoji);
            Destroy(laughEmoji.gameObject); // 현재는 Destory로 되어있으나, 포톤 적용시 prefabPool 을 적용하여야 함
        }
    }

    public async void OnClickAngryEmoji()
    {
        if (isEmojable)
        {
            Transform angryEmoji = Instantiate(emojis[ANGRY_EMOJI_NUM], emojiSummonPosition, Quaternion.identity);
            await manipulateSize(angryEmoji);
            Destroy(angryEmoji.gameObject);
        }
    }

    public async void OnClickCryEmoji()
    {
        if (isEmojable)
        {
            Transform cryEmoji = Instantiate(emojis[CRY_EMOJI_NUM], emojiSummonPosition, Quaternion.identity);
            await manipulateSize(cryEmoji);
            Destroy(cryEmoji.gameObject);
        }
    }

    public async void OnClickTauntEmoji()
    {
        if (isEmojable)
        {
            Transform tauntEmoji = Instantiate(emojis[TAUNT_EMOJI_NUM], emojiSummonPosition, Quaternion.identity);
            await manipulateSize(tauntEmoji);
            Destroy(tauntEmoji.gameObject);
        }
    }

    #endregion

    [SerializeField] [Range(0.1f, 1f)] private float shrinkTime = 0.5f;
    [SerializeField] [Range(1f, 3f)] private float appearTime = 2f;
    private async UniTask manipulateSize(Transform emojiTrans)
    {
        isEmojable = false;
        float elapsedTime = 0f;

        Vector3 initialSize = Vector3.zero;
        Vector3 finalSize = Vector3.one;
        emojiTrans.localScale = initialSize;

        while (elapsedTime <= shrinkTime)
        {
            emojiTrans.localScale = Vector3.Lerp(initialSize, finalSize, elapsedTime / shrinkTime);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        elapsedTime = 0f;
        await UniTask.Delay(TimeSpan.FromSeconds(appearTime));

        while (elapsedTime <= shrinkTime)
        {
            emojiTrans.localScale = Vector3.Lerp(finalSize, initialSize, elapsedTime / shrinkTime);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        isEmojable = true;  
    }

    private async UniTaskVoid moveDown(Vector3 startPosition, int firstIndex, int lastIndex)
    {
        if (lastIndex < firstIndex)
        {
            isTouchable = true;
            isEmojiDown = true;
            lastPosition = emojiButtonTransform[firstIndex - 1].position;
            return;
        }

        emojiButtonTransform[firstIndex].gameObject.SetActive(true);

        Vector3 targetPosition = startPosition - (Vector3.up * gapDistance);

        await emojiMove(firstIndex, startPosition, targetPosition);

        moveDown(targetPosition, firstIndex + 1, lastIndex).Forget();
    }

    private async UniTaskVoid moveUp(Vector3 startPosition, int firstIndex, int lastIndex)
    {
        if (lastIndex < firstIndex)
        {
            isTouchable = true;
            isEmojiDown = false;
            return;
        }

        Vector3 targetPosition = startPosition + (Vector3.up * gapDistance);

        await emojiMove(lastIndex, startPosition, targetPosition);

        emojiButtonTransform[lastIndex].gameObject.SetActive(false);

        moveUp(targetPosition, firstIndex, lastIndex - 1).Forget();
    }

    private async UniTask emojiMove(int index, Vector3 startPosition, Vector3 targetPosition)
    {
        float elapsedTime = 0f;

        while (elapsedTime <= downTime)
        {
            emojiButtonTransform[index].position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / downTime);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }
}
