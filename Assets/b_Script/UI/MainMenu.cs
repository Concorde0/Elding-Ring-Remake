using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
    private Button newGameButton;
    private Button continueButton;
    private Button exitButton;
    
    [SerializeField] private GameObject faderPrefab;
    private SceneFader faderInstance;
    
    private PlayableDirector director;

    private void Awake()
    {
        newGameButton = transform.GetChild(1).GetComponent<Button>();
        continueButton = transform.GetChild(2).GetComponent<Button>();
        exitButton = transform.GetChild(3).GetComponent<Button>();

        newGameButton.onClick.AddListener(PlayTimeLine);
        continueButton.onClick.AddListener(ContinueGame);
        exitButton.onClick.AddListener(ExitGame);

        director = FindObjectOfType<PlayableDirector>();
        director.stopped += NewGame;

    }
    
    private void PlayTimeLine()
    {
        if (director != null)
        {
            director.Play();
        }
    }

    private void NewGame(PlayableDirector obj)
    {
        PlayerPrefs.DeleteAll();
        StartCoroutine(LoadSceneAsync("SampleScene")); 
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        if (faderInstance == null && faderPrefab != null)
        {
            GameObject go = Instantiate(faderPrefab);
            DontDestroyOnLoad(go);
            faderInstance = go.GetComponent<SceneFader>();
        }

        if (faderInstance != null)
        {
            yield return faderInstance.FadeOut(faderInstance.fadeoutDuration);
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            yield return null;
        }

        if (faderInstance != null)
        {
            yield return faderInstance.FadeIn(faderInstance.fadeDuration);
            
            Destroy(faderInstance.gameObject);
            faderInstance = null;
        }
    }

    private void ContinueGame()
    {
        
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}

