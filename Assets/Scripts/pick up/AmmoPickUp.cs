using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    private bool collected;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player" && !collected)
        {
            //PlayerController.instance.activeGun.GetAmmo();

            Destroy(gameObject);

            collected = true;
        }
    }
}
