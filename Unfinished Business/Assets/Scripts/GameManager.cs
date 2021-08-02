using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void VoidCallback();

public class GameManager : MonoBehaviour
{

    [SerializeField] float transistionTime = 1f;
    
    private Animator transitioner;
    private PlayerHealth playerHealthRef;

    public int Score { get; private set; }
    public int CurrentHighestScore { get; private set; }
    public static GameManager instance { get; private set; }
    public GameOverMenu GOM;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void Update()
    {
        if (playerHealthRef != null)
        {
            Score = playerHealthRef.Money;
            if (Score > CurrentHighestScore)
            {
                CurrentHighestScore = Score;
            }
        }

        if (playerHealthRef.hitAtZero) 
        {
            GOM.Open();
        }
    }

    private void OnSceneLoaded(Scene aScene, LoadSceneMode aMode)

    {
        transitioner = GameObject.FindGameObjectWithTag("SceneTransitioner").GetComponent<Animator>();
        playerHealthRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    private IEnumerator DoSceneTransition (int index, VoidCallback callback = null)
    {
        if (transitioner != null)
        {
            transitioner.SetTrigger("Start");
        }
        
        yield return new WaitForSeconds(transistionTime);
        SceneManager.LoadScene(index);
        if (callback != null)
        {
            callback();
        }
    }

    public void Freeze ()
    {
        Time.timeScale = 0;
    }

    public void Unfreeze ()
    {
        Time.timeScale = 1;
    }

    public void Quit ()
    {
        Application.Quit();
    }

    // Plays a transition and restarts the scene
    public void GameOver (VoidCallback callback)
    {
        LevelAtIndex(SceneManager.GetActiveScene().buildIndex, callback);
    }

    // Plays a transition and loads a specific scene index
    public void LevelAtIndex (int index, VoidCallback callback)
    {
        StartCoroutine(DoSceneTransition(index, callback));
    }

    // Plays a transition and loads the next scene
    public void NextLevel (VoidCallback callback)
    {
        LevelAtIndex(SceneManager.GetActiveScene().buildIndex + 1, callback);
    }

    // Plays a transition and loads the previous scene
    public void PreviousLevel (VoidCallback callback)
    {
        LevelAtIndex(SceneManager.GetActiveScene().buildIndex - 1, callback);
    }

    // Overloads of the above methods that can be used in unity events. 
    // I was unable to use optional parameter syntax here because it 
    // prevented unity events from using the methods.
    public void GameOver()
    {
        LevelAtIndex(SceneManager.GetActiveScene().buildIndex);
    }
    public void LevelAtIndex(int index)
    {
        StartCoroutine(DoSceneTransition(index));
    }
    public void NextLevel()
    {
        LevelAtIndex(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void PreviousLevel()
    {
        LevelAtIndex(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
