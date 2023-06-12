using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PositionData : ScriptableObject
{
    public Transform[] _LobbyPositions;

    public Transform[] _BoardPositions;

    public Transform[] _InFightPositions;


}
