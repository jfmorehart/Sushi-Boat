using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLogic : MonoBehaviour
{
    Vector3 start;
    float animLength = 5;


    public Transform tentacles;
    public Transform meteor;
    public Transform grabby;
    public Sprite grabby2;
	public Vector2 grabbyEnd;

	public bool cutscene;
	// Start is called before the first frame update
	void Start()
    {
		if (cutscene) {
			CustomerSpawner.Instance.bossLock = true;
			Invoke(nameof(CutScene), 0.1f);
		}
        start = tentacles.transform.position;
    }

    void CutScene() {
        GameManager.Instance.gameState = GameManager.GameState.DayEnded;
		StartCoroutine(Grab());
	}


	IEnumerator Grab()
	{
		yield return new WaitForSeconds(2f);
		CameraController cc = Camera.main.GetComponent<CameraController>();
		cc.target = cc.fishTarget;
		cc.trackingHook = true;
		yield return new WaitForSeconds(2f);
		float st = Time.time;
		while (true) {
			if (Vector3.Distance(grabby.position, Vector3.zero) > 0.1f)
			{
				grabby.position = Vector3.Lerp(start, Vector3.zero, (Time.time - st) / 3);
				yield return new WaitForEndOfFrame();
            }
            else {
                Destroy(meteor.gameObject);
                grabby.GetComponent<SpriteRenderer>().sprite = grabby2;
				StartCoroutine(ReturnGrab());
				yield break;
	        }
		}

	}
    IEnumerator ReturnGrab(){
		float st = Time.time;
		while (true)
		{
			if (Vector3.Distance(grabby.position, start) > 0.1f)
			{
				grabby.position = Vector3.Lerp(Vector3.zero, start, (Time.time - st) / 3);
				yield return new WaitForEndOfFrame();
			}
			else
			{
				Destroy(grabby.gameObject);
				StartCoroutine(Rise());
				yield break;
			}
		}
	}
	IEnumerator Rise() {
		float st = Time.time;
		while (true)
		{
			if (Vector3.Distance(tentacles.position, Vector3.zero) > 0.1f)
			{
				tentacles.transform.position = Vector3.Lerp(start, Vector3.zero, (Time.time - st) / 2);
				yield return new WaitForEndOfFrame();
			}
			else
			{
				GameManager.Instance.gameState = GameManager.GameState.DayGoing;
				Hook.ins.active = true;
				CameraController cc = Camera.main.GetComponent<CameraController>();
				cc.target = cc.boatTarget;
				yield return new WaitForSeconds(2f);
				CustomerSpawner.Instance.bossLock = false;
				yield break;
			}
		}

	}
}
