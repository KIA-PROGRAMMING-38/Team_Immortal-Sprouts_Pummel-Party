using UnityEngine;

public interface IUsable
{
    //void SetForUse(BoardgamePlayer usePlayer);

    void SetForUse(BoardPlayerController player = null);
    void Use();
}
