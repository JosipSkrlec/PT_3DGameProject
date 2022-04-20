using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    public static PlayerUIController Instance;

    [SerializeField] private Image _playerBarIndicator;

    [SerializeField] private TMP_Text _playerLifeText;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public void SetupUIPlayer(float playerMaxHealth, float playerCurrentHealth)
    {
        _playerBarIndicator.fillAmount = playerCurrentHealth / playerMaxHealth;

    }

    public void UpdateLife(int life)
    {
        _playerLifeText.text = "x" + life.ToString();

    }


}
