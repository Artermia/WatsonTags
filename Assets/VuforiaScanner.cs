using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Vuforia;
using System.Threading;

//Handles the QR Reader via image targets through Vuforia
[AddComponentMenu("System/VuforiaScanner")]
public class VuforiaScanner : ImageTargetAbstractBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour mTrackableBehaviour;
    private bool cameraInitialized;
    public UISpawn mainMenu;

	void Start()
	{
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
	}

    //Finds image targets and tracks their states
    //If the QRReader object exists, set the ID to the QR code of an image target and spawn the patient UI
    public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            if (GameObject.FindGameObjectWithTag("QRReader") != null)
            {
                Debug.Log(this.ImageTarget.Name);
                mainMenu.setID(this.ImageTarget.Name);
                mainMenu.spawnPatientUI();
            }
        }
        else
        {
            //target is lost
        }
    }

    private void Update()
	{
	}
}