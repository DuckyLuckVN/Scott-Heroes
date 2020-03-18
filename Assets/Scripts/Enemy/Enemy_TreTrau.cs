using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_TreTrau : MonoBehaviour
{

    public Transform pointShoot;
    public GameObject objBullet;
    public Transform transPlayer;
    public Transform transYPlayer;
    public GameObject parDeath;

    public Slider sliderHealth;

    public AudioClip clipHit;
    public AudioClip clipDeath;

    public float speedMove;
    public float rangeCatch;
    public float rangeAttack;
    public float timeDelayAttack;
    public float DamageArrow;
    public float damageJavelin;

    Animator myAnim;
    Rigidbody2D myBody;
    AudioSource myAudio;

    bool facingRight = false;
    bool canMove = true;
    bool isRuning = false;
    bool hasDeath = false;
    float distancePlayer;
    float distanceYPlayer;
    float timeCooldownAttack;
    float oldHealth;

    Vector3 lastPos;

    void Start()
    {
        Health myHealth = gameObject.GetComponent<Health>();

        sliderHealth.maxValue = myHealth.getHealth();
        sliderHealth.value = myHealth.getHealth();

        myAudio = gameObject.GetComponent<AudioSource>();
        myAnim = gameObject.GetComponent<Animator>();
        myBody = gameObject.GetComponent<Rigidbody2D>();
        lastPos = transform.position;
        oldHealth = myHealth.getHealth();
    }


    void Update()
    {
        Health myHealth = gameObject.GetComponent<Health>();
        if(myHealth.getHealth() < oldHealth)
        {
            hitDamage();
        }

        //--------UPDATE ALL TIME------------//
        sliderHealth.value = myHealth.getHealth();
        distancePlayer = Vector3.Distance(transform.position, transPlayer.position);
        transYPlayer.position = new Vector3(transYPlayer.position.x, transPlayer.position.y, 0);



        //----DIE EVENT----//

        //----CHECK RANGE ATTACK----//
        if (timeCooldownAttack < Time.time && distancePlayer < rangeAttack && hasDeath == false)
        {
            thorw();
            timeCooldownAttack = Random.Range(timeDelayAttack - 0.9f, timeDelayAttack) + Time.time;
        }

        if(transform.position.y > transYPlayer.position.y && hasDeath == false)
        {
            distanceYPlayer = -Vector3.Distance(transform.position, transYPlayer.position);
        } else if(transform.position.y < transYPlayer.position.y && hasDeath == false)
        {
            distanceYPlayer = Vector3.Distance(transform.position, transYPlayer.position);
        }


        //-----CONDITION CAN MOVE-----//
        if (distancePlayer < rangeCatch && distancePlayer > rangeCatch - 3 && canMove == true && hasDeath == false)
        {
            myAnim.SetBool("Moving", true);
            run();
            isRuning = true;

        }
        if (distancePlayer > rangeCatch || distancePlayer <= rangeCatch - 3 || canMove == false && isRuning == true && hasDeath == false) 
        {
            //transform.position = lastPos;
            myAnim.SetBool("Moving", false);
            isRuning = false;
        }





        if(transPlayer.position.x < transform.position.x && facingRight == true && hasDeath == false)
        {
            flip();
        } else if (transPlayer.position.x > transform.position.x && facingRight == false && hasDeath == false)
        {
            flip();
        }

    }

    void thorw()
    {
        if(hasDeath == false)
        {
            float thorwX = distancePlayer * 60.33333f;
            float thorwY = 200 + distanceYPlayer * 79.5f;

            GameObject a = Instantiate(objBullet, pointShoot.position, Quaternion.Euler(0, 0, 0));

            if (transPlayer.position.x > transform.position.x)
            {
                a.GetComponent<Rigidbody2D>().AddForce(new Vector2(thorwX, thorwY));
                a.GetComponent<Animator>().Play("Bullet_TreTrau_facingRight");
            }
            else
            {
                a.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1 * thorwX, thorwY));
            }

            myAnim.Play("TreTrau_Fight");
        }

    }

    void flip()
    {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;

    }


    void run()
    {

        if (transPlayer.position.x > transform.position.x)
        {
            myBody.velocity = new Vector2(speedMove, myBody.velocity.y);
        }
        else if (transPlayer.position.x < transform.position.x)
        {
            myBody.velocity = new Vector2(-speedMove, myBody.velocity.y);
        }
        lastPos = transform.position;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Block")
        {
            canMove = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Block")
        {
            canMove = false;
            lastPos = transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //----Player va chạm với Enemy----//
        if(other.gameObject.tag == "Player" && hasDeath == false)
        {
            Health playerHealth = other.gameObject.GetComponent<Health>();
            playerHealth.addDamage(5);

            if(transPlayer.position.x < transform.position.x)
            {
                other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0 , 6);
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-2000, 0));
            }
            else
            {
                other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 6);
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(2000,0));
            }

        }


    }

    void Death()
    {
        if (hasDeath == false)
        {
            hasDeath = true;
            //myAnim.SetBool("Death", true);
            myAnim.Play("TreTrau_Death");
            playAudio(clipDeath);
            Instantiate(parDeath, transform.position, transform.rotation);
            Debug.Log("OK");
        }
    }

    void hitDamage()
    {
        Health myHealth = gameObject.GetComponent<Health>();
        if (myHealth.getHealth() <= 0 && hasDeath == false)
        {
            Death();
        }
        else if (myHealth.getHealth() > 0)
        {
            oldHealth = myHealth.getHealth();
            myAnim.Play("TreTrau_Damage");
            playAudio(clipHit);
        }
    }

    void playAudio(AudioClip clip)
    {
        myAudio.clip = clip;
        myAudio.Play();
    }
}
