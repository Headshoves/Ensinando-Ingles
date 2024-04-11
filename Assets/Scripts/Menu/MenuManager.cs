using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _initialScreen;

    [BoxGroup("Buttons")][SerializeField] private Button _campaignButton;
    [BoxGroup("Buttons")][SerializeField] private Button _exploreButton;

    [BoxGroup("Levels Settings")][SerializeField] private GameObject _levelsScreen;
    [BoxGroup("Levels Settings")][SerializeField] private GameObject _buttonLevelCampaignPrefab;

    [SerializeField] private float _timeToFade = 1f;

    private void Start() {
        _campaignButton.onClick.RemoveAllListeners();
        _campaignButton.onClick.AddListener(() => ShowLevels(_initialScreen, true));
        _exploreButton.onClick.RemoveAllListeners();
        _exploreButton.onClick.AddListener(() => ShowLevels(_initialScreen, false));
    }

    public void ShowLevels(GameObject prevousScreen, bool isCampaign) {
        prevousScreen.GetComponent<CanvasGroup>().DOFade(0, _timeToFade / 2).OnComplete(() => {
            prevousScreen.SetActive(false);
        
            _levelsScreen.SetActive(true);
            _levelsScreen.GetComponent<CanvasGroup>().DOFade(1, _timeToFade / 2).OnComplete(() => { ShowLevelButtons(isCampaign); });
        });
    }

    private List<GameObject> _levelButtons = new List<GameObject>();
    private async void ShowLevelButtons(bool isCampaign) {
        if (_levelButtons.Count > 0) { foreach (GameObject levelButton in _levelButtons) { DestroyImmediate(levelButton); } }
        _levelButtons.Clear();

        List<Level> levels = GameProgress.Instance.Game.Levels.ToList();

        for (int i = 0; i < levels.Count; i++) {
            int index = i;
            GameObject button = Instantiate(_buttonLevelCampaignPrefab, _levelsScreen.transform);
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = levels[index].LevelName;

            _levelButtons.Add(button);

            if (isCampaign) {
                button.transform.GetChild(1).gameObject.SetActive(true);
                button.transform.GetChild(2).gameObject.SetActive(true);
                button.transform.GetChild(3).gameObject.SetActive(true);
                if (levels[index].Score > 0) {
                    for (int j = 0; j < levels[index].Score; j++) {
                        button.transform.GetChild(j+1).GetComponent<Image>().color = Color.yellow;
                    }
                }

                if (index == 0 || index > 0 && levels[index - 1].IsDone) { button.transform.GetChild(4).gameObject.SetActive(false); }
                else { button.transform.GetChild(4).gameObject.SetActive(true); button.GetComponent<Button>().interactable = false; }
            }

            button.GetComponent<Button>().onClick.AddListener(() => { GoToLevel(levels[index].SceneReference); });
        }

        for(int i = 0; i < _levelButtons.Count; i++) {
            int index = i;
            _levelButtons[index].GetComponent<CanvasGroup>().DOFade(1, .2f);

            await Task.Delay(TimeSpan.FromSeconds(.2f));
        }
    }

    private void GoToLevel(string sceneReference) {

        SceneManager.LoadSceneAsync(sceneReference, LoadSceneMode.Additive);

        SceneManager.UnloadSceneAsync("00_Menu");
    }
}
