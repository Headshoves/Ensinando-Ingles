using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _creditsButton;
    [SerializeField] private Button _returnCreditsButton;
    [SerializeField] private Button _returnButton;

    [Header("Screens")]
    [SerializeField] private GameObject _menuScreen;
    [SerializeField] private GameObject _creditScreen;

    [SerializeField] private float _timeToTransition = 0.3f;

    private AsyncLoader _loader;

    private void Awake() {
        _loader = GetComponent<AsyncLoader>();

        _menuScreen.GetComponent<CanvasGroup>().alpha = 1.0f;
        _menuScreen.SetActive(true);
        _creditScreen.GetComponent<CanvasGroup>().alpha = 0.0f;
        _creditScreen.SetActive(false);
    }

    private void Start() {
        _playButton.onClick.RemoveAllListeners();
        _playButton.onClick.AddListener(() => { _loader.LoadLevelBtn("1-Game"); });
        _returnButton.onClick.RemoveAllListeners();
        _returnButton.onClick.AddListener(() => { _loader.LoadLevelBtn("0-GeneralMenu"); });
        _creditsButton.onClick.RemoveAllListeners();
        _creditsButton.onClick.AddListener(() => { ShowCredits(true); });
        _returnCreditsButton.onClick.RemoveAllListeners();
        _returnCreditsButton.onClick.AddListener(() => { ShowCredits(false); });
    }

    private void ShowCredits(bool show) {
        
        if(show) {
            _creditScreen.SetActive(true);
            _creditScreen.GetComponent<CanvasGroup>().DOFade(1, _timeToTransition);
        }
        else {
            _creditScreen.GetComponent<CanvasGroup>().DOFade(0, _timeToTransition).OnComplete(() => {
                _creditScreen.SetActive(false);
            });
        }
    }
}
