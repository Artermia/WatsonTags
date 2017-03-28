using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timeAdjust : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
}
