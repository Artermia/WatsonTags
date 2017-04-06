using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class WatsonScript : MonoBehaviour {

    static string data;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startSearch()
    {
        //BroadcastMessage("search");
        GameObject watsonHandler = GameObject.FindGameObjectWithTag("WatsonHandler");
        watsonHandler.GetComponent<WatsonScript>().search();
    }

    public void search()
    {

        GameObject[] inputs = GameObject.FindGameObjectsWithTag("WatsonInput");
        string symptoms = "symptoms";
        bool default_input = true;
        foreach(GameObject input in inputs)
        {
            if(input.GetComponent<Dropdown>().value != 0)
            {
                default_input = false;
                symptoms += "%20" + input.GetComponent<Dropdown>().options[input.GetComponent<Dropdown>().value].text;
            }
        }

        string url;
        if (default_input)
        {
            //url = "https://eb3c57ce-b1fe-4137-b396-76d814189820:kcP3GMJwJ86E@gateway.watsonplatform.net/retrieve-and-rank/api/v1/solr_clusters/scf6b5474f_5fb7_46b1_bf67_54bb30677ab3/solr/demo-collection/select?q=what%20is%20th%20%20treatment%20for%20blood%20clot?&wt=text";
            url = "https://www.google.com/";
        }
        else
        {
            url = "https://eb3c57ce-b1fe-4137-b396-76d814189820:kcP3GMJwJ86E@gateway.watsonplatform.net/retrieve-and-rank/api/v1/solr_clusters/scf6b5474f_5fb7_46b1_bf67_54bb30677ab3/solr/demo-collection/select?q=" + symptoms + "&wt=text";
            Debug.Log("URL: " + url);
        }
        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        if (www.error == null)
        {
            data = www.text;
            Debug.Log("WWW ok!: " + www.text);
            displayData();
        }
        else
        {
            Debug.Log("WWW error: " + www.error);
        }
    }

    void displayData()
    {
        Stream s = GenerateStreamFromString(data);

        XmlReader reader = XmlReader.Create(s);

        reader.ReadToFollowing("arr");
        reader.ReadToDescendant("str");
        string title = reader.ReadElementContentAsString();
        //reader.ReadToNextSibling("str");
        string text = reader.ReadElementContentAsString();

        Debug.Log("Title: " + title);
        Debug.Log("Text: " + text);

        GameObject watsonOutput = GameObject.FindGameObjectWithTag("Watson");

        watsonOutput.GetComponent<Text>().text = text;

    }

    public static Stream GenerateStreamFromString(string s)
    {
        MemoryStream stream = new System.IO.MemoryStream();
        StreamWriter writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;

    }

}
