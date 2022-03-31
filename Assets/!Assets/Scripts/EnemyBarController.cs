using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBarController : MonoBehaviour
{
    public static EnemyBarController Instance;


    [SerializeField] private TMP_Text _enemyName;
    [SerializeField] private Image _enemyBarIndicator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SetupUIEnemy(string enemyName, float enemyMaxHealth, float enemyCurrentHealth)
    {
        _enemyName.text = enemyName;

        _enemyBarIndicator.fillAmount = enemyCurrentHealth / enemyMaxHealth;


    }
}
