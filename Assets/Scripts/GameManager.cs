using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public int money = 0;
    
    public enum GameState
    {
        DayGoing,
        DayEnded
    }

    public GameState gameState = GameState.DayGoing;

    public GameObject pauseMenu;

    public bool tutorial = false;
    // Start is called before the first frame update
    void Start()
    {
        Progress.Load();
        money = Progress.money;
        gameState = GameState.DayGoing;
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            StartCoroutine(Pause());
            
        }
    }

    IEnumerator Pause()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;
        
    }


   
}
