using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiController : MonoBehaviour
{
    [SerializeField] private RectTransform[] emojiButtonTransform;
    [SerializeField] [Range(50f, 80f)] private float gapDistance = 70f;
    [SerializeField] [Range(0.1f, 1f)] private float downTime = 0.3f;

    // 이모티콘은 1~4 까지
    private Vector3 initialPosition;
    private Vector3 lastPosition;
    private const int startIndex = 1;
    private const int lastIndex = 4;
    private bool isEmojiDown = false;

    private void Start()
    {
        initialPosition = transform.position;
    }

    public void ListDownEmojiButtons()
    {
        moveDown(initialPosition, startIndex, lastIndex, isEmojiDown).Forget();
    }

    private async UniTaskVoid moveDown(Vector3 startPosition, int firstIndex, int lastIndex, bool isDown)
    {
        if (!isDown)
        {
            if (lastIndex < firstIndex)
            {
                lastPosition = startPosition;
                isEmojiDown = true;
                return;
            }

            emojiButtonTransform[firstIndex].gameObject.SetActive(true);
            
            Vector3 targetPosition = startPosition - (Vector3.up * gapDistance);

            float elapsedTime = 0f;

            while (elapsedTime <= downTime)
            {
                emojiButtonTransform[firstIndex].position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / downTime);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }

            Vector3 newStartPosition = targetPosition - (Vector3.up * gapDistance);

            moveDown(newStartPosition, firstIndex + 1, lastIndex, isDown).Forget();
        }
        else // 이미 내려와 있는 상태라면
        {
            if (lastIndex < firstIndex)
            {
                isEmojiDown = false;
                return;
            }

            float elapsedTime = 0f;

            Vector3 newStartPosition = lastPosition;
            Vector3 targetPosition = newStartPosition + (Vector3.up * gapDistance);

            while (elapsedTime <= downTime)
            {
                emojiButtonTransform[lastIndex].position = Vector3.Lerp(newStartPosition, targetPosition, elapsedTime / downTime);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }

            newStartPosition = targetPosition;

            //moveDown(firstIndex, lastIndex - 1, isDown).Forget();

        }
        

    }

}
