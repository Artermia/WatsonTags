
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
    public GameObject QRcamera;

	void Start()
	{
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        StartCoroutine(InitializeCamera());
	}

	private IEnumerator InitializeCamera()
	{
		// Waiting a little seem to avoid the Vuforia's crashes.
		yield return new WaitForSeconds(1.25f);

        //var isFrameFormatSet = CameraDevice.Instance.SetFrameFormat(Image.PIXEL_FORMAT.RGB888, true);
        var isFrameFormatSet = CameraDevice.Instance.SetFrameFormat(Vuforia.Image.PIXEL_FORMAT.GRAYSCALE, true);
        Debug.Log(String.Format("FormatSet : {0}", isFrameFormatSet));

		// Force autofocus.
		var isAutoFocus = CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
		if (!isAutoFocus)
		{
			CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_NORMAL);
		}
		Debug.Log(String.Format("AutoFocus : {0}", isAutoFocus));
		cameraInitialized = true;
        Debug.Log("cameraInitialized set to true");
        
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