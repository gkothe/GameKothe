using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;


public class Infos : MonoBehaviour
{


    
    public string nome_armaObjeto;
    public string nome_pilotoObjeto;
    public float health; //TODO, buscar na nave
    public float shield;//TODO, buscar na nave
    public int player;
    public int id;
    public int atkmin; //dano da arma
    public int atkmax; //dano da arma
    public int skillpiloto; 
    public float SP; //penetração da arma
    public float baseprecision;  //piloto e armna 
    public float evademod = 20; //TODO
    public string shipcript; //TODO, como alimentar? acho q vai ser pelo GM qdo iniciar o mapa
    public string nome_arma;
    public string nome_piloto;

    void carregaComponentes() {

        MethodInfo theMethod;
        Type componente;
        Component comp;

        //dados arma
        componente = Type.GetType(nome_armaObjeto);
        gameObject.AddComponent(componente);
        comp = GetComponent(componente);  // Selected Piece    

        theMethod = componente.GetMethod("iniStats");
        theMethod.Invoke(comp, null);

        theMethod = componente.GetMethod("getAtkmin");
        atkmin = (int)theMethod.Invoke(comp, null);

        theMethod = componente.GetMethod("getAtkmax");
        atkmax = (int)theMethod.Invoke(comp, null);

        theMethod = componente.GetMethod("getSP");
        SP = (float)theMethod.Invoke(comp, null);

        theMethod = componente.GetMethod("getBaseprecision");
        baseprecision = (float)theMethod.Invoke(comp, null);

        theMethod = componente.GetMethod("getNomeArma");
        nome_arma = (string)theMethod.Invoke(comp, null);


        componente = Type.GetType(nome_pilotoObjeto);
        gameObject.AddComponent(componente);
        comp = GetComponent(componente);  // Selected Piece  

        theMethod = componente.GetMethod("iniStats");
        theMethod.Invoke(comp, null);

        theMethod = componente.GetMethod("getNomePiloto");
        nome_piloto = (string)theMethod.Invoke(comp, null);

        theMethod = componente.GetMethod("getBaseprecision");
        baseprecision = baseprecision + (float)theMethod.Invoke(comp, null);

        theMethod = componente.GetMethod("getSkillpiloto");
        skillpiloto = (int)theMethod.Invoke(comp, null);

    }
    void Start()
    {
        id = GM.getIdparanave();

        carregaComponentes();

    }












}
