using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{

    [SerializeField] private AudioSource _SFXAudioSource;
    [SerializeField] private AudioClip _click;

    public static BackgroundMusic Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }
    void Start()
    {
        if(FindObjectsOfType<BackgroundMusic>().Length > 1) {
            DestroyImmediate(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            _SFXAudioSource.Stop(); _SFXAudioSource.clip = _click; _SFXAudioSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip) {
        _SFXAudioSource.Stop(); _SFXAudioSource.clip = clip; _SFXAudioSource.Play();
    }
}
