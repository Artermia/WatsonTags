using UnityEngine;
using System;
using System.Collections;

using Vuforia;

using System.Threading;

using ZXing;
using ZXing.QrCode;
using ZXing.Common;


[AddComponentMenu("System/VuforiaScanner")]
public class VuforiaScanner : MonoBehaviour
{    
	private bool cameraInitialized;
    public UISpawn mainMenu;
    public GameObject QRcamera;

	private BarcodeReader barCodeReader;

	void Start()
	{        
		barCodeReader = new BarcodeReader();
		StartCoroutine(InitializeCamera());
	}

	private IEnumerator InitializeCamera()
	{
		// Waiting a little seem to avoid the Vuforia's crashes.
		yield return new WaitForSeconds(1.25f);

        //var isFrameFormatSet = CameraDevice.Instance.SetFrameFormat(Image.PIXEL_FORMAT.RGB888, true);
        var isFrameFormatSet = CameraDevice.Instance.SetFrameFormat(Image.PIXEL_FORMAT.GRAYSCALE, true);
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

	private void Update()
	{
        if (Input.GetKeyDown("k"))
        {
            mainMenu.spawnPatientUI();
            GameObject camera = GameObject.FindGameObjectWithTag("AR Camera");
            Destroy(camera);
            Destroy(gameObject);
        }

		if (cameraInitialized)
		{
            
            try
            {
                
				var cameraFeed = CameraDevice.Instance.GetCameraImage(Image.PIXEL_FORMAT.GRAYSCALE);
				if (cameraFeed == null)
				{
                    Debug.Log("No camera device");
					return;
				}
                Debug.Log("Trying to scan");
                var data = barCodeReader.Decode(cameraFeed.Pixels, cameraFeed.BufferWidth, cameraFeed.BufferHeight, RGBLuminanceSource.BitmapFormat.Gray8);
				if (data != null)
				{
					// QRCode detected.
					Debug.Log(data.Text);
                    mainMenu.spawnPatientUI();
                    GameObject camera = GameObject.FindGameObjectWithTag("AR Camera");
                    Destroy(camera);
                    Destroy(gameObject);

                }
				else
				{
					Debug.Log("No QR code detected!");
				}
			}
			catch (Exception e)
			{
                Debug.Log("failing to scan");
                Debug.LogError(e.Message);
			}
		}
	}
}