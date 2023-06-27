using Cysharp.Threading.Tasks;
using UnityEngine;

public class ItemIsland : Island
{
    private ItemData givenItem;

    void Start()
    {
        InitPositionSettings().Forget();
    }

    /// <summary>
    /// 아이템섬을 활성화, 플레이어가 턴에서 최종 도착한 위치가 아이템섬일 경우 호출
    /// </summary>
    public async UniTask Activate(BoardgamePlayer player)
    {
        await UniTask.Delay(1500);

        ItemProvider.GiveRandomItemTo(player, out givenItem);

        await ShowItem(player);
    }

    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private Sprite randomBox;

    [SerializeField] private BoardgamePlayer me;  // TODO: 받은 사람, 구경하는 사람 다르게 보이도록하는 거 고려해서 추가하기
    private Vector3 offset = new Vector3(0, 1.5f, 0.5f);
    private Vector3 destLocalScale = new Vector3(0.3f, 0.3f, 0.3f);
    private const float GROWING_TIME = 0.5f;
    private async UniTask ShowItem(BoardgamePlayer player)
    {
        GameObject effect = Instantiate(effectPrefab);
        SpriteRenderer appearItem = effect.GetComponentInChildren<SpriteRenderer>();

        appearItem.sprite = givenItem.Icon;

        // TODO: 포톤 연결 후 아래와 같이 현재 턴인 플레이어의 PhotonView 확인해서 IsMine으로 확인
        //if (me == player)
        //{
        //    appearItem.sprite = givenItem.Icon;
        //}
        //else
        //{
        //    appearItem.sprite = randomBox;
        //}

        effect.transform.position = player.transform.position + offset;

        float elapsedTime = 0f;
        while(elapsedTime <= GROWING_TIME)
        {
            appearItem.gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, destLocalScale, elapsedTime / GROWING_TIME);
            elapsedTime += Time.deltaTime;

            await UniTask.Yield();
        }

        await UniTask.Delay(1500);
        Destroy(effect);
    }
}
