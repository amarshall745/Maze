using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealthController : MonoBehaviour
{
    public int currentHealth = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageMonster(int damageAmount)
    {
        currentHealth -= damageAmount;

        if(currentHealth <=0)
        {
            Destroy(gameObject);
        }
    }


}
