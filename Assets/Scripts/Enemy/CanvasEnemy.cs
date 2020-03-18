using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasEnemy : MonoBehaviour {

    public Transform posUII;
    public Transform transCanvas;
    public Slider sliderHealth;

	void Start () {
        transCanvas.gameObject.SetActive(true);
	}
	
	void Update () {

        transCanvas.position = posUII.position;
        if(sliderHealth.value <= 0)
        {
            transCanvas.gameObject.SetActive(false);
            Destroy(gameObject, 3);
        }

	}
}
