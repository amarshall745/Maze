using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject bullet;

    public bool canAutoFire;

    public float fireRate;
    [HideInInspector]
    public float fireCounter;

    public Transform firePoint;

    public float zoomAmount;

    public int currentAmmo, pickupAmount;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(fireCounter > 0)
        {
            fireCounter -= Time.deltaTime;
        }
    }

    public void GetAmmo()
    {
        currentAmmo += pickupAmount;

        UIController.instance.ammoText.text = " AMMO " + currentAmmo;
    }
}
