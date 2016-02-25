using UnityEngine;
using System.Collections;

public class SidewaysPulseScript : MonoBehaviour {


    //shortcut for objects transform component
    private Transform myTransform;

    //sets how far to the sides the object moves
    private float pulseMin = -1.5f;
    private float pulseMax = 2.3f;

    //used for setting how fast the object moves from side to side
    private float pulseSpeed = 0.11f;

    //used as incremation value for moving the object
    private float pulseDirection;



    // Use this for initialization
    void Awake()
    {
        //assigns pulseDirection a value based on pulse speed
        pulseDirection = pulseSpeed;

        //assigns myTransform the gmaeObjects Transform component
        myTransform = gameObject.GetComponent<Transform>();

    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {

        //updates the objects position using "pulseDirection"
        myTransform.position += new Vector3(pulseDirection, 0);

        //Checks if this Objects current position is greater than than "PulseMax"
        if (myTransform.position.x > pulseMax)
        {
            //Debug.Log("Change direction moving left");
            pulseDirection = -pulseSpeed;
        }

        //Checks if this Objects current position is less than than "pulseMin"
        else if (myTransform.position.x < pulseMin)
        {
            //Debug.Log("Change direction moving right");
            pulseDirection = pulseSpeed;
        }

    }

}
