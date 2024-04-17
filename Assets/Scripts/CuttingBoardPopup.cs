using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CuttingBoardPopup : MonoBehaviour
{
    private int direction = 1;
    public GameObject knife;
    public float cutPosition;
    public float knifeSpeed = 2f;
    public List<float> correctCutPositions;
    public List<float> cutPositions;
    public List<GameObject> cutLines;
    private bool moving = true;
    public CuttingBoard cuttingBoard;

    public Image im;
    public Image shadow;
    public ItemInstance fish;
    
    float totalDist = 0;

    public Sprite[] lines;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(ItemInstance item, CuttingBoard board)
    {
        for (int i = 0; i < cutLines.Count; i++)
        {
            cutLines[i].GetComponent<Image>().sprite = lines[0];
            cutLines[i].SetActive(false);
            
        }

        knife.GetComponent<RectTransform>().localPosition = new Vector3(-500, knife.GetComponent<RectTransform>().localPosition.y,
            knife.GetComponent<RectTransform>().localPosition.z);
        fish = item;
        im.sprite = fish.itemData.sprite;
        shadow.sprite = fish.itemData.sprite;
        cuttingBoard = board;
        cutPositions = new List<float>();
        correctCutPositions = new List<float>(item.itemData.cutPositions);
        totalDist = 0;
        
        for (int j = 0; j < correctCutPositions.Count; j++)
        {
            cutLines[j].SetActive(true);
            cutLines[j].GetComponent<RectTransform>().localPosition = new Vector3(correctCutPositions[j],
                cutLines[j].GetComponent<RectTransform>().localPosition.y,
                cutLines[j].GetComponent<RectTransform>().localPosition.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(correctCutPositions.Count>0)
                Cut();
        }
        if (knife.GetComponent<RectTransform>().localPosition.x <= -500f)
        {
            direction = 1;
        }
        else if (knife.GetComponent<RectTransform>().localPosition.x >= 500f)
        {
            direction = -1;
        }

        if (moving)
        {
            if (direction == 1)
            {
                knife.GetComponent<RectTransform>().localPosition =
                    knife.GetComponent<RectTransform>().localPosition + new Vector3(knifeSpeed, 0, 0)*Time.deltaTime;
            }
            else if (direction == -1)
            {
                knife.GetComponent<RectTransform>().localPosition =
                    knife.GetComponent<RectTransform>().localPosition - new Vector3(knifeSpeed, 0, 0)*Time.deltaTime;
            }
        }

    }

    public void Cut()
    {
        cutPositions.Add(knife.GetComponent<RectTransform>().localPosition.x);
        
        StartCoroutine(CutDelay());
    }

    public IEnumerator CutDelay()
    {
        moving = false;
        yield return new WaitForSeconds(0.5f);
        moving = true;
        UpdateCut();
        
        if (correctCutPositions.Count==0)
        {
            yield return new WaitForSeconds(0.5f);
            CloseBoard();
        }
    }


    public void UpdateCut()
    {
        float minDiff = Mathf.Abs(correctCutPositions[0]- cutPositions[cutPositions.Count-1]);
        float posToRemove = correctCutPositions[0];
         
        GameObject lineToChange= cutLines[cutPositions.Count-1];
        for (int i = 0; i < correctCutPositions.Count; i++)
        {
            float diff = Mathf.Abs(correctCutPositions[i]-cutPositions[cutPositions.Count-1]);
            if (diff < minDiff)
            {
                minDiff = diff;
                posToRemove = correctCutPositions[i];
            }
        }
        totalDist += minDiff;
        if (minDiff <= 100)
        {
            lineToChange.GetComponent<Image>().sprite = lines[1];

        }
        else
        {
            lineToChange.GetComponent<Image>().sprite = lines[2];
        }
        lineToChange.GetComponent<RectTransform>().localPosition = new Vector3(
            cutPositions[cutPositions.Count-1],
            lineToChange.GetComponent<RectTransform>().localPosition.y,
            lineToChange.GetComponent<RectTransform>().localPosition.z);
        correctCutPositions.Remove(posToRemove);
    }

    public float DetermineQuality()
    {
        float qualityMultiplier = 1;
        if (totalDist >= 500)
        {
            qualityMultiplier = 0;
        }
        else if(totalDist<=100)
        {
            qualityMultiplier = 1.5f;
        }
        else
        {
            qualityMultiplier = (1000 - totalDist) / 1000;
        }
        return fish.quality* qualityMultiplier;
    }
    public void CloseBoard()
    {
        ItemInstance newitem = new ItemInstance(fish.itemData.processed, Mathf.Min(1,DetermineQuality()));
        cuttingBoard.itemOnStation = newitem;
        cuttingBoard.UpdateSprite();
        gameObject.SetActive(false);
    }
}
