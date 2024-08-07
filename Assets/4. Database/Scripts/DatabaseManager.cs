using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySqlConnector;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace MyProject
{
	public class DatabaseManager : MonoBehaviour
	{
		private string severIP = "127.0.0.1";
		private string dbName = "game";
		private string tableName = "users";
		private string rootPassward = "0619"; // 테스트 시에 활용할 수 있지만 보안에 취약하므로 주의

		private MySqlConnection conn; // mysql DB와 연결상태를 유지하는 객체.

		public static DatabaseManager instance { get; private set; }

        private void Awake()
        {
			instance = this;
        }

        private void Start()
        {
			DBConnect();
        }

        public void DBConnect()
        {
			string config = $"server={severIP};port=3306;database={dbName};uid=root;pwd={rootPassward};charset=utf8;";
		
			conn = new MySqlConnection(config);
			conn.Open();
			//print(conn.State);
		}


		// 로그인을 하려고 할 때, 로그인 쿼리를 날린 즉시 데이터가 오지 않을 수 있으므로,
		// 로그인이 완료 되었을 때 호출될 함수를 파라미터로 함께 받아주도록 함.
		public void Login(string email, string passwd, Action<UserData> successCallback, Action failureCallback)
        {
			MySqlCommand cmd = new MySqlCommand();
			cmd.Connection = conn;
			cmd.CommandText = $"SELECT*FROM {tableName} WHERE email='{email}' AND pw='{passwd}'";

			MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
			DataSet set = new DataSet();

			dataAdapter.Fill(set);

			bool isLoginSuccess = set.Tables.Count > 0 && set.Tables[0].Rows.Count > 0;

			if (isLoginSuccess)
            {
				//로그인 성공 email과 pw값이 동시에 일치하는 행이 존재함
				DataRow row = set.Tables[0].Rows[0];
				print(row["level"]);
				print(row["LEVEL"]);

				UserData data = new UserData(row);

				print(data.email);

				successCallback?.Invoke(data);
            }
			else
            {
				// 로그인 실패
				failureCallback?.Invoke();
            }
			
        }
		
		public void signUp(string email, string passwd, string name, Action successCallback, Action failureCallback)
        {
			MySqlCommand cmd = new MySqlCommand();
			cmd.Connection = conn;
			cmd.CommandText = $"SELECT*FROM {tableName} WHERE email='{email}'";

			MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
			DataSet set = new DataSet();

			dataAdapter.Fill(set);

			bool isSignUpSuccess = set.Tables.Count > 0 && set.Tables[0].Rows.Count > 0;

			if(!isSignUpSuccess)
            {
				cmd.CommandText = $"INSERT INTO {tableName}(email, pw, name) VALUES('{email}', '{passwd}','{name}')";

				int queryCount = cmd.ExecuteNonQuery();
				if(queryCount >0)
					successCallback?.Invoke();
				else
					failureCallback?.Invoke();
			}
			else
            {
				failureCallback?.Invoke();
            }
		}

		public void ChangeInfo(UserData data,string name, string profile, Action successCallback, Action failureCallback)
        {
			int uid = data.UID;

			MySqlCommand cmd = new MySqlCommand();
			cmd.Connection = conn;
			cmd.CommandText = $"UPDATE {tableName} SET name = '{name}', profile_text = '{profile}' WHERE uid = '{uid}' ";

			int queryCount = cmd.ExecuteNonQuery();

			if(queryCount >0)
            {
				data.name = name;
				data.profileText = profile;
				successCallback?.Invoke();
			}
			else
				failureCallback?.Invoke();
        }

		public void Resgin(int uid, Action successCallback, Action failureCallback)
        {
			MySqlCommand cmd = new MySqlCommand();
			cmd.Connection = conn;
			cmd.CommandText = $"DELETE FROM {tableName} WHERE uid = '{uid}'";

			int queryCount = cmd.ExecuteNonQuery();

			if (queryCount > 0)
				successCallback?.Invoke();
			else
				failureCallback?.Invoke();
		}

		//나중에 만들떄는 실패한 함수도 같이 만들어야 함 
		public void LevelUP(UserData data, Action successCallback)
        {
			int level = data.level;
			int nextLevel = level + 1;

			MySqlCommand cmd = new MySqlCommand();
			cmd.Connection = conn;
			cmd.CommandText = $"UPDATE {tableName} SET level={nextLevel} WHERE uid={data.UID}";

			int queryCount = cmd.ExecuteNonQuery();

			if(queryCount > 0)
            {
				//쿼리가 정상적으로 수행됨
				data.level = nextLevel;
				successCallback?.Invoke();
            }
			else
            {
				//쿼리 수행 실패
            }
        }

		public void Search(string name, Action<UserData> successCallback, Action failureCallback)
        {
			MySqlCommand cmd = new MySqlCommand();
			cmd.Connection = conn;
			cmd.CommandText = $"SELECT*FROM {tableName} WHERE name='{name}'";

			MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
			DataSet set = new DataSet();

			dataAdapter.Fill(set);

			bool isSignUpSuccess = set.Tables.Count > 0 && set.Tables[0].Rows.Count > 0;

			if(isSignUpSuccess)
            {
				DataRow row = set.Tables[0].Rows[0];
				UserData data = new UserData(row);

				successCallback?.Invoke(data);
			}
			else
            {
				failureCallback?.Invoke();
			}
		}
	}
}
