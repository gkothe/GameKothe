using UnityEngine;
using System.Collections;

public class Lock : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        Quaternion rot = Quaternion.Euler(90f,0f,0f);

        transform.localRotation = Quaternion.Euler(-transform.parent.rotation.eulerAngles + rot.eulerAngles);


    }
}
