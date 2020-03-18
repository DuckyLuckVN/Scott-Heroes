using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletJavelin: MonoBehaviour
{


    public float moveSpeed;
    public AudioClip clipShoot;
    public float destroyRate;

    Collider2D coll2D;
    

    Rigidbody2D myBody;
    AudioSource myAudioSource;
    Animator myAnimator;
    Vector3 lastPosision;

    bool isMove = true;
    float timeDestroy;

    void Start()
    {
        coll2D = gameObject.GetComponent<Collider2D>();
        coll2D.enabled = false;

        timeDestroy = Time.time + destroyRate;

        myBody = gameObject.GetComponent<Rigidbody2D>();
        myAudioSource = gameObject.GetComponent<AudioSource>();
        myAnimator = gameObject.GetComponent<Animator>();

        myAudioSource.clip = clipShoot;
        myAudioSource.Play();

    }

    void Update()
    {
        //----DESTROY EVENT----//
        if (timeDestroy<= Time.time)
        {
            myAnimator.SetBool("destroy", true);
            GameObject.Destroy(gameObject, 0.50f);
        }


        if(isMove == true)
        {
            if (transform.rotation.z < 0)
            {
                myBody.velocity = new Vector2(-moveSpeed, myBody.velocity.y);
            }
            else if (transform.rotation.z >= 0)
            {
                myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y);
            }
        } else
        {
            gameObject.transform.position = lastPosision;
        }




    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Block")
        {
            isMove = false;
            lastPosision = gameObject.transform.position;
            coll2D.enabled = true;
        }
    }
}
