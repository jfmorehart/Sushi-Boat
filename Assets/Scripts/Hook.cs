using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public KeyCode up;
    public KeyCode down;

    public float reelAccel;
    public float velocity;
    public float drag;

    // Update is called once per frame
    void Update()
    {
        // mouse controls
        //Vector3 mposQuery = Input.mousePosition; //mouse pos in pixel coords
        //mposQuery.z = 10; // distance from camera to geometry
        //Vector2 mousePosition = Camera.main.ScreenToWorldPoint(mposQuery);

        //Vector2 wpos = transform.position;
        //wpos.y = mousePosition.y;
        //transform.position = wpos;

        //keyboard

        float dir = Input.GetKey(up)? 1 : 0  + (Input.GetKey(down)? -1 : 0);

        velocity += (dir * reelAccel * Time.deltaTime);
        velocity *= 1 - Time.deltaTime * drag;
        transform.Translate(Time.deltaTime * velocity * Vector2.up, Space.World);

        
    }
}
