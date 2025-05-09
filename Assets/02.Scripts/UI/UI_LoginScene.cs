using DG.Tweening;
using System;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UIInputFields
{
    public TMP_InputField IDInputField;
    public TMP_InputField PasswordInputField;
    public TMP_InputField PassConfirmInputField;
    public Button ConfirmButton;
}

public class UI_LoginScene : MonoBehaviour
{
    [Header("Panel")]
    public GameObject LoginPanel;
    public GameObject RegisterPanel;
    public TextMeshProUGUI ResultPanel;

    [Header("Login")]
    public UIInputFields LoginInputFields;

    [Header("Register")]
    public UIInputFields RegisterInputFields;

    private const string SALT = "726851";
    private const string PREFIX = "ID_";

    private void Start()
    {
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);

        SetResultText("로그인하세요");
    }

    public void OnClickGoToRegisterButton()
    {
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(true);
        RegisterInputFields.PassConfirmInputField.gameObject.SetActive(true);
    }

    public void OnClickGoToLoginButton()
    {
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);
        RegisterInputFields.PassConfirmInputField.gameObject.SetActive(false);
    }
    private void SetResultText(string message)
    {
        ResultPanel.text = message;

        // DOTween 애니메이션 효과: 살짝 흔들림
        ResultPanel.rectTransform.DOKill(); // 이전 트윈 제거
        ResultPanel.rectTransform.localScale = Vector3.one; // 스케일 초기화
        ResultPanel.rectTransform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 0.5f);
    }

    public void Login()
    {
        string id = LoginInputFields.IDInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            SetResultText("ID를 입력해주세요");
            return;
        }

        string pass = LoginInputFields.PasswordInputField.text;
        if (string.IsNullOrEmpty(pass))
        {
            SetResultText("비밀번호를 입력해주세요");
            return;
        }

        string encrypted = Encryption(pass + SALT);
        string stored = PlayerPrefs.GetString(PREFIX + id, null);

        if (!string.IsNullOrEmpty(stored))
        {
            if (stored == encrypted)
            {
                SetResultText("로그인에 성공했습니다");
                // 게임 씬으로 이동
            }
            else
            {
                SetResultText("비밀번호가 틀렸습니다");
            }
        }
        else
        {
            SetResultText("ID를 등록해 주세요");
            OnClickGoToRegisterButton();
        }
    }

    public void Register()
    {
        // 1. 아이디 입력을 확인한다
        string id = RegisterInputFields.IDInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            SetResultText("ID를 입력해주세요");
            // ResultText 흔들기
            return;
        }

        // 2. 1차 비밀번호 입력을 확인한다
        string password = RegisterInputFields.PasswordInputField.text;
        if (string.IsNullOrEmpty(password))
        {
            SetResultText("비밀번호를 입력해주세요");
            return;
        }

        // 3. 2차 비밀번호 입력을 확인하고, 1차 비밀번호 입력과 같은지 확인한다
        string passcheck = RegisterInputFields.PassConfirmInputField.text;
        if (string.IsNullOrEmpty(passcheck))
        {
            SetResultText("비밀번호를 한번 더 입력해주세요");
            return;
        } else
        {
            if(password == passcheck)
            {
                // 4. PlayerPrefs를 이용해서 아이디/ 비번 저장
                password = Encryption(password + SALT);
                PlayerPrefs.SetString(PREFIX + id, password);
            }
            else
            {
                SetResultText("비밀번호가 일치하지 않습니다");
                return;
            }
        }

        // 5. 로그인 창으로 돌아간다 (아이디 자동입력)
        LoginInputFields.IDInputField.text = id;
        LoginInputFields.PasswordInputField.text = string.Empty;
        SetResultText("등록에 성공했습니다");
        OnClickGoToLoginButton();
    }

    public string Encryption(string text)
    {
        SHA256 sha256 = SHA256.Create();

        byte[] bytes = Encoding.UTF8.GetBytes(text);
        byte[] hash = sha256.ComputeHash(bytes);

        string resultText = string.Empty;
        foreach(byte b in hash)
        {
            resultText += b.ToString("X2");
        }
        return resultText;     
    }
}
