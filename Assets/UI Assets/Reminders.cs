using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reminders : MonoBehaviour {

    static List<Reminder> alerts = new List<Reminder>();

    public GameObject sampleAlert;
    public GameObject popupUI;

    public class Reminder
    {
        public string message;
        public string hour;
        public string min;
        public float timeRemaining;
    }

	// Use this for initialization
	void Start ()
    {
        /*alerts = new List<Reminder>();
        Reminder temp = new Reminder();
        temp.message = "hi";
        temp.hour = "1";
        temp.min = "12";
        temp.timeRemaining = 10;
        alerts.Add(temp);
        Debug.Log("start alerts size " + alerts.Count);*/
	}

    // Update is called once per frame
    void Update()
    {
        List<Reminder> expiredAlerts = new List<Reminder>();
        for(int i=0; i < alerts.Count; ++i)
        {
            alerts[i].timeRemaining -= Time.deltaTime;
            if(alerts[i].timeRemaining <= 0)
            {
                spawnPopup(alerts[i]);
                expiredAlerts.Add(alerts[i]);
            } 
        }
        foreach(Reminder toDelete in expiredAlerts)
        {
            alerts.Remove(toDelete);
        }
        /*foreach (Reminder alert in alerts)
        {
            alert.timeRemaining -= Time.deltaTime;
            //Debug.Log("time remaining: " + alert.timeRemaining);
            if (alert.timeRemaining <= 0)
            {
                Debug.Log("Reminder:" + alert.message);
                spawnPopup(alert);
                alerts.Remove(alert);
            }
        }*/
    }

    //public void addReminder(string message, string hour, string minute)
    public void addReminder()
    {
        Reminder newReminder = new Reminder();
        GameObject reminderUI = GameObject.FindGameObjectWithTag("Reminder");
        string message = reminderUI.transform.FindChild("Message").FindChild("Text").GetComponent<Text>().text;
        string hour = reminderUI.transform.FindChild("Hour").GetComponent<Text>().text;
        string minute = reminderUI.transform.FindChild("Min").GetComponent<Text>().text;
        newReminder.message = message;
        newReminder.hour = hour;
        newReminder.min = minute;
        int currentHour = DateTime.Now.Hour;
        int currentMin = DateTime.Now.Minute;
        int currentSec = DateTime.Now.Second;
        int secRemaining = ((int.Parse(hour) - currentHour) * 60 * 60) + ((int.Parse(minute) - currentMin) * 60) - currentSec;

        if(secRemaining < 0)
        {
            secRemaining += (60 * 24 * 60);
        }

        newReminder.timeRemaining = secRemaining;
        Debug.Log(newReminder.message);
        
        Debug.Log("Seconds remaining: " + secRemaining);
        alerts.Add(newReminder);
        Debug.Log(alerts.Count);

        Destroy(reminderUI);
    }

    private void spawnPopup(Reminder input)
    {
        GameObject user = GameObject.Find("HoloLensCamera");
        GameObject popup = Instantiate(popupUI);
        popup.transform.FindChild("Message").GetComponent<Text>().text = input.message;
        popup.transform.position = user.transform.position + user.transform.forward * 2;
        popup.transform.rotation = user.transform.rotation;
    }

    public void addHour()
    {
        string value = this.transform.FindChild("Hour").GetComponent<Text>().text;
        int numeric = int.Parse(value);
        numeric = (numeric + 1) % 24;
        this.transform.FindChild("Hour").GetComponent<Text>().text = numeric.ToString();
    }

    public void minusHour()
    {
        string value = this.transform.FindChild("Hour").GetComponent<Text>().text;
        int numeric = int.Parse(value);
        numeric = (24 + numeric - 1) % 24;
        this.transform.FindChild("Hour").GetComponent<Text>().text = numeric.ToString();
    }

    public void addMin()
    {
        string value = this.transform.FindChild("Min").GetComponent<Text>().text;
        int numeric = int.Parse(value);
        numeric = (numeric + 1) % 60;
        if(numeric < 10)
        {
            value = "0" + numeric.ToString();
        }
        else
        {
            value = numeric.ToString();
        }
        this.transform.FindChild("Min").GetComponent<Text>().text = value;
    }

    public void minusMin()
    {
        string value = this.transform.FindChild("Min").GetComponent<Text>().text;
        int numeric = int.Parse(value);
        numeric = (numeric - 1 + 60) % 60;
        if (numeric < 10)
        {
            value = "0" + numeric.ToString();
        }
        else
        {
            value = numeric.ToString();
        }
        this.transform.FindChild("Min").GetComponent<Text>().text = value;
    }

    public void destroyReminder()
    {
        GameObject reminderUI = GameObject.FindGameObjectWithTag("Reminder");
        Destroy(reminderUI);
    }

    public void destroyPopup()
    {
        GameObject popupUI = GameObject.FindGameObjectWithTag("Popup");
        Destroy(popupUI);
    }

    public void displayReminders()
    {
        GameObject alertsUI = GameObject.FindGameObjectWithTag("CurrentAlerts");
        int numChildren = alertsUI.transform.childCount;
        for(int i = 0; i < numChildren; ++i)
        {
            Destroy(alertsUI.transform.GetChild(i).gameObject);
        }
        Debug.Log("alerts size " + alerts.Count);
        foreach (Reminder alert in alerts)
        {
            GameObject newAlert = Instantiate(sampleAlert);
            newAlert.transform.FindChild("Hour").GetComponent<Text>().text = alert.hour;
            newAlert.transform.FindChild("Minute").GetComponent<Text>().text = alert.min;
            newAlert.transform.FindChild("Message").GetComponent<Text>().text = alert.message;
            newAlert.transform.SetParent(alertsUI.transform);
            newAlert.transform.localPosition = new Vector3(newAlert.transform.position.x, newAlert.transform.position.y, 0);
            newAlert.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void clearAll()
    {
        alerts = new List<Reminder>();
        displayReminders();
    }
}
