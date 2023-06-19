using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DucklingSpawner : MonoBehaviour
{
    [SerializeField] private Duckling duckling;
    private int spawnPositionCount = 18; // ���� ���� (��ġ 17 + ���� 1)
    private Transform[] spawnPositions; // 0��° �ε����� �Ⱦ�����
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
