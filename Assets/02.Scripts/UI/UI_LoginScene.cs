using DG.Tweening;
using System;
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


    private void Start()
    {
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);

        SetResultText("�α����ϼ���");
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

        // DOTween �ִϸ��̼� ȿ��: ��¦ ��鸲
        ResultPanel.rectTransform.DOKill(); // ���� Ʈ�� ����
        ResultPanel.rectTransform.localScale = Vector3.one; // ������ �ʱ�ȭ
        ResultPanel.rectTransform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 0.5f);
    }


    public void Login()
    {
        string id = LoginInputFields.IDInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            SetResultText("ID�� �Է����ּ���");
            return;
        }

        string pass = LoginInputFields.PasswordInputField.text;
        if (string.IsNullOrEmpty(pass))
        {
            SetResultText("��й�ȣ�� �Է����ּ���");
            return;
        }
        else
        {
            if(PlayerPrefs.GetString("ID") == id)
            {
                if(PlayerPrefs.GetString("PASS") == pass)
                {
                    SetResultText("�α��ο� �����߽��ϴ�");
                }
                else
                {
                    SetResultText("��й�ȣ�� Ʋ�Ƚ��ϴ�");
                }
            } else
            {
                SetResultText("ID�� ����� �ּ���");
                OnClickGoToRegisterButton();
            }
        }
    }

    public void Register()
    {
        // 1. ���̵� �Է��� Ȯ���Ѵ�
        string id = RegisterInputFields.IDInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            SetResultText("ID�� �Է����ּ���");
            // ResultText ����
            return;
        }

        // 2. 1�� ��й�ȣ �Է��� Ȯ���Ѵ�
        string password = RegisterInputFields.PasswordInputField.text;
        if (string.IsNullOrEmpty(password))
        {
            SetResultText("��й�ȣ�� �Է����ּ���");
            return;
        }

        // 3. 2�� ��й�ȣ �Է��� Ȯ���ϰ�, 1�� ��й�ȣ �Է°� ������ Ȯ���Ѵ�
        string passcheck = RegisterInputFields.PassConfirmInputField.text;
        if (string.IsNullOrEmpty(passcheck))
        {
            SetResultText("��й�ȣ�� �ѹ� �� �Է����ּ���");
            return;
        } else
        {
            if(password == passcheck)
            {
                // 4. PlayerPrefs�� �̿��ؼ� ���̵�/ ��� ����
                PlayerPrefs.SetString("ID", id);
                PlayerPrefs.SetString("PASS", password);
            }
            else
            {
                SetResultText("��й�ȣ�� ��ġ���� �ʽ��ϴ�");
                return;
            }
        }

        // 5. �α��� â���� ���ư��� (���̵� �ڵ��Է�)
        LoginInputFields.IDInputField.text = id;
        LoginInputFields.PasswordInputField.text = string.Empty;
        SetResultText("��Ͽ� �����߽��ϴ�");
        OnClickGoToLoginButton();
    }
}
