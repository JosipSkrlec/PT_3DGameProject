using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBarController : MonoBehaviour
{
    public static PlayerBarController Instance;


    [SerializeField] private Image _playerBarIndicator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SetupUIEnemy(float enemyMaxHealth, float enemyCurrentHealth)
    {
        _playerBarIndicator.fillAmount = enemyCurrentHealth / enemyMaxHealth;


    }
}
