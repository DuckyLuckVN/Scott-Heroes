using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperJump : MonoBehaviour {

    public GameObject particleActive;
    public AudioClip clipActive;
    public float forceJump;

    Animator myAnim;
    AudioSource myAudio;

	void Start () {
        myAnim = gameObject.GetComponent<Animator>();
        myAudio = gameObject.GetComponent<AudioSource>();
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Rigidbody2D playerBody = other.gameObject.GetComponent<Rigidbody2D>();
            playerBody.velocity = new Vector2(playerBody.velocity.x, Random.Range(forceJump - 1,forceJump + 2));

            myAnim.Play("Bumper_Action");

            Instantiate(particleActive, transform.position, transform.rotation);

            myAudio.clip = clipActive;
            myAudio.Play();

        }
    }
}
