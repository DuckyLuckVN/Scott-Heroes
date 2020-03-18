using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThuNghiem : MonoBehaviour {

    bool Return = false;

    IEnumerator Start () {

        Debug.Log(Time.time);
        yield return StartCoroutine(Log());
    }

    // Update is called once per frame
    void Update () {

    }

    IEnumerator Log()
    {
        Debug.Log("Abc");
        yield return new WaitForSeconds(0);
    }



    
}
