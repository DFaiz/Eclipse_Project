﻿using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	public Transform cameraTarget;
	Transform myTransform;
	// Use this for initialization
	void Start () 
	{
		myTransform = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (cameraTarget == null) {
			myTransform.position = Vector3.zero;
			myTransform.rotation = Quaternion.identity;
		} else {
			myTransform.position = cameraTarget.transform.position;
			myTransform.rotation = cameraTarget.transform.rotation;

		}
	}

	public void SetTarget(Transform target)
	{
		cameraTarget = target;
	}
}
