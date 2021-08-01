using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private IEnumerator DoSceneTransition (int index)
    {
        if (transitioner != null)
        {
            transitioner.SetTrigger("Start");
        }
        
        yield return new WaitForSeconds(transistionTime);
        SceneManager.LoadScene(index);
    }

    public void GameOver ()
    {
        StartCoroutine(DoSceneTransition(SceneManager.GetActiveScene().buildIndex));
    }

    public void LevelAtIndex (int index)
    {
        StartCoroutine(DoSceneTransition(index));
    }

    public void NextLevel ()
    {
        StartCoroutine(DoSceneTransition(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void PreviousLevel ()
    {
        StartCoroutine(DoSceneTransition(SceneManager.GetActiveScene().buildIndex - 1));
    }
}
