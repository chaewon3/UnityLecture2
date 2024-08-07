using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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

		public InputField infoName;
		public InputField infoProfile;
		public InputField infoPw;

		public InputField UserSearch;

		public Button signUpButton;
		public Button loginButton;
		public Button submitButton;
		public Button InfoButton;
		public Button ChangeButton;
		public Button resignButton;
		public Button SearchButton;

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
			ChangeButton.onClick.AddListener(ChangeInfoSubmit);
			resignButton.onClick.AddListener(ResignButtonClick);
			SearchButton.onClick.AddListener(SearchButtonClick);
		}

		public void LoginButtonClick()
		{
			string hashPW = HashPW(pwInput.text);
			DatabaseManager.instance.Login(emailInput.text, hashPW, OnLoginSuccess, OnLoginFailure);
			emailInput.text = "";
			pwInput.text = "";
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
			infoName.text = userdata.name;
			infoProfile.text = userdata.profileText;
		}

		public void ChangeInfoSubmit()
        {
			string hashPW = HashPW(infoPw.text);

			if (!(userdata.ComparePasswd(hashPW)))
            {
				systemMessage.text = "��й�ȣ�� Ʋ�Ƚ��ϴ�.";
				StartCoroutine(systemPanelOn());
				return;
			}
			DatabaseManager.instance.ChangeInfo(userdata,infoName.text,infoProfile.text, OnChangeInfoSuccess,OnChangeInfofailure);
		}

		public void ResignButtonClick()
        {
			string hashPW = HashPW(signPw.text);

			if (!(userdata.ComparePasswd(hashPW)))
			{
				systemMessage.text = "��й�ȣ�� Ʋ�Ƚ��ϴ�.";
				StartCoroutine(systemPanelOn());
				return;
			}
			DatabaseManager.instance.Resgin(userdata.UID, OnResignSuccess, OnResignFailuew);
		}

		public void SignUpSubmit()
        {
			if(signEmail.text == "" || signName.text == "" || signPw.text =="")
            {
				systemMessage.text = "������ ���� �Է����ּ���.";
				StartCoroutine(systemPanelOn());
				return;
			}

			string hashPW = HashPW(signPw.text);
			DatabaseManager.instance.signUp(signEmail.text, hashPW, signName.text, OnSignUpSuccess, OnSignUpFailure);
        }

		public void OnLevelUPButtonClick()
        {
			DatabaseManager.instance.LevelUP(userdata, OnLevelUPSuccess);
        }

		public void SearchButtonClick()
        {
			DatabaseManager.instance.Search(UserSearch.text, OnSearchSuccess, OnSearchFailure);
		}

		private void OnLevelUPSuccess()
        {
			levelText.text = $"���� : {userdata.level}";
        }

		private void OnLoginSuccess(UserData data)
        {
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

		private void OnChangeInfoSuccess()
        {
			systemMessage.text = "������ �����Ǿ����ϴ�.";
			StartCoroutine(systemPanelOn());
			infoPanel.SetActive(true);
			InfoChangePanel.SetActive(false);

			StringBuilder sb = new StringBuilder();

			sb.AppendLine($"�ȳ��ϼ���, {userdata.name}");
			sb.AppendLine($"�̸��� : {userdata.email}");
			sb.AppendLine($"���� : {userdata.charClass}");
			sb.AppendLine($"�Ұ��� : {userdata.profileText}");

			infoText.text = sb.ToString();
		}

		private void OnChangeInfofailure()
        {
			systemMessage.text = "������ �����߽��ϴ�.";
			StartCoroutine(systemPanelOn());
		}

		private void OnResignSuccess()
        {
			systemMessage.text = "�����Ŵٴ� �ƽ����ϴ٤Ф�";
			StartCoroutine(systemPanelOn());
			InfoChangePanel.SetActive(false);
			loginPanel.SetActive(true);
		}

		private void OnResignFailuew()
        {
			systemMessage.text = "Ż�� �����߽��ϴ�.";
			StartCoroutine(systemPanelOn());
		}

		private void OnSearchSuccess(UserData data)
        {
			StringBuilder sb = new StringBuilder();

			sb.AppendLine($"���� �̸� : {data.name}");
			sb.AppendLine($"�̸��� : {data.email}");
			sb.AppendLine($"���� : {data.charClass}");
			sb.AppendLine($"�Ұ��� : {data.profileText}");

			infoText.text = sb.ToString();
		}

		private void OnSearchFailure()
        {
			systemMessage.text = "�˻��� ������ �����ϴ�.";
			StartCoroutine(systemPanelOn());
		}

		private string HashPW(string passwd)
		{
			string pwhash = "";
			SHA256 sha256 = SHA256.Create();
			byte[] hashArray = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwd));

			foreach (byte b in hashArray)
			{
				pwhash += $"{b:X2}";
			}
			sha256.Dispose();

			return pwhash;
		}

		IEnumerator systemPanelOn()
        {
			systemPanel.SetActive(true);
			yield return new WaitForSeconds(1.3f);
			systemPanel.SetActive(false);
		}

	}
}
