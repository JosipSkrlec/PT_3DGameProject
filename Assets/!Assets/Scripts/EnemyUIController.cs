using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIController : MonoBehaviour
{
    public static EnemyUIController Instance;

    [SerializeField] private TMP_Text _enemyName;
    [SerializeField] private Image _enemyBarIndicator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SetupUIEnemy(string enemyNameToDisplayOnUI, float enemyMaxHealth, float enemyCurrentHealth)
    {
        _enemyName.text = enemyNameToDisplayOnUI;

        _enemyBarIndicator.fillAmount = enemyCurrentHealth / enemyMaxHealth;
    }
}
