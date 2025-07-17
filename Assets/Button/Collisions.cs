using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    public Animator doorAnimation;
    public static Collisions instance;
    // Start is called before the first frame update
    void Start()
    {
        //doorAnimation.SetBool("doorIsMoving", false);
    }

    // Update is called once per frame
    private void Awake()
    {
        instance = this;
    }
    void Update()
    {
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("can press button");
            //PlayerController.instance.PessButton();
        }
    }

    public void doorOpening()
    {
        Debug.Log("door animations");
        doorAnimation.SetBool("doorIsOpen", true);
    }
}
