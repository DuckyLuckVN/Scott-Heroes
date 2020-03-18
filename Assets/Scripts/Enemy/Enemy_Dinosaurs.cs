using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Dinosaurs : MonoBehaviour {

    public Transform taget;
    public Transform pointYPlayer;
    public GameObject particleDeath;
    public Slider sliderHeathBar;
    public float moveSpeed;
    public float damage;
    public float maxDisCatch;
    public float minDisCatch;
    public float timeDelayAttack;

    public AudioClip clipAttack;
    public AudioClip clipHit;
    public AudioClip clipDeath;

    public Collider2D pointAttack;

    AudioSource myAudio;
    Rigidbody2D mybody;
    Animator myAnim;
    SpriteRenderer mySprite;

    float tagetDistance;
    float tagetYDistance;
    float scaleX;
    float oldHealth;
    float move;

    bool isAttack = false;
    bool facingRight = false;
    bool ground = true;
    bool canMove = true;
    bool death = false;
    bool isHit = false;

	void Start () {
        Health health = GetComponent<Health>();
        mySprite = gameObject.GetComponent<SpriteRenderer>();
        pointAttack.enabled = false;
        myAudio = GetComponent<AudioSource>();
        mybody = GetComponent<Rigidbody2D>();
        myAnim = gameObject.GetComponent<Animator>();
        scaleX = transform.localScale.x;
        sliderHeathBar.maxValue = health.getHealth();
        sliderHeathBar.value = health.getHealth();
        oldHealth = health.getHealth();

        taget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
	
	void Update () {

        if(myAnim.GetCurrentAnimatorStateInfo(0).IsName("Dinosaurs_Death_2"))
        {
            mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, mySprite.color.a - 0.01f);
        }

        //----HEALTH MANAGGER---//
        Health health = GetComponent<Health>();
        if(oldHealth != health.getHealth())
        {
            oldHealth = health.getHealth();
            StartCoroutine(Hit());
            
        }
        sliderHeathBar.value = health.getHealth();


        pointYPlayer.transform.position = new Vector3(pointYPlayer.transform.position.x, taget.position.y, pointYPlayer.transform.position.z);
        facingRight = transform.position.x < taget.position.x;


        //-----FLIP----//
        if (facingRight && isAttack == false && death == false)
        {
            transform.localScale = new Vector3(-scaleX, transform.localScale.y, transform.localScale.z);
        }
        else if(!facingRight && isAttack == false && death == false)
        {
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        }

        //----CONDITION MOVE - ATTACK---//
        tagetDistance = Vector3.Distance(transform.position, taget.transform.position);
        if(tagetDistance < maxDisCatch + 1.1 * Mathf.Abs(transform.localScale.x) && tagetDistance >= minDisCatch && ground == true && isAttack == false)
        {
            //-----Dieu kien do cao---//
            if(Vector3.Distance(pointYPlayer.position, transform.position) < 1 && Vector3.Distance(pointYPlayer.position, transform.position) > -0.5f)
            {
                run();
                myAnim.SetBool("Move", true);
            }
        }
        else
        {
            if(ground == true)
            {
                mybody.velocity = new Vector2(0, mybody.velocity.y);
            }
            move = 0;
            myAnim.SetBool("Move", false);

            //----Condition Attack----//
            if(isAttack == false 
                && tagetDistance > minDisCatch - 1 
                && tagetDistance < minDisCatch + transform.localScale.y 
                && move == 0 && Vector3.Distance(pointYPlayer.position, transform.position) < 1 * transform.localScale.y 
                && Vector3.Distance(pointYPlayer.position, transform.position) > -0.5f)
            {
                myAnim.Play("Dinosaurs_Attack");
                playAudio(clipAttack);
                
            }
        }

    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.tag == "Block")
        {
            ground = false;
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Block")
        {
            ground = true;
            if (death)
            {
                mybody.simulated = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player" && death == false)
        {
            Health health = other.gameObject.GetComponent<Health>();
            health.addDamage(damage);

            if (taget.position.x < transform.position.x)
            {
                other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 6);
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-2000, 0));
            }
            else
            {
                other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 6);
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(2000, 0));
            }
        }
    }



    void run()
    {
        if (canMove && death == false)
        {
            if (!facingRight)
            {
                move = -moveSpeed;
            }
            else
            {
                move = moveSpeed;
            }
            mybody.velocity = new Vector2(move + Time.deltaTime, mybody.velocity.y);
            myAnim.Play("Dinosaurs_Runing");
        }
    }


    void Death()
    {
        myAnim.Play("Dinosaurs_Death");
        playAudio(clipDeath);
    }

    void playAudio(AudioClip clip)
    {
        myAudio.clip = clip;
        myAudio.Play();
    }

    void delayMoveOnHit()
    {
        
    }

    public IEnumerator Attack()
    {
        if(isAttack == false && death == false)
        {
            isAttack = true;
           // myAnim.Play("Dinosaurs_Attack");
           // yield return new WaitForSeconds(0.1f);
           // yield return new WaitForSeconds(0.15f);
            pointAttack.enabled = true;
            yield return new WaitForSeconds(1.15f);
            pointAttack.enabled = false;
            isAttack = false;
        }

    }

    IEnumerator Hit()
    {
        Health health = GetComponent<Health>();
        if(health.getHealth() > 0)
        {
            isHit = true;
            canMove = false;
            myAnim.Play("Dinosaurs_Hit");
            playAudio(clipHit);
            yield return new WaitForSeconds(0.45f);
            canMove = true;
            isHit = false;
        }
        else
        {
            death = true;
            Instantiate(particleDeath, transform.position, transform.rotation);
            Death();
        }
    }
}
