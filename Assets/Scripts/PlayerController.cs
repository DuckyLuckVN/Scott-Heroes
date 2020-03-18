using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float maxSpeed;
    public float jumpHeigh;
    public float myPower;
    public float moveCooldown;

    public Camera cameraMain;
    public Camera cameraRoom_1;
    public Transform shootPoint;
    public GameObject bulletArrow;
    public GameObject bulletJavelin;
    public GameObject uiDeath;
    public GameObject particleKnockout;
    public GameObject ImgPower;

    public AudioClip clipDeath;
    public AudioClip clipAttack_1;
    public AudioClip clipAttack_2;
    public AudioClip clipShoot;
    public AudioClip clipPowerCooldown;
    public AudioClip clipJump_1;
    public AudioClip clipJump_2;
    public AudioClip clipDamage_1;
    public AudioClip clipDamage_2;
    public AudioClip clipKnockout;

    public Slider myHealthSlider;
    public Slider myPowerSlider;
    public Text txtCoins;
    public Collider2D knockoutCollider;


    Rigidbody2D myBody;
    Animator myAnim;
    AudioSource myAudioSource;

    bool facingRight = true;
    bool stand = false;
    bool onTouch = false;
    bool arrowRight = false;
    bool arrowLeft = false;
    bool death = false;
    bool conditionMove = true;
    bool isknockout = false;
    bool test = false;

    float move;
    float phoneMove;
    float bulletCooldown;
    float timeCooldownMove;
    float coins;
    float healthOld;

    void Start () {
        Health myHealth = gameObject.GetComponent<Health>();

        myPowerSlider.maxValue = myPower;
        myPowerSlider.value = myPower;
        myHealthSlider.maxValue = myHealth.getHealth();
        myHealthSlider.value = myHealth.getHealth();
        knockoutCollider.enabled = false;

        myAnim = gameObject.GetComponent<Animator>();
        myBody = gameObject.GetComponent<Rigidbody2D>();
        myAudioSource = gameObject.GetComponent<AudioSource>();
        healthOld = myHealth.getHealth();
    }
	

	void Update () {
        if (Input.GetKeyDown(KeyCode.M))
        {
            test = !test;
            Debug.Log(test);
        }
        StartCoroutine(logTest());


        myAnim.SetBool("Stand", stand);
        knockoutAction();
        //---Health Manager---//
        Health myHealth = gameObject.GetComponent<Health>();

        if(myHealth.getHealth() < healthOld)
        {
            hitDamage();
        }

        txtCoins.text = coins.ToString();

        if(myHealth.getHealth() == 0)
        {
            Death();
        }

        //---DEATH ANIMATION---//
        if(death == true)
        {
            Time.timeScale -= Time.deltaTime;
        }

        //---KEY TEST SOMTHING---//
        if (Input.GetKeyDown(KeyCode.H))
        {
            knockOut();
        }


        if (arrowLeft == true && death == false)
        {
            phoneMove = -1;
        }

        if (arrowRight == true && death == false)
        {
            phoneMove = 1;
        }
        if(conditionMove == false)
        {
            move = 0;
            phoneMove = 0;
        }




        //---POWER MANAGER---//
        myPowerSlider.value = myPower;
        myHealthSlider.value = myHealth.getHealth();
        if(myPower < 100)
        {
            myPower += Time.deltaTime * 10.5f;
        }else if(myPower > 100)
        {
            myPower = 100;
        }


        //------Controller for Computer-----//

        if (Input.GetKeyDown(KeyCode.C) && death == false)
        {
            click_B();

        } else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) && death == false)
        {
            click_X();
        }
        else if (Input.GetKeyDown(KeyCode.V) && death == false)
        {
            shootJavelin();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && death == false)
        {
            myBody.AddForce(new Vector2(0, -500));
        }





        //----Condition Player Run----//
        if(onTouch == true && timeCooldownMove < Time.time)
        {
            move = phoneMove;
        }
            Run();


        if(move > 0 && facingRight == false && death == false)
        {
            flip();
        } else if(move < 0 && facingRight == true && death == false)
        {
            flip();
        }

        


    }

    void save()
    {
        if (move > 0 && !facingRight && death == false)
        {
            flip();
        }
        else if (move < 0 && facingRight && death == false)
        {
            flip();
        }


        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
    }

    void OnCollisionExit2D(Collision2D other)
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {


        if(other.gameObject.tag == "Block" || other.gameObject.tag == "Javelin")
        {
            if(move == 0 && death == false)
            {
                myAnim.SetBool("Moving", false);
            }
            else if(move != 0 && death == false)
            {
                //myAnim.Play("Player_Run");
                myAnim.SetBool("Moving", true);
            }
            stand = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
    }

    private void OnTriggerStay2D(Collider2D other)
    {
    }



    //---PLAYER ACTION---//
    void flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
   
    }

    void Run()
    {
        if (conditionMove)
        {
            if (onTouch == false && death == false)
            {
                if (timeCooldownMove < Time.time)
                {
                    move = Input.GetAxisRaw("Horizontal");
                }
            }
        }

        myBody.velocity = new Vector2(move * maxSpeed, myBody.velocity.y);

        if(move != 0)
        {
            myAnim.SetBool("Moving", true);
        }
        else
        {
            myAnim.SetBool("Moving", false);
        }
    }

    public void Jump()
    {
        if(stand == true && death == false && isknockout == false)
        {
            float nRandom = Random.Range(1, 3);
            if (nRandom == 1)
            {
                myAudioSource.clip = clipJump_1;
            }
            if (nRandom == 2)
            {
                myAudioSource.clip = clipJump_2;
            }


            myBody.velocity = new Vector2(myBody.velocity.x, jumpHeigh);
            myAnim.Play("Player_Jump_1");
            stand = false;
            //myAudioSource.clip = clipJump;
            myAudioSource.Play();
        }
    }

    public void shootArrow()
    {

        float powShoot = 5;
        //--ANIMATION WHEN SHOOT--//
        if (stand == true && myPower >= powShoot)
        {
            myAnim.SetBool("Stand", stand);
            myAnim.Play("Player_Shoot");
        }
        else if (stand == false && myPower >= powShoot)
        {
            myAnim.SetBool("Stand", stand);
            myAnim.Play("Boy_Fight");
        }
        if (myPower < powShoot)
        {
            playAudio(clipPowerCooldown);
        }

        if (myPower >= powShoot)
        {
            float numRandomAttack = Random.Range(1, 3);
            if (numRandomAttack == 1)
            {
                myAudioSource.clip = clipAttack_1;
            }
            else if (numRandomAttack == 2)
            {
                myAudioSource.clip = clipAttack_2;
            }

            if (facingRight)
            {
                myPower -= powShoot;
                Instantiate(bulletArrow, shootPoint.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                myAudioSource.Play();
            }
            else if (!facingRight)
            {
                myPower -= powShoot;
                Instantiate(bulletArrow, shootPoint.transform.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                myAudioSource.Play();
            }

        }
    }

    public void shootJavelin()
    {
        float powShoot = 50;
        //--ANIMATION WHEN SHOOT--//
        if (stand == true && myPower >= powShoot)
        {
            myAnim.SetBool("Stand", stand);
            myAnim.Play("Player_Shoot");
        }
        else if (stand == false && myPower >= powShoot)
        {
            myAnim.SetBool("Stand", stand);
            myAnim.Play("Boy_Fight");
        }

        if (myPower >= powShoot)
        {
            float numRandomAttack = Random.Range(1, 3);
            if (numRandomAttack == 1)
            {
                myAudioSource.clip = clipAttack_1;
            }
            else if (numRandomAttack == 2)
            {
                myAudioSource.clip = clipAttack_2;
            }

            if (facingRight)
            {

                myPower -= powShoot;
                Instantiate(bulletJavelin, shootPoint.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                myAudioSource.Play();
            }
            else if (!facingRight)
            {
                myPower -= powShoot;
                Instantiate(bulletJavelin, shootPoint.transform.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                myAudioSource.Play();
            }
        }else
        {
            playAudio(clipPowerCooldown);
        }
    }
    public void knockOut()
    {
        if(myPower > 50 && stand == true && isknockout == false && death == false)
        {
            myAnim.Play("Player_Knockout");
            StartCoroutine(knockoutAction());
        }
    }

    //----PHOME CONTROLLER---//
    public void clickLeftArrow()
    {
        if(death == false)
        {
            onTouch = true;
            arrowLeft = true;
        }

    }

    public void clickRightArrow()
    {
        if (death == false)
        {
            onTouch = true;
            arrowRight = true;
        }
    }

    public void deSelect()
    {
            phoneMove = 0;
            onTouch = false;
        if(arrowLeft == true)
        {
            arrowLeft = false;
        }

        if (arrowRight == true)
        {
            arrowRight = false;
        }
    }

    public void click_X()
    {
        if (death == false)
        {
            Jump();
        }
    }

    public void click_B()
    {
        if (death == false)
        {
            shootArrow();
        }
    }

    public void click_O()
    {
        if(death == false)
        {
            shootJavelin();
        }

    }

    

    /* public void addDamage(float damage)
    {

        if(damage > myHealth && death == false)
        {
            myAnim.Play("Player_Death");
            playAudio(clipDeath);
            death = true;
            move = 0;
            phoneMove = 0;
            uiDeath.GetComponent<Animator>().Play("Panel_Death_Death");
            myHealth = 0;
        }
        else if(death == false)
        {
            myHealth -= damage;
            timeCooldownMove = moveCooldown + Time.time;
            move = 0;
            myAnim.Play("Player_Damage");

            float randomNumber = Random.Range(1, 3);
            if (randomNumber == 1)
            {
                playAudio(clipDamage_1);
            }
            else if (randomNumber == 2)
            {
                playAudio(clipDamage_2);
            }
        }
    } */

    public void Death()
    {
        if(death == false)
        {
            myAnim.Play("Player_Death");
            playAudio(clipDeath);
            death = true;
            move = 0;
            phoneMove = 0;
            uiDeath.GetComponent<Animator>().Play("Panel_Death_Death");
        }
    }

    public void hitDamage()
    {
        Health myHealth = gameObject.GetComponent<Health>();

        healthOld = myHealth.getHealth();
        if (death == false)
        {
            timeCooldownMove = moveCooldown + Time.time;
            move = 0;
            myAnim.Play("Player_Damage");
            //---RANDOM CLIP DAMAGE---//
            float randomNumber = Random.Range(1, 3);
            if (randomNumber == 1)
            {
                playAudio(clipDamage_1);
            }
            else if (randomNumber == 2)
            {
                playAudio(clipDamage_2);
            }
        }
        knockoutCollider.enabled = false;
    }

    public void reloadSence()
    {
        Application.LoadLevel(0);
        Time.timeScale = 1;
    }

    void playAudio(AudioClip clip)
    {
        myAudioSource.clip = clip;
        myAudioSource.Play();
    }

    public void addCoins(float coin)
    {
        coins += coin;
    }

    IEnumerator knockoutAction()
    {
        conditionMove = false;
        myPower -= 5;
        isknockout = true;
        if (isknockout == true)
        {
            yield return new WaitForSeconds(0.15f);
            if (facingRight)
            {
                Instantiate(particleKnockout, knockoutCollider.transform.position, knockoutCollider.transform.rotation);
            }
            else
            {
                GameObject particle = Instantiate(particleKnockout, knockoutCollider.transform.position, knockoutCollider.transform.rotation);
                particle.transform.localScale = new Vector3(-1,1,1);
            }
            
            knockoutCollider.enabled = true;
            playAudio(clipKnockout);
            yield return new WaitForSeconds(0.15f);
            knockoutCollider.enabled = false;
            conditionMove = true;
            isknockout = false;
        }
    }

    IEnumerator logTest()
    {
        while (test)
        {
            Debug.Log("Okay");
            yield return null;
        }

    }
}
