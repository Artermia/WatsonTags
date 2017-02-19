﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpawn : MonoBehaviour {

    public GameObject patientUI;
    public GameObject watsonUI;
    public GameObject notepadUI;
    public GameObject alertsUI;
    public GameObject cameraUI;
    public GameObject qrReader;
    public GameObject ARCamera;
    public Camera Hololens;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void destroyUI()
    {
        GameObject[] UIobjects = GameObject.FindGameObjectsWithTag("UserInterface");
        foreach(GameObject destroyee in UIobjects)
        {
            Destroy(destroyee);
        }
    }
    public void spawnQRReader()
    {
        destroyUI();
        Instantiate(qrReader);
        Instantiate(ARCamera);
    }

    public void spawnPatientUI()
    {
        destroyUI();
        GameObject temp = Instantiate(patientUI);
        temp.GetComponent<Canvas>().worldCamera = Hololens;
    }

    public void spawnWatsonUI()
    {
        destroyUI();
        GameObject temp = Instantiate(watsonUI);
        temp.GetComponent<Canvas>().worldCamera = Hololens;
    }

    public void spawnNotepadUI()
    {
        destroyUI();
        Instantiate(notepadUI);
    }

    public void spawnAlertsUI()
    {
        destroyUI();
        Instantiate(alertsUI);
    }

    public void spawnCameraUI()
    {
        destroyUI();
        Instantiate(cameraUI);
    }
}