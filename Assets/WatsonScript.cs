using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class WatsonScript : MonoBehaviour {

    //variable holding the Watson response
    static string data;
    
    //Unused
    void Start() { }
    void Update() { }

    //Starts on Submit on the Watson UI button
    //Calls search
    public void startSearch()
    {
        //BroadcastMessage("search");
        GameObject watsonHandler = GameObject.FindGameObjectWithTag("WatsonHandler");
        watsonHandler.GetComponent<WatsonScript>().search();
    }

    //Called by startSearch()
    //Takes symptoms from Watson Inputs and places it in the url to request
    //Creates a UnityWebRequest with the url and calls the async function to return Watson's data
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
                string temp = input.GetComponent<Dropdown>().options[input.GetComponent<Dropdown>().value].text;
                temp = temp.Replace(" ", "%20");
                symptoms += "%20" + temp;
            }
        }

        string url;
        if (default_input)
        {
            url = "https://eb3c57ce-b1fe-4137-b396-76d814189820:kcP3GMJwJ86E@gateway.watsonplatform.net/retrieve-and-rank/api/v1/solr_clusters/scf6b5474f_5fb7_46b1_bf67_54bb30677ab3/solr/demo-collection/select?q=what%20is%20th%20%20treatment%20for%20blood%20clot?&wt=text";
        }
        else
        {
            url = "https://eb3c57ce-b1fe-4137-b396-76d814189820:kcP3GMJwJ86E@gateway.watsonplatform.net/retrieve-and-rank/api/v1/solr_clusters/scf6b5474f_5fb7_46b1_bf67_54bb30677ab3/solr/demo-collection/select?q=" + symptoms + "&wt=text";
            Debug.Log("URL: " + url);
        }
        UnityWebRequest request = UnityWebRequest.Get(url);
        StartCoroutine(WaitForRequest(request));
    }

    //Called by search()
    //Async function for the GET request to Watson
    //Stores the result in data variable
    IEnumerator WaitForRequest(UnityWebRequest request)
    {
        yield return request.Send();
        if (request.isError)
        {
            Debug.Log("UnityWebRequest error: " + request.error);
        }
        else
        {
            data = request.downloadHandler.text;
            Debug.Log("UnityWebRequest ok!: " + request.downloadHandler.text);
            displayData();
        }
    }

    //Function for parsing Watson responses and displaying it on the screen
    //Parses the Watson XML response into the title and text
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

    //Function to create a string from a string for use in XmlReader
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