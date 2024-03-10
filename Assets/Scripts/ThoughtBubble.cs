using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThoughtBubble : Station
{
    public bool orderAdded = false;

    private Recipe recipe;
    private OrderManager.Order order;

    public Sprite wrongOrder;
    public Sprite rightOrder;

    SpriteRenderer tsr;
    [SerializeField] private SpriteRenderer orderSR;
    // Start is called before the first frame update
    public override void Start()
    {
        tsr = GetComponent<SpriteRenderer>();
        recipe = OrderManager.Instance.recipes[Random.Range(0, OrderManager.Instance.recipes.Count)];
		base.Start();
	}

	public override bool OnItemAdd(Item item)
	{
        if (order == null) {
            return false;
		}
        else if(order.recipe.recipeItem == item) {
            Debug.Log("CONGRATS");
			orderSR.sprite = rightOrder;
			order.CompleteOrder();
		}
        else {
            Debug.Log("wrong order");
			orderSR.sprite = wrongOrder;
			order.FailOrder();
		}
		OrderManager.Instance.UpdateOrderUI();
		orderSR.sortingOrder = -21;
        tsr.sortingOrder = -22;
		//Destroy(gameObject);
		GetComponent<Collider2D>().enabled = false;
		return base.OnItemAdd(item);
	}

	public override bool OnColliderClicked()
    {
        bool ret = base.OnColliderClicked();
        if (!orderAdded)
        {
			orderSR.sprite = recipe.recipeItem.sprite;
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
