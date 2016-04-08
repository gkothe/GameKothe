using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Infos : MonoBehaviour {

	public float health;
	public float shield;
    public int id;
    public int atkmin = 10; //dano da arma
    public int atkmax = 30; //dano da arma
    public float  SP = 0.5f; //dano da arma
    public int baseprecision = 30;  //piloto e armna 
    public float evademod = 10;
    public string shipcript;

	void Start(){
		id = GM.getIdparanave();
	}


    public float precision(GameObject alvo, int qtd_linha){

        Infos info = alvo.GetComponent<Infos>();

        float perc = 10 + ((qtd_linha / 50 )*10) + baseprecision  - info.evademod; //busca do alvo, busca os redutor

        if (perc > 95)
            perc = 95;

        return perc;
    }


    public Dictionary<string, float>   ataque(GameObject alvo, float prec)
    {

        //teste pra ve se acerta
        float danohull = 0;
        float danoshield = 0;
        float dano = 0;
        Infos info = alvo.GetComponent<Infos>();
        Dictionary<string, float> danos = new Dictionary<string, float>();



        if (Random.Range(0, 100f) < prec) {

            dano = Random.Range(atkmin, atkmax);
            danohull = dano * (SP/10);
            danoshield = dano - danohull;
            info.health = info.health - danohull;
            info.shield = info.shield - danoshield;

      
          
        }


        danos.Add("danohull", danohull);
        danos.Add("danoshield", danoshield);


        return danos;

    }




}
