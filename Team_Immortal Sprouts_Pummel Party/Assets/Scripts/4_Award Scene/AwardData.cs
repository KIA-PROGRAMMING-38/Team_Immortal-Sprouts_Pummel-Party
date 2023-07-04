using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AwardType // 굳이 enum으로 해야할 이유가 있을까 싶긴함?
{
    MVP = 0,
    LOSER = 1,
    FIGHTER = 2,
    FINAL = 3
}

public class AwardData
{
    private List<int>[] playerLists;
    
    public AwardData(AwardProvider provider, int awardTypeCount)
    {
        playerLists = new List<int>[awardTypeCount];

        playerLists[(int)AwardType.MVP] = Statistics.GetMVPIndex();
        playerLists[(int)AwardType.LOSER] = Statistics.GetLoserIndex();
        playerLists[(int)AwardType.FIGHTER] = Statistics.GetFighterIndex();
        playerLists[(int)AwardType.FINAL] = Statistics.GetFinalWinner();

        provider.OnGetWinnerList -= getAwardPlayers;
        provider.OnGetWinnerList += getAwardPlayers;
    }

    private List<int> getAwardPlayers(AwardType type) => playerLists[(int)type];
}
