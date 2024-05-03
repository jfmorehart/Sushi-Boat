using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    [TextArea(6,100)]
    public string[] tutorialText;
    public GameObject[] tutorialImages;
    public int textIndex;
    public int imageIndex;

    public GameObject textUI;

    public Station cooler;
    public Station cuttingBoard;
    public Station riceDrawer;
    public Station riceCooker;
    public Station prepTable;
    public GameObject customer;
    public GameObject tutorialBoat;

    public Item cookedRice;
    public Item cutTuna;
    public Item tunaNigiri;

    public Hook hook;


    private Vector3 ogScale;

    public void SkipTutorial()
    {
        Progress.BeatTutorial();
        SceneManager.LoadScene("MainMenu");
    }
    // Start is called before the first frame update
    void Start()
    {
        ogScale = textUI.transform.GetChild(1).GetComponent<RectTransform>().localScale;
        StartCoroutine(Tutorial());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowText()
    {
        textUI.SetActive(true);
        if (textIndex<=tutorialText.Length-1)
        {
            StartCoroutine(SquashAndStretch());
            textUI.transform.GetChild(1).GetComponentInChildren<TMP_Text>().text = tutorialText[textIndex];
            textIndex++;
        }
    }
    IEnumerator SquashAndStretch()
    {
        RectTransform rect = textUI.transform.GetChild(1).GetComponent<RectTransform>();
        rect.localScale = ogScale;
        Tween squash1 = rect.DOScale(new Vector3( ogScale.x*1.1f,ogScale.y*0.9f,ogScale.z),0.2f);
        yield return squash1.WaitForCompletion();
        Tween stretch1 = rect.DOScale(new Vector3( ogScale.x*0.9f,ogScale.y*1.1f,ogScale.z),0.2f);
        yield return stretch1.WaitForCompletion();
        Tween back = rect.DOScale(ogScale,0.2f);
        yield return back.WaitForCompletion();
        rect.localScale = ogScale;
    }
    public void ShowImage()
    {
        if (imageIndex<=tutorialImages.Length-1)
        {
            tutorialImages[imageIndex].SetActive(true);
            imageIndex++;
        }
    }

    public void HideImage()
    {
        foreach (var im in tutorialImages)
        {
            im.SetActive(false);
        }
    }
    

    
    
    public void TutorialBoat()
    {
        Vector3 spawnPos = CustomerSpawner.Instance.spawn.position;
        GameObject c = Instantiate(tutorialBoat);
        c.transform.position = spawnPos;
        c.GetComponent<CustomerBoat>().timer = 99999;
        customer = c.transform.GetChild(0).gameObject;
    }
    IEnumerator Tutorial()
    {
        cooler.GetComponent<Collider2D>().enabled = false;
        cuttingBoard.GetComponent<Collider2D>().enabled = false;
        riceCooker.GetComponent<Collider2D>().enabled = false;
        riceDrawer.GetComponent<Collider2D>().enabled = false;
        cooler.GetComponent<Collider2D>().enabled = false;
        ShowText();
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        textUI.SetActive(false);
        ShowImage();
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        HideImage();
        ShowText();
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        ShowText();
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        TutorialBoat();
        ShowText();
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        textUI.SetActive(false);
        ShowImage();
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        HideImage();
        ShowImage();
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => hook.fishOn);
        HideImage();
        ShowImage();
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        HideImage();
        ShowText();
        ShowImage();
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        HideImage();
        textUI.SetActive(false);
        bool flag1 = false;
        cooler.GetComponent<Collider2D>().enabled = true;
        cuttingBoard.GetComponent<Collider2D>().enabled = true;
        while (!flag1)
        {
            cooler.BlinkOn();
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => cooler.GetComponent<CoolerSlot>().itemOnStation==null);
            cooler.BlinkOff();
            cuttingBoard.BlinkOn();
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Mouse0));
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => !cuttingBoard.GetComponent<SectionCuttingBoard>().cuttingBoardUI.isActiveAndEnabled);
            foreach (var st in cuttingBoard.GetComponent<SectionCuttingBoard>().stations)
            {
                if (st.itemOnStation != null)
                {
                    flag1 = true;
                }
            }
            cuttingBoard.BlinkOff();
            yield return null;
        }
        
        cooler.GetComponent<Collider2D>().enabled = true;
        ShowImage();
        ShowText();
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        HideImage();
        textUI.SetActive(false);
        bool flag2 = false;
        riceDrawer.GetComponent<Collider2D>().enabled = true;
        riceCooker.GetComponent<Collider2D>().enabled = true;
        while (!flag2)
        {
            riceDrawer.BlinkOn();
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => GameObject.Find("draggablePrefab(Clone)"));
            riceDrawer.BlinkOff();
            riceCooker.BlinkOn();
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Mouse0));
            if (riceCooker.GetComponent<RiceCooker>().cooking)
            {
                flag2 = true;
            }
            riceCooker.BlinkOff();
            yield return null;
        }
        ShowText();
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        textUI.SetActive(false);
        bool flag3 = false;
        bool r = false;
        bool f = false;
        prepTable.GetComponent<Collider2D>().enabled = true;
        while (!flag3)
        {

            if(!r)
                riceCooker.BlinkOn();
            if(!f)
                cuttingBoard.BlinkOn();
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => GameObject.Find("draggablePrefab(Clone)"));
            riceCooker.BlinkOff();
            cuttingBoard.BlinkOff();
            prepTable.BlinkOn();
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Mouse0));
			foreach (ItemInstance ii in prepTable.GetComponent<PrepStation>().currentItems)
			{
				if (ii.itemData == cookedRice)
				{
					r = true;
				}
			}
			foreach (ItemInstance ii in prepTable.GetComponent<PrepStation>().currentItems) { 
	            if(ii.itemData == cutTuna) {
					f = true;
				}
	        }
            if (prepTable.GetComponent<PrepStation>().GetCurrentPreppedItem().itemData ==tunaNigiri)
            {
                flag3= true;
            }
            prepTable.BlinkOff();
            yield return null;
        }
        
        bool flag4 = false;
        while (!flag4)
        {
            prepTable.BlinkOn();
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => GameObject.Find("draggablePrefab(Clone)"));
            prepTable.BlinkOff();
            customer.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Mouse0));
            yield return new WaitForSeconds(0.1f);
            if (customer.GetComponent<Customer>().finished)
            {
                flag4 = true;
            }
                
            customer.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            yield return null;
        }
        prepTable.BlinkOff();
        cooler.GetComponent<Collider2D>().enabled = false;
        cuttingBoard.GetComponent<Collider2D>().enabled = false;
        riceCooker.GetComponent<Collider2D>().enabled = false;
        riceDrawer.GetComponent<Collider2D>().enabled = false;
        cooler.GetComponent<Collider2D>().enabled = false;
        ShowText();
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        ShowText();
        ShowImage();
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        HideImage();
        ShowText();
        ShowImage();
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        HideImage();
        ShowText();
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        
        ShowText();
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        Progress.BeatTutorial();
        SceneManager.LoadScene("MainMenu");

    }
}
