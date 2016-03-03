﻿using UnityEngine;
using System.Collections;

public class GunPlasmaCaster : MonoBehaviour {

    // Det objekt som faktisk er våben-modellen, så vi kan rotere den når den skyder
    public GameObject gunGraphic;

    public GameObject muzzleFlashPrefab;
    public GameObject muzzleFlashPlacement;
    private ParticleSystem muzzleFlash;

    public GameObject fuelHolderRight;
    public GameObject fuelHolderLeft;
    private float fuelScaleStart;
    private float fuelRemaining = 1.0f;

    private bool isShooting = false;

    private Vector3 localScaleTemp;

    void FixedUpdate()
    {
        if (isShooting)
        {
            fuelRemaining -= 0.005f;
            fuelRemaining = (fuelRemaining < 0.0f)?0.0f:fuelRemaining;

            localScaleTemp.x = fuelScaleStart;
            localScaleTemp.y = fuelScaleStart;
            localScaleTemp.z = fuelScaleStart * fuelRemaining;

            if (fuelHolderLeft != null)
                fuelHolderLeft.transform.localScale = localScaleTemp;

            if (fuelHolderRight != null)
                fuelHolderRight.transform.localScale = localScaleTemp;
        }
        //    gunGraphic.transform.Rotate(Vector3.up, 10.0f); //Rotér rundt om Y-aksen
    }

    public void StartShooting()
    {
        isShooting = true;

        //GameObject muzzleFlashObject = (GameObject.Instantiate(muzzleFlashPrefab, muzzleFlashPlacement.transform.position, muzzleFlashPrefab.transform.rotation) as GameObject);
        //muzzleFlashObject.transform.parent = muzzleFlashPlacement.transform;
        //muzzleFlash = muzzleFlashObject.GetComponent<ParticleSystem>();

        //if (muzzleFlash != null)
        //    muzzleFlash.GetComponent<ParticleSystem>().Play();
    }

    public void StopShooting()
    {
        isShooting = false;
        //if (muzzleFlash != null)
        //    muzzleFlash.GetComponent<ParticleSystem>().Stop();
    }

    // Use this for initialization
    void Start()
    {
        if (fuelHolderLeft != null)
            fuelScaleStart = fuelHolderLeft.transform.localScale.z;

        if (fuelHolderRight != null)
            fuelScaleStart = fuelHolderRight.transform.localScale.z;

        if (muzzleFlash != null)
            muzzleFlash.GetComponent<ParticleSystem>().Stop();
    }
}
