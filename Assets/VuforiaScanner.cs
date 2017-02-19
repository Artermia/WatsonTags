using UnityEngine;
using UnityEngine.UI;

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

	Texture2D two_texture;

	void Start()
	{        
		barCodeReader = new BarcodeReader();
		StartCoroutine(InitializeCamera());
		//texture = new Texture2D(128, 128);
		//renderer = GetComponent<Renderer>();
		//renderer.material.mainTexture = texture;

		//GameObject imageFrame = GameObject.Find ("Captured Image");
		//imageFrame.GetComponent<Sprite> ();
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
                
				var currentImage = CameraDevice.Instance.GetCameraImage(Vuforia.Image.PIXEL_FORMAT.GRAYSCALE);
				if (currentImage == null)
				{
                    Debug.Log("No camera device");
					return;
				}
                Debug.Log("Trying to scan");


				two_texture = new Texture2D(128, 128, TextureFormat.Alpha8, true);
				RawImage raw_image = GameObject.FindGameObjectWithTag("V Image").GetComponent<RawImage>();

				currentImage.CopyToTexture(two_texture);
				raw_image.texture = two_texture;
				two_texture.Apply();

				//raw_image.GetComponent<RectTransform>().sizeDelta = new Vector2(currentImage.Width, currentImage.Height);

                var data = barCodeReader.Decode(currentImage.Pixels, currentImage.BufferWidth, currentImage.BufferHeight, RGBLuminanceSource.BitmapFormat.Gray8);
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