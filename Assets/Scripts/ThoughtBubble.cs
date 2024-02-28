using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThoughtBubble : Station
{
    public bool orderAdded = false;

    private Recipe recipe;
    private OrderManager.Order order;

    [SerializeField] private SpriteRenderer orderSR;
    // Start is called before the first frame update
    void Start()
    {
        recipe = OrderManager.Instance.recipes[Random.Range(0, OrderManager.Instance.recipes.Count)];
        orderSR.sprite = recipe.recipeItem.sprite;
    }

	public override bool OnItemAdd(Item item)
	{
        if(order.recipe.recipeItem == item) {
            Debug.Log("CONGRATS");
			order.CompleteOrder();
		}
        else {
            Debug.Log("wrong order");
            order.FailOrder();
		}
		Destroy(gameObject);
		return base.OnItemAdd(item);
	}

	public override bool OnColliderClicked()
    {
        bool ret = base.OnColliderClicked();
        if (!orderAdded)
        {
            orderAdded = true;
            order = new OrderManager.Order(recipe, GetComponentInParent<Rowboat>().timer);
            OrderManager.Instance.orders.Add(order);
            OrderManager.Instance.UpdateOrderUI();
        }
        //else
        //{
        //    if (OrderManager.Instance.CheckOrder(order))
        //    {
        //        OrderManager.Instance.UpdateOrderUI();
        //        Destroy(gameObject);
        //    }
        //}
        return ret;
    }
}
