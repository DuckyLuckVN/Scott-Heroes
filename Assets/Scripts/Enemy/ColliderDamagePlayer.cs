using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDamagePlayer : MonoBehaviour {

    public float damageTrigger;
    public float damageCollision;
    public float knockback;
    public GameObject particleAttack;


    void Start () {
	}
	

	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && other.isTrigger == false)
        {
            Health health = other.gameObject.GetComponent<Health>();
            health.addDamage(damageTrigger);
            Instantiate(particleAttack, other.transform.position, transform.rotation);
        }
    }

}
