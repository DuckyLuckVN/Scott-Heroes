using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public Animator textAnim; 


	void Start () {
        AnimatorClipInfo[] clipInfo = textAnim.GetCurrentAnimatorClipInfo(0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
