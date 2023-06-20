using Cysharp.Threading.Tasks;
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

    private void Start()
    {
        initialPosition = transform.position;
    }

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
