using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GeneralMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _botaoComecar;
    [SerializeField] private CanvasGroup _telaGeral;

    private AsyncLoader _loader;

    void Start()
    {
        _loader = GetComponent<AsyncLoader>();

        _botaoComecar.GetComponent<Button>().onClick.RemoveAllListeners();
        _botaoComecar.GetComponent<Button>().onClick.AddListener(() => { GoToGame(); });
    }

    private void GoToGame() {
        _botaoComecar.GetComponent<DOTweenAnimation>().enabled = false;
        _botaoComecar.GetComponent<RectTransform>().DOScale(.8f, .2f).OnComplete(() => {
            _telaGeral.DOFade(0, .1f).OnComplete(() => { _loader.LoadLevelBtn("1-Menu"); });
        });
    }
}
