using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoader : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] private GameObject _loadScreen;
    [SerializeField] private GameObject _mainMenu;

    [Header("Slider")]
    [SerializeField] private Slider _loadingSlider;

    public void LoadLevelBtn(string levelToLoad) {
        _loadScreen.SetActive(true);
        if (_mainMenu != null) {
            _mainMenu.GetComponent<CanvasGroup>().DOFade(0, .1f).OnComplete(() => { StartCoroutine(LoadLevelAsync(levelToLoad)); });
        }
        else {
            StartCoroutine(LoadLevelAsync(levelToLoad));
        }
    }

    private IEnumerator LoadLevelAsync(string levelToLoad) {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while(!loadOperation.isDone) {
            float progressValue = Mathf.Clamp01(loadOperation.progress / .9f);
            _loadingSlider.value = progressValue;
            yield return null;
        }
    }
}
