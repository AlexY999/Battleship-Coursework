using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    [Header("Game panel")]
    [SerializeField] private GameObject gamePanel;
    [Header("WinLose panel")]
    [SerializeField] private GameObject winLosePanel;
    [SerializeField] private TextMeshProUGUI winLoseText;
    [Header("Scene changer")]
    [SerializeField] private SceneChanger sceneChanger;

    public void SetGamePanel()
    {
        OfAllPanels();
        gamePanel.SetActive(true);
    }
    
    public void SetWinLosePanel(bool isWin, int score)
    {
        OfAllPanels();
        winLosePanel.SetActive(true);
        if (isWin)
            winLoseText.text = "WIN !!!";
        else
            winLoseText.text = "LOSE !!!";
    }

    public void OnMenuButtonClick()
    {
        sceneChanger.gameObject.SetActive(true);
        sceneChanger.StartSceneChange(gameObject);
        Invoke(nameof(LoadMenuScene), 1.5f);
        AudioManager.Instance().OnButtonClickPlayAudioClip();
    }
    
    public void OnRestartButtonClick()
    {
        sceneChanger.gameObject.SetActive(true);
        sceneChanger.StartSceneChange(gameObject);
        Invoke(nameof(LoadGameScene), 1.5f);
        AudioManager.Instance().OnButtonClickPlayAudioClip();
    }

    private void LoadMenuScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MenuScene");
        asyncLoad.completed += OnMenuSceneLoadCompleted;
    }

    private void OnMenuSceneLoadCompleted(AsyncOperation obj)
    {
        SceneChanger changer = GameObject.Find("LoadingWindow").GetComponent<SceneChanger>();
        changer.StartSceneChange();
    }
    
    private void LoadGameScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");
        asyncLoad.completed += OnMenuSceneLoadCompleted;
    }

    private void OnGameSceneLoadCompleted(AsyncOperation obj)
    {
        SceneChanger changer = GameObject.Find("LoadingWindow").GetComponent<SceneChanger>();
        changer.StartSceneChange();
    }

    private void OfAllPanels()
    {
        gamePanel.SetActive(false);
        winLosePanel.SetActive(false);
    }
}
