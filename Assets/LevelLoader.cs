using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public string levelToLoad = "Preloader";
    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void Start()
    {
        Invoke("LoadLevel", 1);
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.touchCount >= 1)
        //{
        //    LoadLevel();
        //}
    }

    public void LoadLevel()
    {

        StartCoroutine(LoadAsynchronously(levelToLoad));

    }

    IEnumerator LoadAsynchronously(string sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while(operation.isDone == false)
        {

            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            Debug.Log("Time : "+Time.timeSinceLevelLoad);
            Debug.Log(operation.progress);
            yield return null;
        }
    }
}
