using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockout : MonoBehaviour {

    public float damage;
    public AudioClip clipKnockoutBullet;
    public AudioClip clipKnockoutEnemy;

    AudioSource myAudio;
	void Start () {

        myAudio = GetComponent<AudioSource>();
    }
	
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            Health otherHealth = other.GetComponent<Health>();
            otherHealth.addDamage(damage);
            other.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 15);
            playAudio(clipKnockoutEnemy);
        }
        else if(other.tag == "Bullet")
        {
            other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 500));
            playAudio(clipKnockoutBullet);
        }

    }

    void playAudio(AudioClip clip)
    {
        myAudio.clip = clip;
        myAudio.Play();
    }
}
