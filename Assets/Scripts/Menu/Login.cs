using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;

public class Login : MonoBehaviour
{
    [BoxGroup("Login")][SerializeField] private TMP_InputField _loginInput;
    [BoxGroup("Login")][SerializeField] private TextMeshProUGUI _loginPrompt;

    [BoxGroup("Password")][SerializeField] private TMP_InputField _passwordInput;
    [BoxGroup("Password")][SerializeField] private TextMeshProUGUI _passwordPrompt;

    [BoxGroup("Teste")][SerializeField] private string _username;
    [BoxGroup("Teste")][SerializeField] private string _password;

    private void Start() {
        _passwordPrompt.gameObject.SetActive(false);
        _loginPrompt.gameObject.SetActive(false);
    }

    public void TryLogin() {

    }

    #region PROMPT
    public void OnChangeLogin() {
        if(_loginInput.text.Length < 1) {
            _loginPrompt.gameObject.SetActive(true);
            _loginPrompt.text = "Insira o seu nome de usuário.";
            _loginPrompt.color = new Color(1, 0, 0, 1);
        }
        else {
            _loginPrompt.gameObject.SetActive(false);
        }
    }

    public void OnChangePassword() {
        if (_passwordInput.text.Length < 1) {
            _passwordPrompt.gameObject.SetActive(true);
            _passwordPrompt.text = "Insira a sua senha.";
            _passwordPrompt.color = new Color(1, 0, 0, 1);
        }
        else {
            _passwordPrompt.gameObject.SetActive(false);
        }
    }
    #endregion
}
