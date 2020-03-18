using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	void Start () {
    }
	

	void Update () {
		
	}


    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            Health player = other.gameObject.GetComponent<Health>();
            player.addDamage(10000);
        }
    }
}
