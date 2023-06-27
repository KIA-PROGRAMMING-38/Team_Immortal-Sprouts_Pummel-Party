using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EmojiPresenter : MonoBehaviour
{
    [SerializeField] private EmojiData emojiData;
    [SerializeField] private EmojiView emojiView;

    public Transform GetEmojiBubble(int emojiIndex) => emojiData.GetEmojiBubble(emojiIndex);
    public Transform GetPlayerPosition() => emojiData.GetPlayerEmojiTransform();

    public int GetEmojiCount() => emojiData.EmojiCount;
    public bool IsSlotDownalbe() => emojiData.IsSlotDown; // 혹시 모를 나중 연출을 위해서 미리 만들어 두었음
    public void SetSlotDown(bool isSlotDownable) => emojiData.IsSlotDown = isSlotDownable; // 혹시 모를 나중 연출을 위해서 미리 만들어 두었음
    public bool IsSlotTouchable() => emojiData.IsSlotTouchable;
    public void SetSlotTouchable(bool isSlotTouchable) => emojiData.IsSlotTouchable = isSlotTouchable;
    public bool IsEmojiTouchable() => emojiData.IsEmojiTouchable;
    public void SetEmojiTouchable(bool isEmojiTouchable) => emojiData.IsEmojiTouchable = isEmojiTouchable;
    public void EmojiSlotControl() => emojiData.AdjustSlotCanvas();

}