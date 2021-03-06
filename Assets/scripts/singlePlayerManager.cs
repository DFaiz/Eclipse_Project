﻿using UnityEngine;
using System.Collections;

public class singlePlayerManager : MonoBehaviour {

	public GameObject Space;
	public OVRCameraRig ovrCam;
	public Camera screenCam;
	public bool amIAlive;
	public float respawnTime;
	public GameObject Fighters;
	public Canvas endGameCanvas;

	int whoAmI;
	spawnSpot[] spots;
	FighterSpawningSpot[] fighterSpots;
	int spaceshipId;
	int groupId; 
	GameObject Fighter;
	Camera[] displayCams;

	void Start(){
		PhotonNetwork.offlineMode = true;
		modeSelect ();
		amIAlive = true;
		Debug.Log ("Start");
		whoAmI = 0; //Pilot
		PhotonNetwork.ConnectUsingSettings("Alpha");
		fighterSpots = GameObject.FindObjectsOfType<FighterSpawningSpot>();
		
	}
	
	void Update(){
		if (amIAlive == false) {
			if (respawnTime > 0) {
				
				respawnTime -= Time.deltaTime;		
			}
			if (respawnTime <= 0) {
				spawn ();
				
			}
		}
		if (Input.GetKey(KeyCode.Escape)) {
			Debug.Log ("espcaped game");
			endGameCanvas.gameObject.SetActive(true);
			endGameCanvas.GetComponent<EndGame>().Fighter = Fighter;
		}
	}
	
	
	void OnGUI(){
		/*Debug.Log (PhotonNetwork.connectionStateDetailed.ToString ());
		GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString());
		GUILayout.Label ("Number of players in room "+PhotonNetwork.countOfPlayers.ToString());
		GUILayout.Label ("player ID " + PhotonNetwork.player.ID);
		if (!(Fighter == null)) {
			GUILayout.Label ("Number of players in room "+Fighter.transform.position.ToString());
		}
		if(amIAlive){
			GUI.Label (new Rect (Screen.width / 2, 10, 300, 300), "Respawn in:" + (int)respawnTime);
		}*/
	}
	
	void OnJoinedLobby(){
		Debug.Log ("OnJoinedToLobby");
		
		RoomOptions roomOptions = new RoomOptions (){isVisible = true};
		PhotonNetwork.JoinOrCreateRoom("mmo",roomOptions,TypedLobby.Default);
	}
	
	
	void OnJoinedRoom(){
		Debug.Log ("OnJoinedRoom");
		getGroupId ();
		
		
		spawn ();
		
	}
	
	void spawn(){
		//where the fighter is spawning
		if (fighterSpots == null) {
			Debug.LogWarning ("unable spawn a Fighter to a Fighter spot, fighterSpots = null");
			return;
		}
		FighterSpawningSpot myFighterSpot = fighterSpots [Random.Range (0, fighterSpots.Length)];
		
		
		//instantiating a fighter into the game
		Fighter = PhotonNetwork.Instantiate ("SinglePlayerNewFighter", myFighterSpot.transform.position, myFighterSpot.transform.rotation, 0);
		amIAlive = true;
		
		
		
		
		//spawning he player into the location in the fighter
		spots = Fighter.GetComponentsInChildren<spawnSpot>();
		if (spots == null) {
			Debug.LogWarning ("unable spawn a player to a player spot, spots = null");
			return;
		}
		setupCamera (spots [whoAmI]);
	}
	
	void getSpaceshipId(){
		if (PhotonNetwork.player.ID % 2 == 0) {
			spaceshipId = (PhotonNetwork.player.ID - 1) + 1000;
			return;
		} else {
			spaceshipId = PhotonNetwork.player.ID+1000;
			return;
		}
		
	}
	
	void getGroupId(){
		int num = PhotonNetwork.player.ID;
		if (num % 2 == 0) {
			groupId = (num / 2) - 1;
			return;
		} else {
			groupId = num / 2;
			return;
		}
		
	}
	
	/*public void OnFailedToConnectToPhoton()
	{
		Debug.Log("OnFailedToConnectToPhoton");
		
		if (CheckForInternetConnection () == false) {
			Debug.Log("No internet connection detected - going back to main menu");
			PhotonNetwork.Disconnect();
			Application.LoadLevel ("spaceship2");
		}
		
		Debug.Log("Internet connetion detected");
		//OnJoinedRoom (); this is not a function thats a callback

		PhotonNetwork.JoinOrCreateRoom("mmo",roomOptions,TypedLobby.Default);
	}*/
	
	
	/*void OnPhotonJoinRoomFailed()
	{
		Debug.Log("OnPhotonJoinRoomFailed");
		Debug.Log("Creating room");
		PhotonNetwork.CreateRoom ("mmo");// need to change room name to something with value
	}*/
	
	/*
	public static bool CheckForInternetConnection()
	{
		try
		{
			using (var client = new WebClient())
				using (var stream = client.OpenRead("http://www.google.com"))
			{
				Debug.Log("Internet connection is ok");
				return true;
			}
		}
		catch
		{
			Debug.Log("No internet connection - I'm out");
			return false;
		}
	}*/
	void modeSelect(){
		
		if (PlayerPrefs.GetString ("cameraMode") == "screen") {
			ovrCam.gameObject.SetActive(false);
			Debug.Log ("cam");
		} else {
			if (PlayerPrefs.GetString ("cameraMode") == "OVR") {
				Debug.Log ("OVR");		
			} 		
		}
		
	}
	
	void setupCamera(spawnSpot mySpot){
		if (PlayerPrefs.GetString ("cameraMode") == "screen") {
			Fighter.GetComponent<networkFighter> ().myScreenCam = screenCam;
			screenCam.transform.position = mySpot.transform.position;
			screenCam.transform.rotation = mySpot.transform.rotation;
			screenCam.GetComponent<CameraFollow> ().SetTarget (mySpot.transform);
		} else {
			if (PlayerPrefs.GetString ("cameraMode") == "OVR") {
				Fighter.GetComponent<networkFighter> ().myOVRCam = ovrCam;
				ovrCam.transform.position = mySpot.transform.position;
				ovrCam.transform.rotation = mySpot.transform.rotation;
				ovrCam.GetComponent<CameraFollow> ().SetTarget (mySpot.transform);
			} 		
			
		}
	}


}
