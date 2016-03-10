using UnityEngine;
using System.Collections;

public class GunLMG : MonoBehaviour {

    // Det objekt som faktisk er våben-modellen, så vi kan rotere den når den skyder
    public GameObject gunGraphic;

    public GameObject muzzleFlashPrefab;
    public GameObject muzzleFlashPlacement;
    private ParticleSystem muzzleFlash;

    private bool isShooting = false;

    void FixedUpdate()
    {
        if (isShooting)
            gunGraphic.transform.Rotate(Vector3.up, 10.0f); //Rotér rundt om Y-aksen
    }

    public void StartShooting(Transform targetPos = null)
    {
        isShooting = true;

        GameObject muzzleFlashObject = (GameObject.Instantiate(muzzleFlashPrefab, muzzleFlashPlacement.transform.position, muzzleFlashPrefab.transform.rotation) as GameObject);
        muzzleFlashObject.transform.parent = muzzleFlashPlacement.transform;
        muzzleFlash = muzzleFlashObject.GetComponent<ParticleSystem>();

        if (muzzleFlash != null)
            muzzleFlash.GetComponent<ParticleSystem>().Play();
    }

    public void StopShooting()
    {
        isShooting = false;
        if (muzzleFlash != null)
            muzzleFlash.GetComponent<ParticleSystem>().Stop();
    }

    // Use this for initialization
    void Start()
    {
        if (muzzleFlash != null)
            muzzleFlash.GetComponent<ParticleSystem>().Stop();
    }
}
