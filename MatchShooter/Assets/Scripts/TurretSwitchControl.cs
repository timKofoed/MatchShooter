using UnityEngine;
using System.Collections;

public class TurretSwitchControl : MonoBehaviour {

    // Get the different turrets, their sockets, etc. and prepare to switch between them via the animation
    bool isSwitching = false;
    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = this.GetComponent<Animator>();
	}
	
    public void BeginTurretSwap()
    {
        Debug.Log("BeginTurretSwap");
        SwitchToTurret(0);
    }

    public void SwitchToTurret(int turretIndex)
    {
        isSwitching = true;
        if (animator != null)
            animator.SetTrigger("Switch");

        Debug.Log("attempting to switch to turret index " + turretIndex);
        StartCoroutine( DelayedSwitchPermission() );
    }

    private IEnumerator DelayedSwitchPermission()
    {
        yield return new WaitForSeconds(2f);
        isSwitching = false;
    }

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetAxis("TurretSwitch") > 0.0f)
        {
            if(!isSwitching)
                BeginTurretSwap();
        }
	}
}
