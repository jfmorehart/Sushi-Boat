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
	public Vector2 tentaclesEnd;

	public bool cutscene;

	public AudioClip cutsceneMusic;
	public AudioClip bossbackGroundMusic;
	
	// Start is called before the first frame update
	void Start()
    {
		if (cutscene) {
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
		SoundManager.Instance.StartBGM(cutsceneMusic);
		yield return new WaitForSeconds(2f);
		CameraController cc = Camera.main.GetComponent<CameraController>();
		cc.target = cc.fishTarget;
		cc.trackingHook = true;
		Hook.ins.active = false;
		yield return new WaitForSeconds(3f);
		Camera.main.GetComponent<CameraShake>().CutScene(1.5f);
		float st = Time.time;

		while (true) {
			if (Vector3.Distance(grabby.position, grabbyEnd) > 0.1f)
			{
				grabby.position = Vector3.Lerp(start, grabbyEnd, (Time.time - st) / 3);
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
				grabby.position = Vector3.Lerp(grabbyEnd, start, (Time.time - st) / 3);
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
			Camera.main.GetComponent<CameraShake>().cutscene=true;
			if (Vector3.Distance(tentacles.position, tentaclesEnd) > 0.1f)
			{
				tentacles.transform.position = Vector3.Lerp(start, tentaclesEnd, (Time.time - st) / 2);
				yield return new WaitForEndOfFrame();
			}
			else
			{
				CameraShake c = Camera.main.GetComponent<CameraShake>();
				c.cutscene=false;
				c.transform.localPosition = c.cutscenePos;
				GameManager.Instance.gameState = GameManager.GameState.DayGoing;
				CameraController cc = Camera.main.GetComponent<CameraController>();
				Hook.ins.active = false;
				cc.trackingHook = false;
				cc.target = cc.boatTarget;
				
				
				SoundManager.Instance.StartBGM(bossbackGroundMusic);
				yield break;
			}
		}

	}
}
