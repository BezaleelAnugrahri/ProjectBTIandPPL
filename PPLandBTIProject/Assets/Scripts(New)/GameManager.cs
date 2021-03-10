using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ListObject;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private string trackerName;

    [Header("Game Object Caller")]
    public GameObject RayCastTarget;
    public GameObject ObjectDescription;
    public GameObject buttonAction;
    public GameObject ParentQRCode;

    [SerializeField]
    private ObjectDatas[] listObjectDatas;
    
    public GameObject[] prefabsObject;

    [Header("UI elements")]
    public Text objectNamePlace;
    public Text objectDescriptionPlace;

    private string descInd;
    private string descEng;

    private string lang;

    void Awake()
    {
        listObjectDatas = Resources.LoadAll("Datas", typeof(ObjectDatas)).Cast<ObjectDatas>().ToArray();
    }


    void Update()
    {

        OnTrackingLost();

        trackerName = PlayerPrefs.GetString("Choose");
        //Debug.Log("Choosing = " + name);
        StartScanning(trackerName);
        UI(trackerName);

    }
	

    void StartScanning(string name)
    {

        for (int i = 0; i < listObjectDatas.Length; i++)
        {
            
            if (listObjectDatas[i].qrCodeARTrackerName == name)
            {
                
                //call all datas from asset inside array
                //Debug.Log("QRCode Detected = " + listObjectDatas[i].qrCodeARTrackerName);
                descInd = listObjectDatas[i].objectDescInd;
                descEng = listObjectDatas[i].objectDescEng;

                objectNamePlace.text = listObjectDatas[i].objectARName;

                LanguageChooser(lang);
                
            }
            

            
        }

    }

    void OnTrackingLost()
    {
        
        //tell the user if Qrcode isn't on the list
        objectDescriptionPlace.text = "Sorry this QRCode isn't on the Database";

        objectNamePlace.text = "";

        descInd = null;
        descEng = null;

        /*for (int i = 0; i < prefabsObject.Length; i++)
        {
            prefabsObject[i].SetActive(false);
        }*/

    }

    #region UI_Sector

    #region Translate

    public void TranslationMode(string language)
    {
        lang = language;
    }

	void LanguageChooser(string lang)
    {
        
        //Choosing the Language 
                switch (lang)
                {
                    case "ENG":
                        {
                            objectDescriptionPlace.text = descEng;
                            break;
                        }
                    case "IND":
                        {
                            objectDescriptionPlace.text = descInd;
                            break;
                        }
                    default:
                        {
                            objectDescriptionPlace.text = descInd;
                            break;
                        }
                }

    }
    
    #endregion


    void UI(string ObjectName)
	{
		if(ObjectName == ""){
			RayCastTarget.gameObject.SetActive(true);
            ObjectDescription.gameObject.SetActive(false);
			//change.gameObject.SetActive(true);
			buttonAction.gameObject.SetActive(false);
			
		}else{
			RayCastTarget.gameObject.SetActive(false);
            ObjectDescription.gameObject.SetActive(true);	
			//change.gameObject.SetActive(false);	
			buttonAction.gameObject.SetActive(true);

		}
	}
    

	public void NextScenes(string sceneName){
		SceneManager.LoadScene(sceneName);
	}
    #endregion
}
