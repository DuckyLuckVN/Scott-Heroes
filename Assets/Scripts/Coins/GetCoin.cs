using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCoin : MonoBehaviour {

    public AudioClip clipGetCoin;
    public GameObject parGetCoin;

    AudioSource myAudio;
    Animator myAnim;

    bool getCoin = false;
	void Start () {
        myAudio = gameObject.GetComponent<AudioSource>();
        myAnim = gameObject.GetComponent<Animator>();
	}
	
	void Update () {
		

	}


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && getCoin == false)
        {
            myAudio.clip = clipGetCoin;
            myAudio.Play();

            Instantiate(parGetCoin, transform.position, transform.rotation);
            myAnim.Play("Coin_onGet");
            Destroy(gameObject, 2f);
            getCoin = true;

            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.addCoins(1);
        }
    }
}
