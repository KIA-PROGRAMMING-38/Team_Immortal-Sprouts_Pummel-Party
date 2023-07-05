using UnityEngine;

public interface IUsable
{
    void Use(BoardPlayerController player = null);
    
    void OnTimeOut();
}
