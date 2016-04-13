using UnityEngine;
using System.Collections;

public class Lock : MonoBehaviour {


    Transform camera ;	// Use this for initialization
    Quaternion rot;

    void Start () {
        camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }
	
	// Update is called once per frame
	void Update () {
        
        rot = Quaternion.Euler(90f, camera.rotation.eulerAngles.y, 0f);
        transform.localRotation = Quaternion.Euler(-transform.parent.rotation.eulerAngles + rot.eulerAngles);


    }
}
