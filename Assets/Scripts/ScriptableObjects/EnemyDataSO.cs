using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects / EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    public List<EnemyStats> DataList;

    public EnemyStats GetEnemyStats(EnemyTypeEnum enemyType)
    { 
        EnemyStats stats = DataList[0];

        foreach (var item in DataList)
        {
            if (enemyType == item.EnemyType)
                stats = item;
        }

        return stats;
    }

}
