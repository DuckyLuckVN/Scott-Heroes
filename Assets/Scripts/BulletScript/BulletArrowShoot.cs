using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletArrowShoot : MonoBehaviour
{


    public float moveSpeed;
    public AudioClip clipShoot_1;
    public float damage;
    public GameObject particleAttack;

    Rigidbody2D myBody;
    AudioSource myAudioSource;


    void Start()
    {
        myBody = gameObject.GetComponent<Rigidbody2D>();
        myAudioSource = gameObject.GetComponent<AudioSource>();

        myAudioSource.clip = clipShoot_1;

        myAudioSource.Play();

    }

    void Update()
    {


        if (transform.rotation.z < 0)
        {
            myBody.velocity = new Vector2(-moveSpeed, myBody.velocity.y);
        }
        else
        {
            myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y);
        }


    }

 
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Health myHealth = other.gameObject.GetComponent<Health>();
            if(myHealth.getHealth() > 0)
            {
                myHealth.addDamage(damage);
                Destroy(gameObject);
                Instantiate(particleAttack, transform.position, transform.rotation);
            }

        }

        if(other.gameObject.tag == "Block")
        {
            Instantiate(particleAttack, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

}