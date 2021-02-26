using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   

    private string name;
    //public string[] Stories;
    //public Text NamesTextbox;
    //public Text DescriptionTextbox;
    
    public GameObject RayCastTarget;
    public GameObject ObjectDescription;

    public GameObject[] Character;
	[SerializeField]
	//public GameObject change;
	public GameObject buttonAction;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void StartScanning(string Name)
    {
		int n = Character.Length;
        
		for(int i = 0; i < n; i++){

            if (Character[i].gameObject.name.Contains(Name))
            {
                Character[i].gameObject.SetActive(true);
                //NamesTextbox.text = Character[i].gameObject.name;
                //DescriptionTextbox.text = Stories[i];
				
            }else{
				
                Character[i].gameObject.SetActive(false);
                //NamesTextbox.text = "";
                //DescriptionTextbox.text = "";
				
            }
        
        }
        
    }
	
	
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
			
			StartScanning(ObjectName);
		}
	}
    


    // Update is called once per frame
    void Update()
    {
		name = PlayerPrefs.GetString("Choose");
                
		UI(name);
    }
	
	public void NextScenes(string sceneName){
		SceneManager.LoadScene(sceneName);
	}
	
}
