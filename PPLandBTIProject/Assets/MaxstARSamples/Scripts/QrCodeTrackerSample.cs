/*==============================================================================
Copyright 2017 Maxst, Inc. All Rights Reserved.
==============================================================================*/

using UnityEngine;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using maxstAR;

public class QrCodeTrackerSample : ARBehaviour
{	
	#region EDITING_SITE(1)
	[SerializeField]
	private string[] ObjectNames;
	private bool choice;
    
    [TextArea(3, 1000)]
	public string[] History;
    [TextArea(3,1000)]
	public string[] Sejarah;
	
	public AudioSource[] HistoryNarator;
	public AudioSource[] SejarahNarator;
	
	public Text nameText;
	public Text historyText;
	#endregion
	
    private CameraBackgroundBehaviour cameraBackgroundBehaviour = null;

    private string defaultSearchingWords = "[DEFUALT]";
    private Dictionary<string, List<QrCodeTrackableBehaviour>> QrCodeTrackablesMap =
        new Dictionary<string, List<QrCodeTrackableBehaviour>>();

    void Awake()
    {
		Init();

        cameraBackgroundBehaviour = FindObjectOfType<CameraBackgroundBehaviour>();
        if (cameraBackgroundBehaviour == null)
        {
            Debug.LogError("Can't find CameraBackgroundBehaviour.");
            return;
        }
    }

	void Start()
	{
		PlayerPrefs.SetString("Choose", "");//default state
		choice = false;
		
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        QrCodeTrackablesMap.Clear();
        QrCodeTrackableBehaviour[] QrCodeTrackables = FindObjectsOfType<QrCodeTrackableBehaviour>();

        if (QrCodeTrackables.Length > 0)
        {
            if (QrCodeTrackables[0].QrCodeSearchingWords.Length < 1)
            {
                List<QrCodeTrackableBehaviour> qrCodeList = new List<QrCodeTrackableBehaviour>();

                qrCodeList.Add(QrCodeTrackables[0]);
                QrCodeTrackablesMap.Add(defaultSearchingWords, qrCodeList);
            }
        }

        foreach (var trackable in QrCodeTrackables)
        {
            string key = trackable.QrCodeSearchingWords;

            if (key.Length < 1) key = defaultSearchingWords;

            if (QrCodeTrackablesMap.ContainsKey(key))
            {
                bool isNew = true;

                foreach (var QrCodeTrackableList in QrCodeTrackablesMap[key])
                {
                    if (trackable.name.Equals(QrCodeTrackableList.name))
                    {
                        isNew = false;
                        break;
                    }
                }

                if (isNew) QrCodeTrackablesMap[defaultSearchingWords].Add(trackable);
            }
            else
            {
                List<QrCodeTrackableBehaviour> qrCodeList = new List<QrCodeTrackableBehaviour>();

                qrCodeList.Add(trackable);
                QrCodeTrackablesMap.Add(key, qrCodeList);
            }

            
			Debug.Log("Trackable add: " + trackable.TrackableName);
        }

        TrackerManager.GetInstance().StartTracker(TrackerManager.TRACKER_TYPE_QR_TRACKER);
        AddTrackerData();
		
		StartCamera();

		// For see through smart glass setting
		if (ConfigurationScriptableObject.GetInstance().WearableType == WearableCalibration.WearableType.OpticalSeeThrough)
		{
			WearableManager.GetInstance().GetDeviceController().SetStereoMode(true);

			CameraBackgroundBehaviour cameraBackground = FindObjectOfType<CameraBackgroundBehaviour>();
			cameraBackground.gameObject.SetActive(false);

			WearableManager.GetInstance().GetCalibration().CreateWearableEye(Camera.main.transform);

			// BT-300 screen is splited in half size, but R-7 screen is doubled.
			if (WearableManager.GetInstance().GetDeviceController().IsSideBySideType() == true)
			{
				// Do something here. For example resize gui to fit ratio
			}
		}
	}

	private void AddTrackerData()
    {
	}

	private void DisableAllTrackables()
    {
        foreach (var key in QrCodeTrackablesMap.Keys)
        {
            foreach (var trackable in QrCodeTrackablesMap[key])
            {
                trackable.OnTrackFail();
				
				QrCodeOffTrigger();
            }
		}
	}

	void Update()
	{
		DisableAllTrackables();
		
		
		TrackingState state = TrackerManager.GetInstance().UpdateTrackingState();

        if (state == null)
        {
            return;
        }

        cameraBackgroundBehaviour.UpdateCameraBackgroundImage(state);

        TrackingResult trackingResult = state.GetTrackingResult();

        for (int i = 0; i < trackingResult.GetCount(); i++)
		{
			Trackable trackable = trackingResult.GetTrackable(i);
            
			//Debug.Log("Trackable add: " + trackable.GetName());

            bool isNotFound = true;

            
			foreach (var key in QrCodeTrackablesMap.Keys)
            {
                if (key.Length < 1) continue;
				
                
				if (trackable.GetName().Contains(key))
                {
                    foreach (var qrCodeTrackable in QrCodeTrackablesMap[key])
                    {
                        qrCodeTrackable.OnTrackSuccess(
                            "", trackable.GetName(), trackable.GetPose());
							
						QrCodeOnTrigger(trackable.GetName());
                    }
						
					isNotFound = false;        

					break;
                }
								
            }
			
		

            if (isNotFound && QrCodeTrackablesMap.ContainsKey(defaultSearchingWords))
            {
                foreach (var qrCodeTrackable in QrCodeTrackablesMap[defaultSearchingWords])
                {
                    qrCodeTrackable.OnTrackSuccess(
                        "", trackable.GetName(), trackable.GetPose());
                }
            }
		}
	}

	void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			TrackerManager.GetInstance().StopTracker();
			StopCamera();
		}
		else
		{
			StartCamera();
			TrackerManager.GetInstance().StartTracker(TrackerManager.TRACKER_TYPE_QR_TRACKER);
		}
	}
	

	
	
	void OnDestroy()
    {
        QrCodeTrackablesMap.Clear();
		TrackerManager.GetInstance().StopTracker();
		TrackerManager.GetInstance().DestroyTracker();
		StopCamera();

	}
	
	
	#region EDITING_SITE(2)
	void QrCodeOnTrigger(string name){
		
		int ArrayLength = ObjectNames.Length;
		
		for(int i = 0; i < ArrayLength; i++){
			
			if(ObjectNames[i].Contains(name)){
		
				PlayerPrefs.SetString("Choose", name);
				//Debug.Log("Trackable detected : " + name);

                nameText.text = ObjectNames[i];
				
				if(choice == true){
					historyText.text = History[i];
				}else{
					historyText.text = Sejarah[i];
				}
				
			} 
		}
	}
	
	void QrCodeOffTrigger(){
		
		PlayerPrefs.SetString("Choose", "");
		//Debug.Log("QR Code Lost...");
		
		nameText.text = "";
		historyText.text = "";
	}
	
	public void AudioEnglish(){
		string name = PlayerPrefs.GetString("Choose");
		
		for (int i = 0; i < HistoryNarator.Length; i++)
		{
			if (HistoryNarator[i].gameObject.name.Contains(name)){
				choice = true;
				HistoryNarator[i].GetComponent<AudioSource>().Play();
				Debug.Log("English Narator for = " + name + "has been played");
				
			}
		}
		
	}
	
	public void AudioIndonesia(){
		string name = PlayerPrefs.GetString("Choose");
		
		for (int i = 0; i < SejarahNarator.Length; i++)
		{
			if (SejarahNarator[i].gameObject.name.Contains(name)){
				choice = false;
				SejarahNarator[i].GetComponent<AudioSource>().Play();
				Debug.Log("Indonesian Narator for = " + name + "has been played");
				
			}
		}
	}
	#endregion
}