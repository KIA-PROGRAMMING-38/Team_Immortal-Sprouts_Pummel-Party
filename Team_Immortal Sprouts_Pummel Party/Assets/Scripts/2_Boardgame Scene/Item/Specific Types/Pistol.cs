using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : ControllItem
{
    public override void OnTimeOut()
    {

    }

    private Transform _playerTransform;
    public override void SetForUse(BoardgamePlayer usePlayer)
    {
        base.SetForUse(usePlayer);
        
        _playerTransform = usePlayer.transform;
        gameObject.transform.SetParent(_playerTransform, false);
    }
}
