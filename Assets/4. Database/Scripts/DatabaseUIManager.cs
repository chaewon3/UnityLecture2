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
				systemMessage.text = "비밀번호가 틀렸습니다.";
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
				systemMessage.text = "비밀번호가 틀렸습니다.";
				StartCoroutine(systemPanelOn());
				return;
			}
			DatabaseManager.instance.Resgin(userdata.UID, OnResignSuccess, OnResignFailuew);
		}

		public void SignUpSubmit()
        {
			if(signEmail.text == "" || signName.text == "" || signPw.text =="")
            {
				systemMessage.text = "정보를 전부 입력해주세요.";
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
			levelText.text = $"레벨 : {userdata.level}";
        }

		private void OnLoginSuccess(UserData data)
        {
			userdata = data;

			loginPanel.SetActive(false);
			infoPanel.SetActive(true);

			StringBuilder sb = new StringBuilder();

			sb.AppendLine($"안녕하세요, {data.name}");
			sb.AppendLine($"이메일 : {data.email}");
			sb.AppendLine($"직업 : {data.charClass}");
			sb.AppendLine($"소개글 : {data.profileText}");

			infoText.text = sb.ToString();
			levelText.text = $"레벨 : {data.level}";
		}

		private void OnLoginFailure()
        {
			systemMessage.text = "로그인에 실패했습니다 ㅠㅠ";
			StartCoroutine(systemPanelOn());
		}

		private void OnSignUpSuccess()
        {
			systemMessage.text = "회원가입에 성공했습니다.";
			StartCoroutine(systemPanelOn());
			loginPanel.SetActive(true);
			SignUpPanel.SetActive(false);
		}

		private void OnSignUpFailure()
		{
			systemMessage.text = "이미 존재하는 아이디입니다.";
			StartCoroutine(systemPanelOn());
		}

		private void OnChangeInfoSuccess()
        {
			systemMessage.text = "정보가 수정되었습니다.";
			StartCoroutine(systemPanelOn());
			infoPanel.SetActive(true);
			InfoChangePanel.SetActive(false);

			StringBuilder sb = new StringBuilder();

			sb.AppendLine($"안녕하세요, {userdata.name}");
			sb.AppendLine($"이메일 : {userdata.email}");
			sb.AppendLine($"직업 : {userdata.charClass}");
			sb.AppendLine($"소개글 : {userdata.profileText}");

			infoText.text = sb.ToString();
		}

		private void OnChangeInfofailure()
        {
			systemMessage.text = "수정에 실패했습니다.";
			StartCoroutine(systemPanelOn());
		}

		private void OnResignSuccess()
        {
			systemMessage.text = "떠나신다니 아쉽습니다ㅠㅠ";
			StartCoroutine(systemPanelOn());
			InfoChangePanel.SetActive(false);
			loginPanel.SetActive(true);
		}

		private void OnResignFailuew()
        {
			systemMessage.text = "탈퇴에 실패했습니다.";
			StartCoroutine(systemPanelOn());
		}

		private void OnSearchSuccess(UserData data)
        {
			StringBuilder sb = new StringBuilder();

			sb.AppendLine($"유저 이름 : {data.name}");
			sb.AppendLine($"이메일 : {data.email}");
			sb.AppendLine($"직업 : {data.charClass}");
			sb.AppendLine($"소개글 : {data.profileText}");

			infoText.text = sb.ToString();
		}

		private void OnSearchFailure()
        {
			systemMessage.text = "검색된 유저가 없습니다.";
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
