using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI loadingText;

    public void StartSceneChange(GameObject window)
    {
        StartCoroutine(FadeInUIElements(window));
    }

    public void StartSceneChange()
    {
        StartCoroutine(nameof(FadeOutUIElements));
    }

    IEnumerator FadeInUIElements(GameObject window)
    {
        List<Image> imagesList = new List<Image>();
        List<TextMeshProUGUI> textsList = new List<TextMeshProUGUI>();

        if (window.gameObject.activeSelf)
        {
            imagesList.AddRange(window.GetComponentsInChildren<Image>().ToList());
            textsList.AddRange(window.GetComponentsInChildren<TextMeshProUGUI>().ToList());
        }

        float tick = 0f;
        while (background.color != Color.black)
        {
            tick += Time.deltaTime;
            foreach (var image in imagesList)
            {
                image.color = Color.Lerp(image.color, Color.clear, tick);
            }

            foreach (var text in textsList)
            {
                text.color = Color.Lerp(text.color, Color.clear, tick);
            }

            background.color = Color.Lerp(background.color, Color.black, tick);
            loadingText.color = Color.Lerp(loadingText.color,  new Color(0.1960784f, 0.1960784f, 0.1960784f, 1), tick);
            yield return null;
        }
    }
    
    IEnumerator FadeOutUIElements()
    {
        float tick = 0f;
        background.color = Color.black;
        loadingText.color = new Color(0.1960784f, 0.1960784f, 0.1960784f, 1);
        while (background.color != Color.clear)
        {
            tick += Time.deltaTime;
            background.color = Color.Lerp(Color.black, Color.clear, tick);
            loadingText.color = Color.Lerp(loadingText.color, Color.clear , tick);
            yield return null;
        }
    }
}
