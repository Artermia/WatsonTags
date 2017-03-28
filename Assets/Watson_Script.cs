using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using IBM.Watson.DeveloperCloud.Services.RetrieveAndRank.v1;
using Vuforia;
//using log4net;
using System.Threading;

using ZXing;
using ZXing.QrCode;
using ZXing.Common;


[AddComponentMenu("System/VuforiaScanner")]
public class Watson_Script: MonoBehaviour
{

    static WatsonResponses responses = new WatsonResponses();
    static float timer = 1;
    static bool waiting = false;

    class WatsonResponses
    {
        public List<string> response = new List<string>();
        public int numResponses = 0;
    }

	void Start()
	{        
		
	}

    void Update()
    {
        if (waiting)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                waiting = false;
                Debug.Log("Displaying Results~~~~~~~~~");
                displayOutput();
            }
        }
    }
		

	private void OnSearch(SearchResponse resp, string data)
	{
        
		if(resp != null)
		{
			Debug.Log("response : " + resp.response);
			if(resp.responseHeader != null)
			{
				Debug.Log ("Retrieve and Ranked : respoense status : "+resp.responseHeader.status);
				if (resp.responseHeader._params != null)
					Debug.Log ("Retrieve and Ranked");
				else
					Debug.Log ("Retrieve and Rank parameter is null !");
			}
			else
			{
				Debug.Log ("Retrieve and Rank response header is null !");
			}

			if (resp.response != null)
			{
				if(resp.response.docs != null)
				{
					if (resp.response.docs.Length == 0)
						Debug.Log ("Retrieve and Rank there are no docs!");
					else {
						Debug.Log("doc : " + resp.response.numFound);
                        responses.numResponses = resp.response.numFound;
                        Debug.Log("Watson Responses~~~~~~~~~~~~~~~~~ " + responses.numResponses);
						foreach (Doc doc in resp.response.docs)
						{
							Debug.Log ("Retrieve and Rank : id : " + doc.id);
							Debug.Log ("Retrieve and Rank : id : " + doc.id);
							if (doc.title != null) {
								if (doc.title.Length == 0)
									Debug.Log ("Retrieve and Rank NO title!");
								else
									Debug.Log ("Retrieve and Rank string: " + doc.title);
							} else {
								Debug.Log ("Retrieve and Rank title is null !");
							}
							if (doc.body != null)
							{
								if (doc.body.Length == 0)
									Debug.Log ("Retrieve and Rank : No body");
                                else { 
										Debug.Log ("Retrieve and Rank : body : " + doc.body);
                                        responses.response.Add(doc.body);
                                    }
							}
							else
							{
								Debug.Log ("Retrieve and Rank : Body is null");
							}
								
						}
					}
				}
				else
				{
					Debug.Log ("Retrieve and Rank: docs is null !");
				}
			}
			else
			{
				Debug.Log ("Retrieve and Rank : reponse is null !");
			}
		}
		else
		{
			Debug.Log ("Retrieve and Rank : Search response is null !");
		}
	}

    public void search()
    {
        RetrieveAndRank test = new RetrieveAndRank();
        responses = new WatsonResponses();
        //		Log.Debug("ExampleRetrieveAndRank", "Attempting to search!");
        Debug.Log("ExampleRetrieveAndRank Attempting to search!");
        string[] fl = { "id", "title", "body" };
        if (!test.Search(OnSearch, "scf6b5474f_5fb7_46b1_bf67_54bb30677ab3", "demo-collection", "What is the treatment for blood clot?", fl, true))
            Debug.Log("ExampleRetrieveAndRank Failed to search!");
        //		Log.Debug("ExampleRetrieveAndRank", "Failed to search!");

        timer = 1;
        waiting = true;
        Debug.Log("~~~~~~~~~~~~~~Waiting~~~~~~~~~~~~~~~~~");
        
    }

    public void displayOutput()
    {
        Debug.Log("Watson Response: " + responses.numResponses);

        GameObject watsonOutput = GameObject.FindGameObjectWithTag("Watson");
        /*for (int i = 0; i < 4; ++i)
        {
            watsonOutput.transform.GetChild(i).GetComponent<Text>().text = responses.response[i];
        }*/
        watsonOutput.GetComponent<Text>().text = responses.response[0];
    }
}