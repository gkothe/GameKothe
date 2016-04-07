using UnityEngine;
using System.Collections;

public class Infos : MonoBehaviour {

	public float health;
	public float shield;
    
    public int id;
	public string shipcript;

	void Start(){
		id = GM.getIdparanave();
	}
}
