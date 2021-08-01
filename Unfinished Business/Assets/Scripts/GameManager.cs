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

    public static GameManager instance { get; private set; }

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

    private void OnSceneLoaded(Scene aScene, LoadSceneMode aMode)

    {
        transitioner = GameObject.FindGameObjectWithTag("SceneTransitioner").GetComponent<Animator>();
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

    public void GameOver (VoidCallback callback = null)
    {
        LevelAtIndex(SceneManager.GetActiveScene().buildIndex, callback);
    }

    public void LevelAtIndex (int index, VoidCallback callback = null)
    {
        StartCoroutine(DoSceneTransition(index, callback));
    }

    public void NextLevel (VoidCallback callback = null)
    {
        LevelAtIndex(SceneManager.GetActiveScene().buildIndex + 1, callback);
    }

    public void PreviousLevel (VoidCallback callback = null)
    {
        LevelAtIndex(SceneManager.GetActiveScene().buildIndex - 1, callback);
    }
}
