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
    
    public ItemInstance fish;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(ItemInstance item, CuttingBoard board)
    {
        for (int i = 0; i < cutLines.Count; i++)
        {
            cutLines[i].SetActive(false);
        }
        fish = item;
        cuttingBoard = board;
        correctCutPositions = item.itemData.cutPositions;
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
            if(cutPositions.Count<correctCutPositions.Count)
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
                    knife.GetComponent<RectTransform>().localPosition + new Vector3(knifeSpeed, 0, 0);
            }
            else if (direction == -1)
            {
                knife.GetComponent<RectTransform>().localPosition =
                    knife.GetComponent<RectTransform>().localPosition - new Vector3(knifeSpeed, 0, 0);
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
        if (cutPositions.Count == correctCutPositions.Count)
        {
            CloseBoard();
        }
    }


    public float DetermineQuality()
    {
        float qualityMultiplier = 1;
        float totalDist = 0;
        for (int i = 0; i < correctCutPositions.Count; i++)
        {
            float minDiff = Mathf.Abs(correctCutPositions[i] - cutPositions[0]);
            for (int j = 0; j < cutPositions.Count; j++)
            {
                float diff = Mathf.Abs(correctCutPositions[i] - cutPositions[j]);
                if (diff < minDiff)
                {
                    minDiff = diff;
                }
            }
            totalDist += minDiff;
        }

        if (totalDist >= 500)
        {
            qualityMultiplier = 0;
        }
        else if(totalDist<=30)
        {
            qualityMultiplier = 1.5f;
        }
        else
        {
            qualityMultiplier = (500 - totalDist) / 500;
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
