using UnityEngine;
using System.Collections;

public class Lock : MonoBehaviour {


    Transform camera ;	// Use this for initialization
    Quaternion rot;
    Quaternion rot2;
    Transform hpbar;	// Use this for initialization
    Transform shieldbar;	// Use this for initialization

    void Start () {
        camera = GameObject.FindGameObjectWithTag("MainCamera").transform;

        hpbar = transform.Search("Hpbar");
        shieldbar = transform.Search("ShieldBar");

    }
	
	// Update is called once per frame
	void Update () {
      //  rot2 = transform.localRotation;
        rot = Quaternion.Euler(90f, camera.rotation.eulerAngles.y, 0f);
        transform.localRotation = Quaternion.Euler(-transform.parent.rotation.eulerAngles + rot.eulerAngles);
       // hpbar.rotation = rot2;
        //shieldbar.rotation = rot2;

    }
}
