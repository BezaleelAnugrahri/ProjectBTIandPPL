using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject Menu1;
    public GameObject Menu2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextScenes(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void PlayAnimation(bool menuBool)
    {
        //int boolean = PlayerPrefs.GetInt("MainMenuBool");

        if (menuBool == false)
        {
            Menu1.GetComponent<Animator>().SetBool("showMain", false);
            Menu2.GetComponent<Animator>().SetBool("showList", true);

            //PlayerPrefs.SetInt("MainMenuBool", 0);
        }
        else
        {
            Menu1.GetComponent<Animator>().SetBool("showMain", true);
            Menu2.GetComponent<Animator>().SetBool("showList", false);

            //PlayerPrefs.SetInt("MainMenuBool", 1);
        }
    }

    public void exitApk()
    {
        Application.Quit();
    }

}
