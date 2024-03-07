using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        BeforeDayStarted,
        DayGoing,
        DayEnded
    }

    public GameState gameState = GameState.BeforeDayStarted;
    
    //temp menu
    public GameObject tempMenu;

    public GameObject tempText;
    // Start is called before the first frame update
    void Start()
    {
        Progress.Load();
        money = Progress.money;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.BeforeDayStarted)
        {
            if (Input.anyKey)
            {
                StartGame();
                gameState = GameState.DayGoing;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    
    
    //temporary
    public void StartGame()
    {
        StartCoroutine(TempStart());
    }

    IEnumerator TempStart()
    {
        tempText.SetActive(false);
        float moveTime = 1f;
        float timer = 0f;
        Vector2 startPosition = tempMenu.GetComponent<RectTransform>().anchoredPosition;
        Vector2 endPosition = new Vector2(0, 800);
        while (timer<moveTime)
        {
            tempMenu.GetComponent<RectTransform>().anchoredPosition =
                Vector2.Lerp(startPosition, endPosition, timer/moveTime);
            timer += Time.deltaTime;
            yield return null;
        }
        tempMenu.SetActive(false);
        
    }
}
