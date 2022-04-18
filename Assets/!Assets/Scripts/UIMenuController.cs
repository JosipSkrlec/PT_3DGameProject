using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenuController : MonoBehaviour
{
    [SerializeField] private CanvasGroup _backgroundFadedImage;
    [SerializeField] private CanvasGroup _settingsPanel;


    private void Awake()
    {
        _backgroundFadedImage.alpha = 0.0f;
        _backgroundFadedImage.blocksRaycasts = false;

        _settingsPanel.alpha = 0.0f;
        _settingsPanel.blocksRaycasts = false;
    }

    public void StartTheGame()
    {
        SceneManager.LoadScene(1);

    }

    public void OpenSettings()
    {
        _backgroundFadedImage.DOFade(1.0f,0.25f).OnComplete(() => _backgroundFadedImage.blocksRaycasts = true);
        _settingsPanel.DOFade(1.0f,0.25f).OnComplete(() => _settingsPanel.blocksRaycasts = true);

    }

    public void CloseSettings()
    {
        _backgroundFadedImage.DOFade(0.0f, 0.25f).OnComplete(() => _backgroundFadedImage.blocksRaycasts = false);
        _settingsPanel.DOFade(0.0f, 0.25f).OnComplete(()=> _settingsPanel.blocksRaycasts = false);

    }


}
