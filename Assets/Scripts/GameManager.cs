using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float _transitionTime = .2f;
    [SerializeField] private bool _useKeyboard = false;
    [SerializeField] private ScoreManager _scoreManager;
    private int _attempts = 0;

    [Header("Audios")]
    [SerializeField] private AudioClip _correctSFX;
    [SerializeField] private AudioClip _incorrectSFX;

    [Header("Intro")]
    [SerializeField] private GameObject _choiceGameMethodScreen;
    [SerializeField] private GameObject _choiceGameDifficultScreen;

    [Header("Game")]
    [SerializeField] private GameObject _gameScreen;
    [SerializeField] private TextMeshProUGUI _textLetter;
    [SerializeField] private Button _audioButton;
    [SerializeField] private List<GameObject> _answersButtons = new List<GameObject>();

    [Header("Help")]
    [SerializeField] private RectTransform _helpScreen;
    [SerializeField] private TextMeshProUGUI _helpText;
    [SerializeField] private GameObject _helpButton;
    [SerializeField][TextArea(1,10)] private List<string> _helpTexts = new List<string>();

    private int _helpIndex = 0;

    [Header("End")]
    [SerializeField] private GameObject _endGameScreen;
    [SerializeField] private TextMeshProUGUI _endText;

    [Header("Restart")]
    [SerializeField] private Button _restartButton;

    [SerializeField] private List<AlphabetLetter> _alphabet = new List<AlphabetLetter>();

    private int _indexLetter = 0;

    private bool _randomGame;
    private List<int> _randomOrder = new List<int>();

    private AudioSource _as;
    private AsyncLoader _loader;

    private void Start() {
        _as = GetComponent<AudioSource>();
        _loader = GetComponent<AsyncLoader>();

        _audioButton.onClick.RemoveAllListeners();
        _audioButton.onClick.AddListener(() => { if (_randomGame) PlayAudio(_randomOrder[_indexLetter]); else PlayAudio(_indexLetter); });
    }

    public void SetGameMode(bool keyboard) {
        _useKeyboard = keyboard;

        _choiceGameMethodScreen.GetComponent<CanvasGroup>().DOFade(0, _transitionTime).OnComplete(() => { _choiceGameMethodScreen.SetActive(false); });

        _choiceGameDifficultScreen.SetActive(true);
        _choiceGameDifficultScreen.GetComponent<CanvasGroup>().DOFade(1, _transitionTime);

        _helpIndex = 1;
    }

    public void SetDifficulty(bool difficulty) {
        _randomGame = difficulty;

        _indexLetter = 0;

        _choiceGameDifficultScreen.GetComponent<CanvasGroup>().DOFade(0, _transitionTime).OnComplete(() => { 
            _choiceGameDifficultScreen.SetActive(false);
           
            if(_randomGame) {
                _randomOrder.Clear();

                for (int i = 0; i < 26; i++) {
                    _randomOrder.Add(i);
                }

                for(int i = 0; i < _randomOrder.Count; i++) {
                    int temp = _randomOrder[i];
                    int newPos = UnityEngine.Random.Range(0, _randomOrder.Count -1);
                    int valuePos = _randomOrder[newPos];

                    _randomOrder[i] = valuePos;
                    _randomOrder[newPos] = temp;
                }

                SetLetter(_indexLetter);
            }
            else {
                SetLetter(_indexLetter);
            }
        });

        if (_randomGame && _useKeyboard) _helpIndex = 3;
        else if (!_randomGame && _useKeyboard) _helpIndex = 2;
        else if (_randomGame && !_useKeyboard) _helpIndex = 4;
        else if (!_randomGame && !_useKeyboard) _helpIndex = 2;

        _restartButton.gameObject.SetActive(true);
        _scoreManager.SetLevelQtd(_alphabet.Count);
    }

    private void PlayAudio(int index) {
        _as.Stop();
        _as.clip = _alphabet[index].clip;
        _as.Play();
    }

    List<int> wrongIndexes = new List<int>();

    private void SetLetter(int index) {
        if(_randomGame) {
            _textLetter.text = _alphabet[_randomOrder[index]].phonetic;
            PlayAudio(_randomOrder[index]);
        }
        else {
            _textLetter.text = _alphabet[index].phonetic;
            PlayAudio(index);
        }

        _audioButton.GetComponent<RectTransform>().DOScale(1, _transitionTime);

        wrongIndexes.Clear();

        if (!_useKeyboard) {
            for (int i = 0; i < _answersButtons.Count; i++) {

                int indexTemp = i;

                _answersButtons[indexTemp].GetComponent<RectTransform>().DOScale(0, 0);
                _answersButtons[indexTemp].GetComponent<Button>().onClick.RemoveAllListeners();

                if (indexTemp == 0) {
                    _answersButtons[indexTemp].GetComponent<Button>().onClick.AddListener(() => { Correct(index, _answersButtons[indexTemp].GetComponent<Button>()); });
                    if(_randomGame)
                        _answersButtons[indexTemp].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _alphabet[_randomOrder[index]].letter;
                    else
                        _answersButtons[indexTemp].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _alphabet[index].letter;
                }
                else {
                    int wrong = UnityEngine.Random.Range(0, _alphabet.Count - 1);

                    for(int r = 0; r < 26; r++) {
                        if(wrong == index || wrongIndexes.Contains(wrong)) {
                            wrong = UnityEngine.Random.Range(0, _alphabet.Count - 1);
                        }
                        else {
                            wrongIndexes.Add(wrong);

                            _answersButtons[indexTemp].GetComponent<Button>().onClick.AddListener(() => { Incorrect(wrong, _answersButtons[indexTemp].GetComponent<Button>()); });
                            _answersButtons[indexTemp].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _alphabet[wrong].letter;
                            break;
                        }
                    }
                }
            }

            for (int i = 0; i < _answersButtons[0].transform.parent.childCount; i++) {
                _answersButtons[i].transform.SetSiblingIndex(UnityEngine.Random.Range(0, 3));
            }

            for (int i = 0; i < _answersButtons.Count; i++) {
                _answersButtons[i].GetComponent<RectTransform>().DOScale(1, .2f);
            }
        }
        else {
            for (int i = 0; i < _answersButtons.Count; i++) {
                _answersButtons[i].GetComponent<RectTransform>().DOScale(0, 0);
            }
        }

        _attempts = 0;
    }

    private async void Correct(int clip, Button button = null) {
        BackgroundMusic.Instance.PlaySFX(_correctSFX);
        if (_randomGame) {
            PlayAudio(_randomOrder[clip]);
            await Task.Delay(TimeSpan.FromSeconds(_alphabet[_randomOrder[clip]].clip.length));
        }
        else {
            PlayAudio(clip);
            await Task.Delay(TimeSpan.FromSeconds(_alphabet[clip].clip.length));
        }
        _scoreManager.AddScore(_attempts);
        NextLetter();
    }

    private void Incorrect(int clip, Button button = null) {
        BackgroundMusic.Instance.PlaySFX(_incorrectSFX);
        PlayAudio(clip);
        _attempts++;
        if(button != null) { button.interactable = false; }
    }

    private void NextLetter() {
        _indexLetter++;

        for (int i = 0; i < _answersButtons.Count; i++) {
            _answersButtons[i].GetComponent<RectTransform>().DOScale(0, _transitionTime);
            _answersButtons[i].GetComponent<Button>().interactable = true;
        }

        _audioButton.GetComponent<RectTransform>().DOScale(0, _transitionTime).OnComplete(() => {
            if(_indexLetter < 26) {
                if (_randomGame) { SetLetter(_indexLetter); } else { SetLetter(_indexLetter); }
            }
            else {
                ShowEndGame();
            }
        });
    }

    public void ShowHelpScreen() {
        _helpScreen.gameObject.SetActive(true);
        _helpText.text = _helpTexts[_helpIndex];
    }

    private void ShowEndGame() {
        _gameScreen.GetComponent<CanvasGroup>().DOFade(0, .2f);
        _endGameScreen.SetActive(true);
        _endGameScreen.GetComponent<CanvasGroup>().DOFade(1, .5f);

        if(_scoreManager.GetScore() == 5) {
            _endText.text = "Você concluiu a atividade com maestria com 5 estrelas! \n Deseja jogar novamente?";
        }
        else if (_scoreManager.GetScore() > 1 && _scoreManager.GetScore() < 5) {
            _endText.text = "Você mandou bem, fez "+ _scoreManager.GetScore() + " estrelas!\n Mas pode melhorar! \n Deseja melhorar sua pontuação?";
        }
        else {
            _endText.text = "Você foi bem, conseguiu 1 estrela, mas acho que precisa praticar mais! \n Deseja praticar mais uma vez?";
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) { SceneManager.LoadScene(0); }

        if (_useKeyboard) {
            if (Input.GetKeyDown(KeyCode.A)) { CheckKeyboardLetter("A"); }
            if (Input.GetKeyDown(KeyCode.B)) { CheckKeyboardLetter("B"); }
            if (Input.GetKeyDown(KeyCode.C)) { CheckKeyboardLetter("C"); }
            if (Input.GetKeyDown(KeyCode.D)) { CheckKeyboardLetter("D"); }
            if (Input.GetKeyDown(KeyCode.E)) { CheckKeyboardLetter("E"); }
            if (Input.GetKeyDown(KeyCode.F)) { CheckKeyboardLetter("F"); }
            if (Input.GetKeyDown(KeyCode.G)) { CheckKeyboardLetter("G"); }
            if (Input.GetKeyDown(KeyCode.H)) { CheckKeyboardLetter("H"); }
            if (Input.GetKeyDown(KeyCode.I)) { CheckKeyboardLetter("I"); }
            if (Input.GetKeyDown(KeyCode.J)) { CheckKeyboardLetter("J"); }
            if (Input.GetKeyDown(KeyCode.K)) { CheckKeyboardLetter("K"); }
            if (Input.GetKeyDown(KeyCode.L)) { CheckKeyboardLetter("L"); }
            if (Input.GetKeyDown(KeyCode.M)) { CheckKeyboardLetter("M"); }
            if (Input.GetKeyDown(KeyCode.N)) { CheckKeyboardLetter("N"); }
            if (Input.GetKeyDown(KeyCode.O)) { CheckKeyboardLetter("O"); }
            if (Input.GetKeyDown(KeyCode.P)) { CheckKeyboardLetter("P"); }
            if (Input.GetKeyDown(KeyCode.Q)) { CheckKeyboardLetter("Q"); }
            if (Input.GetKeyDown(KeyCode.R)) { CheckKeyboardLetter("R"); }
            if (Input.GetKeyDown(KeyCode.S)) { CheckKeyboardLetter("S"); }
            if (Input.GetKeyDown(KeyCode.T)) { CheckKeyboardLetter("T"); }
            if (Input.GetKeyDown(KeyCode.U)) { CheckKeyboardLetter("U"); }
            if (Input.GetKeyDown(KeyCode.V)) { CheckKeyboardLetter("V"); }
            if (Input.GetKeyDown(KeyCode.W)) { CheckKeyboardLetter("W"); }
            if (Input.GetKeyDown(KeyCode.X)) { CheckKeyboardLetter("X"); }
            if (Input.GetKeyDown(KeyCode.Y)) { CheckKeyboardLetter("Y"); }
            if (Input.GetKeyDown(KeyCode.Z)) { CheckKeyboardLetter("Z"); }
        }
    }

    private void CheckKeyboardLetter(string letter) {
        if (_randomGame) {
            if (_alphabet[_randomOrder[_indexLetter]].letter == letter) { Correct(_randomOrder[_indexLetter]); }
            else { Incorrect(_randomOrder[_indexLetter]); }
        }
        else {
            if (_alphabet[_indexLetter].letter == letter) { Correct(_indexLetter); }
            else { Incorrect(_indexLetter); }
        }
    }

    public void ResetGame() {
        _loader.LoadLevelBtn("1-Game");
    }

    public void ReturnMenu() {
        _loader.LoadLevelBtn("1-Menu");
    }
}

[Serializable]
public struct AlphabetLetter {
    public string letter;
    public string phonetic;
    public Sprite sprite;
    public AudioClip clip;
}
