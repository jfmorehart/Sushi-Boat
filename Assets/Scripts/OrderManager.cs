using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    
    public class Order
    {
        public Recipe recipe;
        public float timer;
        public int money;
            
        public Order(Recipe r,float t)
        {
            recipe = r;
            timer = t;
            money = r.price;
        }

        public void FailOrder()
        {
            OrderManager.Instance.orders.Remove(this);
        }

        public void CompleteOrder()
        {
            OrderManager.Instance.orders.Remove(this);
        }
    }

    public List<Order> orders;
    public List<Recipe> recipes; 
    public float orderExpireTime = 20f;
    public int maxOrdersCount;
    public GameObject ordersUI;
    
    // Start is called before the first frame update
    void Start()
    {
        orders = new List<Order>();
        for (int i = 0; i < 3; i++)
        {
            NewOrder();
        }
        UpdateOrderUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (orders.Count < maxOrdersCount)
        {
            NewOrder();
        }
        UpdateOrderTimer();
    }

    public void NewOrder()
    {
        Order newOrder = new Order( recipes[Random.Range(0,recipes.Count)],orderExpireTime);
        orders.Add(newOrder);
    }

    public void UpdateOrderUI()
    {
        for (int i = 0; i < orders.Count; i++)
        {
            Transform ord = ordersUI.transform.GetChild(i);
            ord.gameObject.SetActive(true);
            ord.GetChild(1).GetComponent<Image>().sprite = orders[i].recipe.recipeSprie;

            for (int k = 0; k < ord.GetChild(2).childCount; k++)
            {
                ord.GetChild(2).GetChild(k).GetComponent<InventorySlot>().item = null;
                ord.GetChild(2).GetChild(k).gameObject.SetActive(false);
            }
            for (int j = 0; j < orders[i].recipe.ingredients.Count; j++)
            {
                Item ingredient = orders[i].recipe.ingredients[j];
                if (ingredient != null)
                {
                    ord.GetChild(2).GetChild(j).gameObject.SetActive(true);
                    ord.GetChild(2).GetChild(j).GetComponent<InventorySlot>().item = ingredient;
                    ord.GetChild(2).GetChild(j).GetComponent<Image>().sprite = ingredient.sprite;
                }
                else
                {
                    ord.GetChild(2).GetChild(j).gameObject.SetActive(false);
                }
            }

        }
    }

    public void UpdateOrderTimer()
    {
        for (int i = 0; i < orders.Count; i++)
        {
            Transform ord = ordersUI.transform.GetChild(i);
            ord.GetChild(3).localScale = new Vector3(orders[i].timer / orderExpireTime, 1, 1);
            orders[i].timer -= Time.deltaTime;
            if (orders[i].timer <= 0)
            {
                orders[i].FailOrder();
                UpdateOrderUI();
            }
        }
    }
}
