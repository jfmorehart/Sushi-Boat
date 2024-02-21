using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtBubble : Clickable
{
    public bool orderAdded = false;

    private Recipe recipe;
    private OrderManager.Order order;

    [SerializeField] private SpriteRenderer orderSR;
    // Start is called before the first frame update
    void Start()
    {
        recipe = OrderManager.Instance.recipes[Random.Range(0, OrderManager.Instance.recipes.Count)];
        orderSR.sprite = recipe.recipeSprie;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public override void OnColliderClicked()
    {
        base.OnColliderClicked();
        if (!orderAdded)
        {
            orderAdded = true;
            order = new OrderManager.Order(recipe, GetComponentInParent<Rowboat>().timer);
            OrderManager.Instance.orders.Add(order);
            OrderManager.Instance.UpdateOrderUI();
        }
        else
        {
            if (OrderManager.Instance.CheckOrder(order))
            {
                OrderManager.Instance.UpdateOrderUI();
                Destroy(gameObject);
            }
        }

    }
}
