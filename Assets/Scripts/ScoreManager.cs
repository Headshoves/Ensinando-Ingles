using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] private RectTransform _placar;
    [SerializeField] private TextMeshProUGUI _pontuacao;

    private int _stars = 0;
    private int _levelQtd = 0;

    private int _totalScore;
    private int _actualScore;

    public void SetLevelQtd(int qtd) {
        _actualScore = 0;
        _stars = 0;
        _levelQtd = qtd;
        _totalScore = _levelQtd * 5;
        _pontuacao.text = _actualScore + " / " + _totalScore;
    }

    public void AddScore(int totalErrors) {
        int totalPoints = 5;
        totalPoints = totalErrors > 4 ? 1 : totalPoints - totalErrors;

        _actualScore += totalPoints;

        _stars = StarsCount();

    }

    private int StarsCount() {

        float stars = ((float)_actualScore / (float)_totalScore);
        stars = stars / 0.2f;

        if((int)stars != _stars) {
            _placar.DOScale(1.2f, .2f).OnComplete(() => { _placar.DOScale(1f, .2f); });
            _placar.GetChild((int)stars -1).DOScale(1.5f, .3f).OnComplete(() => { _placar.GetChild((int)stars-1).DOScale(1f, .3f); });
        }

        _pontuacao.text = _actualScore + " / " + _totalScore;

        if (_actualScore == _totalScore) return 5;
        return (int)stars;
    }

    public int GetScore() { return _stars; }
}
