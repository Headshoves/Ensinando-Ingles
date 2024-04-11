using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AlphabetManager : MonoBehaviour
{
    [BoxGroup("Settings")][SerializeField] private AudioSource _audioSource;
    [BoxGroup("Settings")][SerializeField] private float _timeToScale = 0.2f;

    [SerializeField] private List<AudioClip> _alphabetClips = new List<AudioClip>();
    [SerializeField] private List<Sprite> _alphabetLetter = new List<Sprite>();
    [SerializeField] private List<Sprite> _alphabetPhonetic = new List<Sprite>();

    [SerializeField] private GameObject _question;
    [SerializeField] private GameObject _correctAnswer;
    [SerializeField] private GameObject[] _incorrectAnswers;

    [BoxGroup("Feedbacks")][SerializeField] private GameObject _correctScreen;


    private List<int> phases = new List<int>();
    private void Start() {
        _question.GetComponent<Button>().onClick.RemoveAllListeners();
        _question.GetComponent<Button>().onClick.AddListener(() => { PlaySound(); });

        _correctAnswer.GetComponent<Button>().onClick.RemoveAllListeners();
        _correctAnswer.GetComponent<Button>().onClick.AddListener(() => { CorrectAnswer(); });

        for(int i = 0; i < _incorrectAnswers.Length; i++) {
            int index = i;
            _incorrectAnswers[index].GetComponent<Button>().onClick.RemoveAllListeners();
            _incorrectAnswers[index].GetComponent<Button>().onClick.AddListener(() => { IncorrectAnswer(_incorrectAnswers[index].GetComponent<Button>()); });
        }


        for (int i = 0; i < _alphabetClips.Count; i++) {
            phases.Add(i);
        }

        System.Random random = new System.Random();
        phases = phases.OrderBy(x => random.Next()).ToList();
        NextLevel();
    }

    private void PlaySound() {
        _audioSource.Stop();
        _audioSource.clip = _alphabetClips[phases[0]];
        _audioSource.Play();
    }

    private void NextLevel() {

        for (int i = 0; i < _incorrectAnswers.Length; i++) { 
            int index = i;
            _incorrectAnswers[index].GetComponent<Button>().interactable = true;
            _incorrectAnswers[index].transform.DOScale(0, _timeToScale); 
        }

        _question.transform.DOScale(0, _timeToScale);
        _correctAnswer.transform.DOScale(0, _timeToScale).OnComplete(() => {
            _question.GetComponent<Image>().sprite = _alphabetPhonetic[phases[0]];
            _question.transform.DOScale(1, _timeToScale);

            List<int> tempPhases = phases;

            for (int i = 0; i < _incorrectAnswers.Length; i++) {
                int index = i;

                int indexLetter = Random.Range(1, tempPhases.Count);
                tempPhases.Remove(indexLetter);
                _incorrectAnswers[index].GetComponent<Image>().sprite = _alphabetLetter[indexLetter];

                _incorrectAnswers[index].transform.SetSiblingIndex(Random.Range(0, _incorrectAnswers.Length));
                _incorrectAnswers[index].transform.DOScale(1, _timeToScale);
            }

            _correctAnswer.transform.SetSiblingIndex(Random.Range(0, _incorrectAnswers.Length));
            _correctAnswer.GetComponent<Image>().sprite = _alphabetLetter[phases[0]];
            _correctAnswer.transform.DOScale(1, _timeToScale);
        });

    }

    private void CorrectAnswer() {
        phases.RemoveAt(0);
        NextLevel();
    }

    private void IncorrectAnswer(Button button) {
        button.interactable = false;
    }
}
