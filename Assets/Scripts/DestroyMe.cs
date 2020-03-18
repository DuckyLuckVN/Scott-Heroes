using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour {

    public float rateTime;

    float timeDestroy;
	
	void Start () {

        rateTime = Time.time + rateTime;

    }
	
	
	void Update () {

        if(rateTime <= Time.time)
        {
            Destroy(gameObject);
        }

		
	}
}
