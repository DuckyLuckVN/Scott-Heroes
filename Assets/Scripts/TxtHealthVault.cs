using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxtHealthVault : MonoBehaviour {

    public Slider sliderHealth;

    float healthVault;
    Text myTxt;

	void Start () {

        myTxt = gameObject.GetComponent<Text>();

	}
	

	void Update () {

       healthVault = sliderHealth.value;
        myTxt.text = healthVault.ToString();
		
	}
}
