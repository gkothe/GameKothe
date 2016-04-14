using UnityEngine;
using System.Collections;

public class AsteroidClass : MonoBehaviour {

    public float danoAsteroide = 20f;
    protected GM gm;
    
    void Start () {
        gm = GameObject.FindWithTag("GameController").GetComponent<GM>() as GM;
    }
	
	void Update () {
	
	}
    

    public void damageShip(GameObject obj) {

        ShipScript objShip = obj.GetComponent<ShipScript>();
        Infos objInfo = obj.GetComponent<Infos>();
        objShip.setHealth(objInfo.health - danoAsteroide);
        objShip.acao_able = false;

        gm.AtualizaInfo();

    }

    void OnTriggerEnter(Collider other)
    {
            if (other.CompareTag ("Ship"))   {
                damageShip(other.gameObject);
            }
       
    }

}
