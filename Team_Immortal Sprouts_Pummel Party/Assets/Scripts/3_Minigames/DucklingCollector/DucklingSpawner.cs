using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DucklingSpawner : MonoBehaviour
{
    [SerializeField] private Duckling duckling;
    private int spawnPositionCount = 18; // 본인 포함 (배치 17 + 본인 1)
    private Transform[] spawnPositions; // 0번째 인덱스는 안쓸꺼임
    void Start()
    {
        spawnPositions = new Transform[spawnPositionCount];
        spawnPositions = transform.GetComponentsInChildren<Transform>();
        spawnDucklings();
    }

    private void spawnDucklings()
    {
        for (int i = 1; i < spawnPositionCount ;++i)
        {
            Instantiate(duckling, spawnPositions[i].position, Quaternion.identity);
        }
        
    }

}
