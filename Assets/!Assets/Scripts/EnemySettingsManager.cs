using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySettingsManager : MonoBehaviour
{
    public static EnemySettingsManager Instance;

    [SerializeField] private EnemySettings _enemyWeak;
    [SerializeField] private EnemySettings _enemyNormal;
    [SerializeField] private EnemySettings _enemyStrong;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public EnemySettings GetEnemySettings(EnemyDifficulty enemyDiff)
    {
        if (enemyDiff == EnemyDifficulty.Normal)
        {
            return _enemyNormal;
        }
        else if (enemyDiff == EnemyDifficulty.Strong)
        {
            return _enemyStrong;
        }
        else
        {
            return _enemyWeak;
        }
    }

}

[Serializable]
public enum EnemyDifficulty
{
    Weak,
    Normal,
    Strong/*,*/
    //Quick,
    //Boss
};

[Serializable]
public class EnemySettings
{
    [Tooltip("Health default is 100")]
    public float _health;
    [Tooltip("default speed is 0.25f")]
    public float _enemySpeed;
    [Tooltip("default damage is around 20.0f")]
    public float _enemyDamage;
    // TODO - dodati jos za izjednjacavanje pozicije varijablu !!!
}