using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour {

    public GameObject taget;
    public float smoothMove;

    Vector3 offset;
    float lowY;

	void Start () {
        offset = transform.position - taget.transform.position;

        lowY = transform.position.y;
		
	}
	

	void Update () {
        Vector3 campos = taget.transform.position + offset;

        transform.position = Vector3.Lerp(transform.position, campos, smoothMove * Time.deltaTime);

        if (transform.position.y != lowY)
        {
            transform.position = new Vector3(transform.position.x, lowY, transform.position.z);
        }

	}
}
