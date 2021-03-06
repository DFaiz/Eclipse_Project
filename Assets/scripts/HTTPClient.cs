﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class HTTPClient : MonoBehaviour {

	private string login_url = "http://52.24.91.179/gamelogin";
	private int success_fail = -1;

	//Login request result options:
	// 0 - cannot connect to server - server down/no internet
	// 1 - cannot login - check username/password or register in website
	// 2 - login success
	// 3 - Login Failed - User Already Logged From Different PC
	// 4 - Cannot Login - Server Failure - Please Try Again Later
	
	//Indicates if and when the request process has finished.
	private bool request_status = false;

	public bool GetStatus ()
	{
		return request_status;
	}

	public void ResetStatus ()
	{
		request_status = false;
	}

	public int GetResultStatus ()
	{
		return success_fail;
	}

	public void Login_POST(string username, string passw)
	{
		WWWForm form = new WWWForm();
		form.AddField("usr", username);
		form.AddField("pass", passw);

		WWW www = new WWW(login_url, form);

		StartCoroutine (WaitForLoginRequest (www));
	}

	private IEnumerator WaitForLoginRequest (WWW www)
	{
		Debug.Log ("in wait");
		yield return StartCoroutine(ExecuteLoginRequest(www));
		//response is JSON
		request_status = true;
		Debug.Log ("out wait");
	}

	private IEnumerator ExecuteLoginRequest(WWW www)
	{
		Debug.Log ("in exec");
		yield return www;
		if (www.error == null)
		{
			if(www.text.Contains("user exists"))
			{
				success_fail = 2;
				Debug.Log(success_fail);
			}
			if(www.text.Contains("user does not exist"))
			{
				success_fail = 1;
				Debug.Log(success_fail);
			}
			if(www.text.Contains("user already logged in"))
			{
				success_fail = 3;
				Debug.Log(success_fail);
			}
			if(www.text.Contains("database query failure"))
			{
				success_fail = 4;
				Debug.Log(success_fail);
			}
		} 
		else {
			success_fail = 0;
			Debug.Log("www error"+success_fail);
		}
		Debug.Log ("out exec");
	}
}