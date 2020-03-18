using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_TreTrau : MonoBehaviour {

    public float timeLife;
    public float damage;

    public AudioClip clipThrow;
    public AudioClip clipAttack;
    public GameObject parAttack;

    Animator myAnim;
    AudioSource myAudio;

    Vector3 lastPos = Vector3.zero;

    bool hasFalen = false;

	void Start () {

        myAudio = gameObject.GetComponent<AudioSource>();
        myAnim = gameObject.GetComponent<Animator>();
        playAudio(clipThrow);
        Destroy(gameObject, 15);

    }
	

	void Update () {

        if(lastPos != Vector3.zero)
        {
            transform.position = lastPos;
        }


    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Block")
        {
            lastPos = transform.position;
            if(transform.localScale.x < 0)
            {
                myAnim.Play("Bullet_TreTrau_Landing_Right");
            }
            else
            {
                myAnim.Play("Bullet_TreTrau_Landing");
            }
            hasFalen = true;
            Destroy(gameObject, 3);
        }

        if (other.gameObject.tag == "Player" && hasFalen == false)
        {
            Health playerHealth = other.gameObject.GetComponent<Health>();
            playerHealth.addDamage(damage);
            hasFalen = true;
            Instantiate(parAttack, transform.position, transform.rotation);
            Destroy(gameObject, 0);
        }


    }

    void playAudio(AudioClip clip)
    {
        myAudio.clip = clip;
        myAudio.Play();
    }
}
