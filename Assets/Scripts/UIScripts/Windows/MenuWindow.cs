using UnityEngine;

public class MenuWindow : MonoBehaviour
{
    public void OnSettinsButtonClick()
    {
        UIManager.Instance().NextPage();
        UIManager.Instance().OnButtonClisk();
    }

    public void OnPlayButtonClick()
    {
        UIManager.Instance().OnButtonClisk();
        UIManager.Instance().GoToGameScene();
    }
}
