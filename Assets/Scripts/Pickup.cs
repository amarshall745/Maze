using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class Pickup : MonoBehaviour
{
    public bool canBePickedUp = true;
    private bool returnTrue = false;
    public bool egg = false;
    public GameObject spawnPoint;

    void Start()
    {
        gameObject.transform.position = spawnPoint.transform.position;
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

    public void StealAndDestroy(Transform holder)
    {
        gameObject.transform.SetParent(holder);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(ShrinkOverTime(holder.transform.parent.gameObject));
    }

    public void StealAndCarry(Transform holder)
    {
        gameObject.transform.SetParent(holder);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    public bool CanBePickedUp(){
        if(canBePickedUp){
            return true;
        }else{
            return false;
        }
    }

    private IEnumerator ShrinkOverTime(GameObject enemy)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;
        float elapsed = 0f;

        while (elapsed < 1)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / 1);
            elapsed += Time.deltaTime;
            yield return null;
        } 

        transform.localScale = targetScale;
        enemy.GetComponent<EnemyController>().UnPause();
        Destroy(gameObject);
    }
}
