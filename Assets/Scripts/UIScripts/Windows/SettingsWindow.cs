using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour
{
    [SerializeField] private Toggle SFXToggle;
    [SerializeField] private Toggle MusicToggle;

    private void Start()
    {
        SFXToggle.isOn = AudioManager.Instance().GetSfxSoudsPointer();
        MusicToggle.isOn = AudioManager.Instance().GetMusicSoudsPointer();
    }

    public void OnSFXButtonClick(bool pointer)
    {
        UIManager.Instance().OnButtonClisk();
        AudioManager.Instance().ChangeSfxSoudsPointer(pointer);
    }
    
    public void OnMusicButtonClick(bool pointer)
    {
        UIManager.Instance().OnButtonClisk();
        AudioManager.Instance().ChangeMusicSoudsPointer(pointer);
    }
    
    public void OnBackButtonClick()
    {
        UIManager.Instance().PreviousPage();
        UIManager.Instance().OnButtonClisk();
    }
}
