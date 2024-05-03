using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWiggle : MonoBehaviour
{
    public float freq = 1;
    public float amp = 1;

	public float scale_freq = 1;
	public float scale_amp = 1;

	float seed;

	private void Start()
	{
		seed = Random.Range(-5f, 5f);
	}
	// Update is called once per frame
	void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, Mathf.Sin(seed + Time.time * freq) * amp);
		Vector3 scale = new Vector3(
			scale_amp * Mathf.Sin(scale_freq * Time.time + seed),
			scale_amp * Mathf.Sin(scale_freq * Time.time + seed * 2),
			0);
		//Remap to 0-1
		scale *= 0.5f;
		scale += Vector3.one;
		transform.localScale = scale;
    }
}
