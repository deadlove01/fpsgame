using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public GameObject loadingScreen;
    public Slider slider;
    public Text loadingText;

    public void LoadLevel(string levelName)
    {
       
        loadingScreen.SetActive(true);
        loadingText.gameObject.SetActive(true);
        slider.gameObject.SetActive(true);

        StartCoroutine(LoadAsync(levelName));
       
    }

    IEnumerator LoadAsync(string levelName)
    {
       // yield return new WaitForSeconds(2);
        print("loading screen: "+loadingScreen.activeInHierarchy);
        var opeation = SceneManager.LoadSceneAsync(levelName);
        opeation.allowSceneActivation = false;
        while (!opeation.isDone)
        {
            float progress = Mathf.Clamp01(opeation.progress / 0.9f);
            slider.value = progress;
        
            loadingText.text = progress * 100f + "%";
            if (progress >= 0.9f)
            {
               
                loadingText.text += ", Press any key to continue!";
                //if (Input.GetKeyDown(KeyCode.Space))
                //{
                opeation.allowSceneActivation = true;
                //}
            }
            yield return null;

        }

        loadingScreen.SetActive(false);
        loadingText.gameObject.SetActive(false);
        slider.gameObject.SetActive(false);
    }
}
