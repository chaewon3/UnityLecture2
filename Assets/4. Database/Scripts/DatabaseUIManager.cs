using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject
{
	public class DatabaseUIManager : MonoBehaviour
	{
		public GameObject loginPanel;
		public GameObject infoPanel;
		public GameObject SignUpPanel;
		public GameObject systemPanel;
		public GameObject InfoChangePanel;

		public InputField emailInput;
		public InputField pwInput;

		public InputField signEmail;
		public InputField signPw;
		public InputField signName;

		public Button signUpButton;
		public Button loginButton;
		public Button submitButton;
		public Button InfoButton;

		public Text infoText;
		public Text levelText;
		public Text systemMessage;

		private UserData userdata;

		private void Awake()
		{
			loginButton.onClick.AddListener(LoginButtonClick);
			signUpButton.onClick.AddListener(SignUpButtonClick);
			submitButton.onClick.AddListener(SignUpSubmit);
			InfoButton.onClick.AddListener(ChangeInfoButtonClick);
		}


		public void LoginButtonClick()
		{
			DatabaseManager.instance.Login(emailInput.text, pwInput.text, OnLoginSuccess, OnLoginFailure);
		}

		public void SignUpButtonClick()
		{
			loginPanel.SetActive(false);
			SignUpPanel.SetActive(true);
		}

		public void ChangeInfoButtonClick()
        {
			infoPanel.SetActive(false);
			InfoChangePanel.SetActive(true);
		}

		public void SignUpSubmit()
        {
			if(signEmail.text == "" || signName.text == "" || signPw.text =="")
            {
				systemMessage.text = "������ ���� �Է����ּ���.";
				StartCoroutine(systemPanelOn());
				return;
			}

			DatabaseManager.instance.signUp(signEmail.text,signPw.text, signName.text, OnSignUpSuccess, OnSignUpFailure);
        }

		public void OnLevelUPButtonClick()
        {
			DatabaseManager.instance.LevelUP(userdata, OnLevelUPSuccess);
        }

		private void OnLevelUPSuccess()
        {
			levelText.text = $"���� : {userdata.level}";
        }

		private void OnLoginSuccess(UserData data)
        {
			print("�α��� ����!");
			userdata = data;

			loginPanel.SetActive(false);
			infoPanel.SetActive(true);

			StringBuilder sb = new StringBuilder();

			sb.AppendLine($"�ȳ��ϼ���, {data.name}");
			sb.AppendLine($"�̸��� : {data.email}");
			sb.AppendLine($"���� : {data.charClass}");
			sb.AppendLine($"�Ұ��� : {data.profileText}");

			infoText.text = sb.ToString();
			levelText.text = $"���� : {data.level}";
		}

		private void OnLoginFailure()
        {
			systemMessage.text = "�α��ο� �����߽��ϴ� �Ф�";
			StartCoroutine(systemPanelOn());
		}

		private void OnSignUpSuccess()
        {
			systemMessage.text = "ȸ�����Կ� �����߽��ϴ�.";
			StartCoroutine(systemPanelOn());

			loginPanel.SetActive(true);
			SignUpPanel.SetActive(false);
		}

		private void OnSignUpFailure()
		{
			systemMessage.text = "�̹� �����ϴ� ���̵��Դϴ�.";
			StartCoroutine(systemPanelOn());
		}

		IEnumerator systemPanelOn()
        {
			systemPanel.SetActive(true);
			yield return new WaitForSeconds(1.3f);
			systemPanel.SetActive(false);

		}

	}
}
