using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }
	public int completed;
	public int failed;

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
        public float currentTimer;
        public int price;
            
        public Order(Recipe r,float t)
        {
            recipe = r;
            timer = t;
            price = r.price;
            currentTimer = timer;
        }

        public void FailOrder()
        {
            OrderManager.Instance.orders.Remove(this);
            //temporary measure for keeping track of order count for customer spawning
            CustomerSpawner.Instance.currentOrders --;
            OrderManager.Instance.failed++;
        }

        public void CompleteOrder()
        {
            GameManager.Instance.money += price;
            OrderManager.Instance.orders.Remove(this);
            //temporary measure for keeping track of order count for customer spawning
            CustomerSpawner.Instance.currentOrders --;
			OrderManager.Instance.completed++;
		}
        
    }

    public List<Order> orders;
    public List<Recipe> recipes;
    public int maxOrdersCount;
    public GameObject ordersUI;

    public TMP_Text moneyUI;
    
    // Start is called before the first frame update
    void Start()
    {
        orders = new List<Order>();
        for (int i = 0; i < ordersUI.transform.childCount; i++)
        {
            ordersUI.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.DayGoing)
        {
            UpdateOrderTimer();
        }
        moneyUI.text = "$" + GameManager.Instance.money;
    }

    public void UpdateOrderUI()
    {
        List<Order> ordersCopy = orders.OrderByDescending(o => o.timer).ToList();
        orders = new List<Order>(ordersCopy);
        for (int l = 0; l < maxOrdersCount; l++)
        {
            ordersUI.transform.GetChild(l).gameObject.SetActive(false);
        }
        for (int i = 0; i < orders.Count; i++)
        {
            Transform ord = ordersUI.transform.GetChild(i);
            ord.gameObject.SetActive(true);
            ord.GetChild(1).GetComponent<Image>().sprite = orders[i].recipe.recipeItem.sprite;

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
            ord.GetChild(3).localScale = new Vector3(orders[i].currentTimer / orders[i].timer, 1, 1);
            orders[i].currentTimer -= Time.deltaTime;
            if (orders[i].currentTimer <= 0)
            {
                orders[i].FailOrder();
                UpdateOrderUI();
            }
        }
    }

    //deprecated
    /*public bool CheckOrder(Order order)
    {
        List<Item> inventoryCopy = new List<Item>(InventoryManager.Instance.items);
        for (int i = 0; i < order.recipe.ingredients.Count; i++)
        {
            if (inventoryCopy.Contains(order.recipe.ingredients[i]))
            {
                inventoryCopy.Remove(order.recipe.ingredients[i]);
            }
            else
            {
                UpdateOrderUI();
                return false;
            }
        }

        for (int j = 0; j < order.recipe.ingredients.Count; j++)
        {
            InventoryManager.Instance.RemoveItem(order.recipe.ingredients[j]);
        }
        InventoryManager.Instance.UpdateInventoryUI();
        order.CompleteOrder();
        UpdateOrderUI();
        return true;
    }*/
    

}
