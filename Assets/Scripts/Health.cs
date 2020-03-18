using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public float health;
    private float damage;

    //--Get and Set Health--//
    public float getHealth()
    {
        return this.health;
    }

    public void setHealth(float num)
    {
        this.health = num;
    }
    
    //--Get and Set Damage--//
    public float getDamage()
    {
        return damage;
    }

    public void setDamage(float damage)
    {
        this.damage = damage;
    }


	void Start () {

		
	}
	

	void Update () {

		
	}

    public void addDamage(float damage)
    {

        if(health > 0)
        {
            health -= damage;
            if (damage > health)
            {
                health = 0;
            }
        }

    }
}
