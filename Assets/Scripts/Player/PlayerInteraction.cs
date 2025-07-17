using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{    
    [Header("Raycast")]
    public Transform raycastPoint;
    public float raycast10 = 10f;

    [Header("Pick up")]
    public GameObject pickedUpGO;


    void Update()
    {
        RayCast();
    }

    
    public void RayCast()
    {
        Ray ray = new Ray(raycastPoint.position, raycastPoint.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            GameObject hitGO = hit.collider.gameObject;

            if (Input.GetKeyDown(KeyCode.E))
            {
                // Drop picked up item
                if (pickedUpGO != null)
                {
                    pickedUpGO.GetComponent<Pickup>().Drop();
                    pickedUpGO = null;
                    return;
                }

                if (hit.distance <= raycast10)
                {
                    Debug.Log("Less than 10");

                    if (hitGO.layer == LayerMask.NameToLayer("Pickup") && pickedUpGO == null)
                    {
                        Pickup pickupScript = hitGO.GetComponent<Pickup>();

                        // Pick up item
                        if (pickupScript != null && pickupScript.CanBePickedUp())
                        {
                            pickupScript.PickUp(gameObject);
                            pickedUpGO = hitGO;
                        }
                    }
                }
            }
        }

    }
}
