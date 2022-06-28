using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource musicAudioSource;
    [Space]
    [SerializeField] private AudioClip buttonClickAudioClip;
    [SerializeField] private List<AudioClip> musicAudioClip;

    private bool _sfxPointer = true;
    private bool _musicPointer = true;
    private int _currentMusicAudioClip = 0;
    private string _currentSceneName;
    
    private static AudioManager _instance;

    public static AudioManager Instance()
    {
        return _instance;
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
        else if (_instance == AudioManager._instance)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void ChangeSfxSoudsPointer(bool pointer)
    {
        if(pointer)
            _sfxPointer = true;
        else
            _sfxPointer = false;

        TurnMusicSFX();
    }

    public void ChangeMusicSoudsPointer(bool pointer)
    {
        if(pointer)
            _musicPointer = true;
        else
            _musicPointer = false;
        
        TurnMusicSFX();
    }

    public bool GetSfxSoudsPointer()
    {
        return _sfxPointer;
    }

    public bool GetMusicSoudsPointer()
    {
        return _musicPointer;
    }

    public void OnButtonClickPlayAudioClip()
    {
        sfxAudioSource.PlayOneShot(buttonClickAudioClip);
    }

    private void PlayMusic()
    {
        musicAudioSource.clip = musicAudioClip[_currentMusicAudioClip];
        musicAudioSource.Play();
        
        if (_currentMusicAudioClip + 1 != musicAudioClip.Count)
            _currentMusicAudioClip++;
        else
            _currentMusicAudioClip = 0;
    }

    private void TurnMusicSFX()
    {
        sfxAudioSource.volume = _sfxPointer ? 1 : 0;
        musicAudioSource.volume = _musicPointer ? 1 : 0;
    }
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (string.IsNullOrEmpty(_currentSceneName) || _currentSceneName != scene.name)
        {
            _currentSceneName = scene.name;
            PlayMusic();
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
