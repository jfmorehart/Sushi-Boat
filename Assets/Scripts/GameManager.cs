using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

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
        DayEnded,
        Dead
    }

    public GameState gameState = GameState.DayGoing;

    public GameObject pauseMenu;

    public bool tutorial = false;
	public bool boss = false;
    

    public static bool paused;

    public Bell bell;

    
    
    //boss stuff
    public int BossLevelMaxHP = 3;
    public int BossLevelCurrentHP = 3;

    public GameObject loseScreen;
    public GameObject health;
    public YachtRock yr;
    public SpriteRenderer boatCracks;
    public Sprite crack1;
    public Sprite crack2;
    public Sprite crack3;
    public GameObject tentacle1;
    public GameObject tentacle2;
    public GameObject tentacle3;

    public GameObject hpBar;

    public Sprite damaged;
	// Start is called before the first frame update
	void Start()
    {
        Progress.Load();
        money = Progress.money;
        gameState = GameState.DayGoing;
        if (SceneManager.GetActiveScene().buildIndex == 6)
        {  //boss scene 
            boss = true;
        }
        else {
            boss = false;
	    }

        bell = GameObject.Find("Bell").GetComponent<Bell>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused) return;
            paused = true;
            StartCoroutine(Pause());
            
        }
    }

    IEnumerator Pause()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForSeconds(0.05f);
		pauseMenu.SetActive(true);
		Time.timeScale = 0;
        
    }


    public void Die()
    {
        boatCracks.sprite = crack3;
        gameState = GameState.Dead;
        StartCoroutine(Death());
        
    }

    IEnumerator Death()
    {
        tentacle1.SetActive(true);
        tentacle2.SetActive(true);
        tentacle3.SetActive(true);
        tentacle1.transform.DOMoveX(18f,0.8f);
        tentacle2.transform.DOMoveX(-19f,0.8f);
        tentacle3.transform.DOMoveX(-17f,0.8f);
        boatCracks.sprite = crack3;
        StartCoroutine(BoatShake());
        yield return new WaitForSeconds(2.5f);
        loseScreen.SetActive(true);
    }

    public void TakeDamage()
    {
        BossLevelCurrentHP -= 1;
        hpBar.transform.GetChild(BossLevelCurrentHP).GetComponent<Image>().sprite = damaged;
        if (BossLevelCurrentHP == 2)
        {
            boatCracks.sprite = crack1;
            StartCoroutine(BoatShake());
        }
        else if (BossLevelCurrentHP == 1)
        {
            boatCracks.sprite = crack2;
            StartCoroutine(BoatShake());
        }
        else if (boss && BossLevelCurrentHP <= 0)
        {
            Die();
            
        }
        

    }

    IEnumerator BoatShake()
    {
        float ogf = 0.7f;
        float ogamp = 1f;
        yr.freq = 4f;
        yr.amp = 1.5f;
        Camera.main.GetComponent<CameraShake>().TriggerShake();
        yield return new WaitForSeconds(1.5f);
        yr.freq = ogf;
        yr.amp = ogamp;


    }

   
}
