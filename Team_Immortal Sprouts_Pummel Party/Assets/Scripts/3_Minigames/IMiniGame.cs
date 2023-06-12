public interface IMiniGame
{
    /// <summary>
    /// 플레이어 스폰
    /// </summary>
    void SetPlayer();

    /// <summary>
    /// 미니게임 종료 시 랭킹 산정하고 등록
    /// </summary>
    void SetRank();

    /// <summary>
    /// 미니게임 제한시간 설정
    /// </summary>
    void SetTimer();
  

}
