using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class GestureHandler : Singleton<GestureHandler> {

    private bool centering = false;

    GestureRecognizer recognizer;

	// Use this for initialization
	void Start () {
        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Hold);
        recognizer.TappedEvent += Recognizer_TappedEvent;
        recognizer.HoldStartedEvent += Recognizer_HoldStartedEvent;
        recognizer.HoldCompletedEvent += Recognizer_HoldCompletedEvent;
        recognizer.StartCapturingGestures();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Recognizer_HoldCompletedEvent(InteractionSourceKind source, Ray ray)
    {
        centering = false;
    }

    private void Recognizer_HoldStartedEvent(InteractionSourceKind source, Ray ray)
    {
        centering = true;
    }

    private void Recognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        Debug.Log("tapped: " + tapCount);
        if (tapCount >= 2)
        {
            Debug.Log("double tap UISpawn");
        }
        // process the event.

    }
}
