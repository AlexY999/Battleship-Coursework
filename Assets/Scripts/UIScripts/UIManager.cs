using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<RectTransform> pages;
    [SerializeField] private SceneChanger sceneChanger;
    [SerializeField] private int currentPage = 0;
    [SerializeField] private float pageTransitionDuration = 1f;
    [SerializeField] private AnimationCurve transitionCurve;

    private Vector3 _position;
    private Coroutine _transitionCoroutine;

    private static UIManager _instance;

    public static UIManager Instance()
    {
        return _instance;
    }

    public void OnButtonClisk()
    {
        AudioManager.Instance().OnButtonClickPlayAudioClip();
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            Transition(currentPage + 1, currentPage, false);
        }
    }

    public void NextPage()
    {
        if (currentPage < pages.Count - 1)
        {
            currentPage++;
            Transition(currentPage - 1, currentPage, true);
        }
    }

    public void GoToGameScene()
    {
        sceneChanger.gameObject.SetActive(true);
        sceneChanger.StartSceneChange(pages[0].gameObject);
        Invoke(nameof(LoadGameScene), 1.5f);
        AudioManager.Instance().OnButtonClickPlayAudioClip();
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance == this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Initialization();
    }
    
    private void Initialization()
    {
        OffAllPages();
        pages[0].gameObject.SetActive(true);
    }

    private void LoadGameScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");
        asyncLoad.completed += OnMenuSceneLoadCompleted;
    }

    private void OnMenuSceneLoadCompleted(AsyncOperation obj)
    {
        SceneChanger changer = GameObject.Find("LoadingWindow").GetComponent<SceneChanger>();
        changer.StartSceneChange();
    }

    private void Transition(int previous, int next, bool goingRight)
    {
        if (_transitionCoroutine != null)
        {
            StopCoroutine(_transitionCoroutine);
        }

        _transitionCoroutine = StartCoroutine(TransitionCoroutine(previous, next, goingRight));
    }

    private IEnumerator TransitionCoroutine(int previous, int next, bool goingRight)
    {
        float screenWidth = gameObject.GetComponent<RectTransform>().rect.width;

        _position.y = pages[previous].localPosition.y;
        _position.z = pages[previous].localPosition.z;

        pages[next].gameObject.SetActive(true);
        UnBlockAllButtons(false);
        
        float timeSpent = 0f;
        while (timeSpent < pageTransitionDuration)
        {
            if (goingRight)
            {
                _position.x = Mathf.Lerp(0f, -screenWidth, transitionCurve.Evaluate(timeSpent/pageTransitionDuration));
                pages[previous].localPosition = _position;
                _position.x = Mathf.Lerp(screenWidth, 0f, transitionCurve.Evaluate(timeSpent/pageTransitionDuration));
                pages[next].localPosition = _position;
            }
            else
            {
                _position.x = Mathf.Lerp(0f, screenWidth, transitionCurve.Evaluate(timeSpent/pageTransitionDuration));
                pages[previous].localPosition = _position;
                _position.x = Mathf.Lerp(-screenWidth, 0f, transitionCurve.Evaluate(timeSpent/pageTransitionDuration));
                pages[next].localPosition = _position;
            }

            timeSpent += Time.deltaTime;
            yield return null;
        }

        UnBlockAllButtons(true);
        pages[previous].gameObject.SetActive(false);
    }

    private void UnBlockAllButtons(bool pointer)
    {
        List<Button> buttonsList = new List<Button>();

        foreach (var page in pages)
        {
            if (page.gameObject.activeSelf)
            {
                Button[] buttons = page.GetComponentsInChildren<Button>();
                buttonsList.AddRange(buttons);
            }
        }

        foreach (var button in buttonsList)
        {
            button.enabled = pointer;
        }
    }

    private void OffAllPages()
    {
        foreach (var page in pages)
        {
            page.gameObject.SetActive(false);
        }
    }
}