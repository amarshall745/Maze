using UnityEditor.Build;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Egg"))
        {
            EggCollected(other.gameObject);
        }
        else
        {
            OtherItem(other.gameObject);
        }
    }

    public void EggCollected(GameObject egg)
    {
        Debug.Log("Egg collected!!!!!!");
    }
    
    public void OtherItem(GameObject other)
    {
        Debug.Log(other + " item collected");
    }
}
