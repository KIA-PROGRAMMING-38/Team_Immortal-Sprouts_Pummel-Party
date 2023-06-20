using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiData : MonoBehaviour
{
    [SerializeField] private EmojiPresenter presenter;
    [SerializeField] private Transform[] bubblePrefabs;
    [SerializeField] private Transform currentPlayerTransform;
    [SerializeField] private Canvas emojiSlotCanvas;
    public bool IsSlotDown { get; set; } = false; // 나중 연출을 위해 미리 만들어 둚
    public bool IsSlotTouchable { get; set; } = true;
    public bool IsEmojiTouchable { get; set; } = true;
    public int EmojiCount { get; private set; } = 4;

    public Transform GetPlayerEmojiTransform() => currentPlayerTransform;
    public Transform GetEmojiBubble(int bubbleIndex) => bubblePrefabs[bubbleIndex];
    public void AdjustSlotCanvas() => emojiSlotCanvas.enabled = !emojiSlotCanvas.enabled;
}
