using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Pickup : MonoBehaviour
{
    public bool canBePickedUp = true;

    void Update()
    {
        
    }

    public void PickUp(GameObject Player){
        Transform hand = Player.transform.Find("Camera Point/Hand");
        if (hand != null)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.transform.SetParent(hand);
            gameObject.transform.localPosition = Vector3.zero;
            canBePickedUp = false;
        }
        else{
            Debug.Log("No player hand detected");
        }

    }

    public void Drop()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        canBePickedUp = true;
        transform.SetParent(null);
        //gameObject.transform.localPosition = new Vector3(gameObject.transform.position.x, 0.5f, gameObject.transform.position.z);
        //gameObject.transform.localRotation = Quaternion.identity;
    }

    public bool CanBePickedUp(){
        if(canBePickedUp){
            return true;
        }else{
            return false;
        }
    }
}
