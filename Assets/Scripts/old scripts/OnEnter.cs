using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnter : MonoBehaviour
{
    public bool level4;
    public GameManager gm;
    public GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && level4)
        {
            Debug.Log("can press e");
            text.SetActive(true);
            if (Input.GetKey("e"))
                {
                Debug.Log("e has been pressed");
                    gm.nextScene();
                }
            
        }
    }


}
