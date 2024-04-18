using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Fish : MonoBehaviour
{
	public FishData data;
	
	[SerializeField]
	string fishName;
    [SerializeField]
    float swimSpeed;
	[SerializeField] float bobFreq, bobSpeed;
	float freqSeed, ampSeed;

    public int direction;

    FishState state;

    private SpriteRenderer sr;

	public LayerMask hookmask;

	public bool inBackground;


	public float quality;

    public void Init(FishData fData)
    {
		data = fData;
		if(data == null) {
			Debug.LogError("no data!");
		}
		//FishAnimInfo f = FindObjectOfType<FishAnimInfo>(false);
		GetComponent<Animator>().runtimeAnimatorController = Instantiate(fData.animController);

		//fishName = data.fishItem.itemName;
		swimSpeed = data.swimSpeed * Random.Range(0.75f, 1.25f);
		bobFreq *= Random.Range(1f, 1.25f);
		GetComponent<SpriteRenderer>().sprite = data.fishSprite;
		freqSeed = Random.Range(1, 1000f);
		ampSeed = Random.Range(0.6f, 1f);

		if (inBackground) {
			Vector3 pos = transform.position;
			Vector3 scl = transform.localScale;
			pos.y *= 0.8f;
			pos.z = -0.1f;
			scl *= 0.6f;
			swimSpeed *= 0.5f;
			transform.position = pos;
			transform.localScale = scl;
			GetComponent<Renderer>().material.SetColor("_col", FishSpawner.ins.backCol);
		}
		
		float roll = Random.Range(0, 100f);
		if(roll > 96) {
			GetComponent<Animator>().SetInteger("QualityLevel", 4);
			quality = 2;
		}else if(roll > 60) {
			GetComponent<Animator>().SetInteger("QualityLevel", 3);
			quality = 0.8f;
		}
		else if (roll > 30)
		{
			GetComponent<Animator>().SetInteger("QualityLevel",2);
			quality = 0.5f;
		}
		else
		{
			GetComponent<Animator>().SetInteger("QualityLevel",1);
			quality = 0.3f;
		}
		GetComponent<Renderer>().material.SetFloat("_qual", 1);
	}
	private void Awake()
	{
        state = FishState.Swimming;
        sr = GetComponent<SpriteRenderer>();
		sr.enabled = false;
		//Menu.EndDayAction += Despawn;
	}
	// Update is called once per frame
	void Update()
    {
        switch (state) {
            case FishState.Unassigned:
                break;
            case FishState.Swimming:
                SwimmingUpdate();
                break;
			case FishState.Caught:
                //wiggle
				break;
		}
    }
	public void Despawn() {
		//Menu.EndDayAction -= Despawn;
		if (gameObject != null)
		{
			Destroy(gameObject);
		}
	}

    public void Hooked() {
		if (sr.flipX) {
			sr.flipX = false;
			sr.flipY = true;
		}

        state = FishState.Caught;
        transform.parent = Hook.ins.transform;

        transform.position = Hook.ins.fishCaughtAnchor.position;
		transform.localPosition = (Vector2)transform.localPosition - new Vector2(direction * -data.mouthPos.y, data.mouthPos.x);
		transform.localEulerAngles = new Vector3(0, 0, -90);
		Hook.ins.fishOn = true;
		Hook.ins.onHook = this;
		
		SoundManager.Instance.PlaySoundEffect(Hook.ins.hooked);
    }

    void SwimmingUpdate() {
		if(!sr.enabled)sr.enabled = true;
		transform.Translate(direction * swimSpeed * Time.deltaTime * Vector3.right, Space.World);
		transform.Translate(Vector2.up * bobSpeed * Mathf.Sin(freqSeed + Time.time * bobFreq * ampSeed) * Time.deltaTime, Space.World);
		if (direction == 1)
		{
			sr.flipX = true;
		}
		else
		{
			sr.flipX = false;
		}
		if (transform.position.x > FishSpawner.ins.rightBound)
		{
			Despawn();
		}
		if (transform.position.x < FishSpawner.ins.leftBound)
		{
			Despawn();
		}
		if (inBackground) return;
		Debug.DrawRay(transform.position + new Vector3(data.mouthPos.x * direction, data.mouthPos.y, 0), direction * Vector2.right, Color.red);
		RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(data.mouthPos.x * direction, data.mouthPos.y, 0), direction * Vector2.right, 1 );
		if (hit && !Hook.ins.fishOn) {
			if (hit.collider.CompareTag("Hook")) {
				Hooked();
			}
		}
	}

    enum FishState{ 
        Unassigned, 
        Swimming, 
        OnTheHook, 
        Caught
    }
}
