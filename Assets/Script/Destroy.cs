using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Invoke("Die", 1f);

    }

    void Die()
    {
        Destroy(gameObject);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
