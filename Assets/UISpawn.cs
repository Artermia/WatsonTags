using System.Collections;
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
        Debug.Log("Destroying UI");
        GameObject[] UIobjects = GameObject.FindGameObjectsWithTag("UserInterface");
        foreach(GameObject destroyee in UIobjects)
        {
            Destroy(destroyee);
        }
        GameObject QRobjects = GameObject.FindGameObjectWithTag("QRReader");
        Destroy(QRobjects);
    }
    public void spawnQRReader()
    {
        destroyUI();
        Instantiate(qrReader);
        //Instantiate(ARCamera);
    }

    public void spawnPatientUI()
    {
        destroyUI();
        Instantiate(patientUI);
    }

    public void spawnWatsonUI()
    {
        destroyUI();
        Instantiate(watsonUI);
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
