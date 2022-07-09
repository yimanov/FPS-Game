using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{

	public float objectHealth = 100f;


    public void objectHitDamage(float amount)
	{
		objectHealth -= amount;
        if(objectHealth<=0f)
		{
			Die();
		}
	}

    void Die()
	{
		Destroy(gameObject) ;
	}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
