using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AwardType
{
    MVP,
    LOSER,
    FIGHTER
}

public class AwardData
{
    public int ID { get; set; }
    public AwardType Type { get; set; }
    public int EggPlus { get; set; }
}