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
    
    [SerializeField] private SpriteRenderer orderSR;

    public AudioClip successSoundEffect;
    public AudioClip okSoundEffect;

    public AudioClip failureSoundEffect;

    public bool orderComplete = false;

    public bool orderFailed = false;
    
    // Start is called before the first frame update
    public override void Start()
    {
        
        Init();
		base.Start();
	}

    public override bool OnItemAdd(ItemInstance item)
    {
	    if (order == null|| orderComplete|| orderFailed) {
            return false;
		}
        else if(order.recipe.recipeItem == item.itemData) {
            Debug.Log("CONGRATS");
			orderSR.sprite = rightOrder;
			if (item.quality >= 0.85f)
			{
				SoundManager.Instance.PlaySoundEffect(successSoundEffect);
			}
			else
			{
				SoundManager.Instance.PlaySoundEffect(okSoundEffect);
			}
			order.CompleteOrder();
			orderComplete = true;
		}
        else {
            Debug.Log("wrong order");
			orderSR.sprite = wrongOrder;
			order.FailOrder();
			orderFailed = true;
			SoundManager.Instance.PlaySoundEffect(failureSoundEffect);
		}
        float total = OrderManager.Instance.numOrdersEaten + 1;
        float oldweight = OrderManager.Instance.numOrdersEaten / total;
        float oldavg = OrderManager.Instance.averageOrderQuality * oldweight;
        float newavg = oldavg + item.quality * (1 - oldweight);
        OrderManager.Instance.averageOrderQuality = newavg;
        OrderManager.Instance.numOrdersEaten += 1;
		//OrderManager.Instance.UpdateOrderUI();
		//Destroy(gameObject);
		GetComponent<Collider2D>().enabled = false;
		return base.OnItemAdd(item);
	}

	public override bool OnColliderClicked()
    {
        bool ret = base.OnColliderClicked();

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

	public void Init()
	{
		
		orderComplete = false;
		orderFailed = false;
		itemOnStation = null;
		recipe = OrderManager.Instance.recipes[Random.Range(0, OrderManager.Instance.recipes.Count)];
		orderSR.sprite = recipe.recipeItem.sprite;
		GetComponent<Collider2D>().enabled = true;
		orderAdded = true;
		order = new OrderManager.Order(recipe, transform.parent.parent.GetComponent<Customer>().timer);
		OrderManager.Instance.orders.Add(order);
		//OrderManager.Instance.UpdateOrderUI();
	}
}
