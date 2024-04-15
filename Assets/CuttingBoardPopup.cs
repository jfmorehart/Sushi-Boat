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
        for (int i = 0; i < correctCutPositions.Count; i++)
        {
            
        }

        return fish.quality*1;
    }
    public void CloseBoard()
    {
        ItemInstance newitem = new ItemInstance(fish.itemData.processed, DetermineQuality());
        cuttingBoard.itemOnStation = newitem;
        cuttingBoard.UpdateSprite();
        gameObject.SetActive(false);
    }
}
