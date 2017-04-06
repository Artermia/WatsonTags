using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Vuforia;
using System.Threading;

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