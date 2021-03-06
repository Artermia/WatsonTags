﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Input;
#if WINDOWS_UWP
using Windows.Storage;
using System.IO;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
#endif
using System;
using System.Xml;
using System.Xml.Linq;
using UnityEngine.UI;

public class UISpawn : MonoBehaviour {

    public GameObject patientUI;
    public GameObject watsonUI;
    public GameObject notepadUI;
    public GameObject alertsUI;
    public GameObject newAlert;
    public GameObject cameraUI;
    public GameObject qrReader;
    public GameObject ARCamera;
    public Camera Hololens;
    public GameObject keyboard;

    static private bool centering = false;
    private bool loadedFiles = false;
    static private string patientInfos;
    static private string ID;

    GestureRecognizer recognizer;

    public class patientInfo
    {
        public string NAME {get;set;}
        public string AGE { get; set; }
        public string DOB { get; set; }
        public string SEX { get; set; }
        public string ROOM { get; set; }
        public string NOTES { get; set; }
    }

	// Use this for initialization
	void Start () {
        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Hold);
        recognizer.HoldStartedEvent += Recognizer_HoldStartedEvent;
        recognizer.HoldCompletedEvent += Recognizer_HoldCompletedEvent;
        recognizer.StartCapturingGestures();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("c"))
        {
            Debug.Log("Centering UI");
            centerUI();
        }
        if (centering)
        {
            centerUI();
        }
	}

    private void Recognizer_HoldCompletedEvent(InteractionSourceKind source, Ray ray)
    {
        centering = false;
    }

    private void Recognizer_HoldStartedEvent(InteractionSourceKind source, Ray ray)
    {
        centering = true;
    }

    /*private void Recognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        Debug.Log("tapped: " + tapCount);
        if(tapCount >= 2)
        {
            Debug.Log("double tap UISpawn");
        }
        // process the event.

    }*/

    public void setID(string input)
    {
        ID = input;
    }

    private patientInfo readXML(string ID)
    {
        patientInfo patient = new patientInfo();
#if WINDOWS_UWP
        if (!loadedFiles){
            Task task = new Task(
            async () =>
            {
            var xmlFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Patients/PatientInfo.xml"));
            Windows.Data.Xml.Dom.XmlDocument xdoc = await Windows.Data.Xml.Dom.XmlDocument.LoadFromFileAsync(xmlFile);
            patientInfos = xdoc.GetXml();
            });
            task.Start();
            task.Wait();
            loadedFiles = true;
        }
        XmlReader reader = XmlReader.Create(new StringReader(patientInfos));
#endif
#if UNITY_EDITOR
        XmlReader reader = XmlReader.Create("Assets/UI Assets/Patients/PatientInfo.xml");
#endif
        while (reader.ReadToFollowing("Patient"))
        {
            XmlReader innerReader = reader.ReadSubtree();
            //innerReader.Read();
            if (innerReader.ReadToDescendant("ID"))
            {
                if(innerReader.ReadElementContentAsString() == ID)
                {
                    innerReader.ReadToFollowing("Name");
                    patient.NAME = innerReader.ReadElementContentAsString();
                    innerReader.ReadToFollowing("Age");
                    patient.AGE = innerReader.ReadElementContentAsString();
                    innerReader.ReadToFollowing("DOB");
                    patient.DOB = innerReader.ReadElementContentAsString();
                    innerReader.ReadToFollowing("Sex");
                    patient.SEX = innerReader.ReadElementContentAsString();
                    innerReader.ReadToFollowing("Room");
                    patient.ROOM = innerReader.ReadElementContentAsString();
                    innerReader.ReadToFollowing("Notes");
                    patient.NOTES = innerReader.ReadElementContentAsString();
                    return patient;
                }
                else
                {
                    //Debug.Log("ID not found");
                }
            }
            else
            {
                Debug.Log("Poorly formated XML file");
            }
        }
        Debug.Log("ID not found");
        return patient;

    }

    private void updateHeader(ref Transform headerBar, string ID)
    {
        patientInfo patient = readXML(ID);
        headerBar.FindChild("Name").GetComponent<Text>().text = "Name: " + patient.NAME;
        headerBar.FindChild("Age").GetComponent<Text>().text = "Age: " + patient.AGE;
        headerBar.FindChild("DOB").GetComponent<Text>().text = "DOB: " + patient.DOB;
        headerBar.FindChild("Sex").GetComponent<Text>().text = "Sex: " + patient.SEX;
        headerBar.FindChild("Patient ID").GetComponent<Text>().text = "ID: " + ID;
        headerBar.FindChild("Room").GetComponent<Text>().text = "Room: " + patient.ROOM;
    }

    private Transform destroyUI()
    {
        GameObject UIobject = GameObject.FindGameObjectWithTag("UserInterface");
        Transform location = UIobject.transform;
        Destroy(UIobject);
        GameObject[] UIobjects = GameObject.FindGameObjectsWithTag("UserInterface");
        foreach(GameObject destroyee in UIobjects)
        {
            Destroy(destroyee);
        }
        GameObject reminderUI = GameObject.FindGameObjectWithTag("Reminder");
        Destroy(reminderUI);
        GameObject qrButton = GameObject.FindGameObjectWithTag("QRReader");
        Destroy(qrButton);
        return location;
    }
    public void spawnQRReader()
    {
        Transform location = destroyUI();
        GameObject UIobject = Instantiate(qrReader);
        //Instantiate(ARCamera);
        UIobject.transform.position = location.position;
        UIobject.transform.rotation = location.rotation;
    }

    public void spawnPatientUI()
    {
        Transform location = destroyUI();
        GameObject UIobject = Instantiate(patientUI);
        UIobject.transform.position = location.position;
        UIobject.transform.rotation = location.rotation;
        Transform headerBar = UIobject.transform.FindChild("Header Bar");
        updateHeader(ref headerBar, ID);
    }

    public void spawnWatsonUI()
    {
        Transform location = destroyUI();
        GameObject UIobject = Instantiate(watsonUI);
        UIobject.transform.position = location.position;
        UIobject.transform.rotation = location.rotation;
        Transform headerBar = UIobject.transform.FindChild("Header Bar");
        updateHeader(ref headerBar, ID);
    }

    public void spawnNotepadUI()
    {
        Transform location = destroyUI();
        GameObject UIobject = Instantiate(notepadUI);
        UIobject.transform.position = location.position;
        UIobject.transform.rotation = location.rotation;
        Transform headerBar = UIobject.transform.FindChild("Header Bar");
        updateHeader(ref headerBar, ID);

        Transform textBox = UIobject.transform.FindChild("Text");
        patientInfo patient = readXML(ID);
        textBox.GetComponent<Text>().text = patient.NOTES;


    }

    public void spawnAlertsUI()
    {
        Transform location = destroyUI();
        GameObject UIobject = Instantiate(alertsUI);
        UIobject.transform.position = location.position;
        UIobject.transform.rotation = location.rotation;
        Transform headerBar = UIobject.transform.FindChild("Header Bar");
        updateHeader(ref headerBar, ID);

    }

    public void spawnNewAlert()
    {
        Transform location = GameObject.FindGameObjectWithTag("UserInterface").transform;
        GameObject UIobject = Instantiate(newAlert);
        UIobject.transform.position = location.position;
        UIobject.transform.rotation = location.rotation;

        
    }

    public void spawnKeyboard()
    {
        Transform location = GameObject.FindGameObjectWithTag("UserInterface").transform;
        GameObject inputDevice = Instantiate(keyboard);
        inputDevice.transform.position = location.position;
        inputDevice.transform.rotation = location.rotation;
        inputDevice.GetComponent<Canvas>().sortingOrder = 10;
        inputDevice.SetActive(true);
    }

    public void spawnCameraUI()
    {
        Transform location = destroyUI();
        GameObject UIobject = Instantiate(cameraUI);
        UIobject.transform.position = location.position;
        UIobject.transform.rotation = location.rotation;
        Transform headerBar = UIobject.transform.FindChild("Header Bar");
        updateHeader(ref headerBar, ID);
    }

    public void centerUI()
    {
        GameObject user = GameObject.Find("HoloLensCamera");
        Debug.Log("Hololens position x: " + user.transform.position.x);
        GameObject[] UIobjects = GameObject.FindGameObjectsWithTag("UserInterface");
        foreach (GameObject UIobject in UIobjects)
        {
            UIobject.transform.position = user.transform.position + user.transform.forward * 2;
            UIobject.transform.rotation = user.transform.rotation;
            /*UIobject.transform.SetParent(user.transform);
            UIobject.transform.localPosition = new Vector3(0, 0, 2);
            UIobject.transform.SetParent(null);*/
        }
        GameObject reminderUI = GameObject.FindGameObjectWithTag("Reminder");
        if (reminderUI != null) {
            reminderUI.transform.position = user.transform.position + user.transform.forward * 2;
            reminderUI.transform.rotation = user.transform.rotation;
        }
    }
}
