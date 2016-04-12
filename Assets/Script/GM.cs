using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
    [HideInInspector]
    public int gameState = 0;
    public Dictionary<string, int> gamestates;
    public Camera PlayerCam;
    public GameObject board;
    public static Button btnGo;
    public static Button btnIniciaFazeMov;
    public static Button btnShoot;
    public static Button btnChecktarget;
    public Dropdown dropMovimento;
    private GameObject SelectedPiece;
    private Component SelectedPiece_script;
    private GameObject SelectedPieceTarget;
    public ArrayList naves_targets = new ArrayList();
    public Dictionary<int, ArrayList> ordem_naves;
    public ArrayList naves_jamoveram = new ArrayList();
    private Type script; //o tipo é pego quando seleciona a nave

    private Text infos_selected;
    public int maiorSkillPiloto = 1;  //maior skill de piloto em jogo, o menor skill SEMPRE será 1.

    public int proxid_nave = 0; //gerador automatico para id das naves
    public int getIdparanave()
    {
        proxid_nave = proxid_nave + 1;
        return proxid_nave;
    }
    // Use this for initialization
    void Start()
    {

        infos_selected = GameObject.Find("Info_selected").GetComponent<Text>();
        btnGo = GameObject.Find("btnGO").GetComponent<Button>() as Button;
        btnShoot = GameObject.Find("btnShoot").GetComponent<Button>() as Button;
        btnIniciaFazeMov = GameObject.Find("btnIniMovimento").GetComponent<Button>() as Button;
        btnChecktarget = GameObject.Find("btnCheckTargets").GetComponent<Button>() as Button;
        PlayerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        iniGamestates();
        ChangeGameState("escolhe_movimento");

    }

    private void iniGamestates()
    {

        gamestates = new Dictionary<string, int>();
        gamestates.Add("escolhe_movimento", 0);
        gamestates.Add("realiza_movimento", 3);

        gamestates.Add("fase_tiro", 10);
        gamestates.Add("fase_tiro_2", 12);

    }

    public void ChangeGameState(string state)
    {

        int gmstate = gamestates[state];

        if (state.Equals("escolhe_movimento"))
        {
            limpaNavesObjetos();

            btnGo.enabled = true;
            btnShoot.enabled = false;
            btnChecktarget.enabled = false;
            btnIniciaFazeMov.enabled = true;

        }
        else if (state.Equals("fase_tiro"))
        {

            btnGo.enabled = false;
            btnShoot.enabled = true;
            btnChecktarget.enabled = true;
            btnIniciaFazeMov.enabled = false;



        }
        else if (state.Equals("realiza_movimento"))
        {
            btnGo.enabled = true;
            btnShoot.enabled = false;
            btnChecktarget.enabled = false;
            btnIniciaFazeMov.enabled = false;


        }


        gameState = gmstate;

    }

    // Update is called once per frame
    void Update()
    {
        GetMouseInputs();
    }

    void GetMouseInputs()
    {
        Ray _ray;
        RaycastHit _hitInfo;


        if (gameState == gamestates["escolhe_movimento"])
        {
            if (Input.GetMouseButtonDown(0))
            {
                _ray = PlayerCam.ScreenPointToRay(Input.mousePosition); // Specify the ray to be casted from the position of the mouse click
                if (Physics.Raycast(_ray, out _hitInfo))
                {
                    if (_hitInfo.collider.gameObject.tag == ("Ship"))
                    {
                        SelectPiece(_hitInfo.collider.gameObject);
                    }
                    else {
                        SelectPiece(null);
                    }
                }
            }
        }


        else if (gameState == gamestates["realiza_movimento"] || gameState == gamestates["fase_tiro"])
        {//seleciona pra atirar ou para movimentar
            if (Input.GetMouseButtonDown(0))
            {
                _ray = PlayerCam.ScreenPointToRay(Input.mousePosition); // Specify the ray to be casted from the position of the mouse click

                // Raycast and verify that it collided
                if (Physics.Raycast(_ray, out _hitInfo))
                {
                    if (_hitInfo.collider.gameObject.tag == ("Ship"))
                    {
                        if (_hitInfo.collider.gameObject.GetComponent<ShipScript>().ativo_MovAtk)
                        {
                            SelectPiece(_hitInfo.collider.gameObject);
                        }

                    }
                    else {
                        SelectPiece(null);
                    }
                }

            }
        }


        else if (gameState == gamestates["fase_tiro_2"])
        {//tiro

            if (Input.GetMouseButtonDown(0))
            {
                _ray = PlayerCam.ScreenPointToRay(Input.mousePosition); // Specify the ray to be casted from the position of the mouse click

                // Raycast and verify that it collided
                if (Physics.Raycast(_ray, out _hitInfo))
                {

                    if (_hitInfo.collider.gameObject.tag == ("Ship"))
                    {

                        SelectPieceTarget(_hitInfo.collider.gameObject);

                    }
                    else {
                        SelectPieceTarget(null);
                    }
                }

            }
        }

    }


    public int skill_ativo = 0;  //skill ativo na fase de movimento ou combate


    #region fases

    public void prepFaseOrdem() //prepara a ordem de movimentação dos pilotos a partir do skill, skill , serve tb
    {

        GameObject ship;
        GameObject[] ships = GameObject.FindGameObjectsWithTag("Ship");
        Infos info;
        ordem_naves = new Dictionary<int, ArrayList>();
        ArrayList conjunto_naves;
        maiorSkillPiloto = 0;

        for (int x = 0; x < ships.Length; x++)
        {
            ship = ships[x];
            info = ship.GetComponent<Infos>();
            maiorSkillPiloto = info.skillpiloto > maiorSkillPiloto ? info.skillpiloto : maiorSkillPiloto;
        }

        for (int x = 1; x <= maiorSkillPiloto; x++)
        {
            conjunto_naves = new ArrayList();

            for (int p = 0; p < ships.Length; p++)
            {
                ship = ships[p];
                info = ship.GetComponent<Infos>();
                if (info.skillpiloto == x)
                {
                    conjunto_naves.Add(ship);
                }

            }
            ordem_naves.Add(x, conjunto_naves);

        }

        if (gameState == gamestates["escolhe_movimento"])
        {
            ChangeGameState("realiza_movimento");
            skill_ativo = 1;
            FaseMovimento();
        }
        else if (gameState == gamestates["fase_tiro"])
        {
            skill_ativo = maiorSkillPiloto;
            faseTiro();

        }


    }

    public void FaseMovimento()
    {
        GameObject ship;
        ArrayList conjunto_naves;

        ShipScript shipClass;
        bool tem_nave_para_mover = false;

        if (skill_ativo > maiorSkillPiloto)
        {

            limpaNavesObjetos();
            ChangeGameState("fase_tiro");
            prepFaseOrdem();

            //fase de tiro de cada nave;
        }
        else {
            conjunto_naves = ordem_naves[skill_ativo];

            for (int x = 0; x < conjunto_naves.Count; x++)
            {
                ship = (GameObject)conjunto_naves[x];
                if (!naves_jamoveram.Contains(ship))
                {
                    ship.GetComponent<Renderer>().material.color = Color.green;

                    shipClass = ((ShipScript)ship.GetComponent<ShipScript>());
                    shipClass.ativo_MovAtk = true;
                    shipClass.texto1.text = "Mover";
                    shipClass.texto1.color = Color.green;
                    tem_nave_para_mover = true;
                }
            }

            if (!(tem_nave_para_mover) && skill_ativo <= maiorSkillPiloto)
            {
                skill_ativo++;
                FaseMovimento();
            }

            else {
                Debug.Log("Deu treta.");
            }
        }

    }


    public void faseTiro()
    {


        GameObject ship;
        ArrayList conjunto_naves;

        ShipScript shipClass;
        bool tem_nave_para_atirar = false;

        if (skill_ativo == 0)
        {

            limpaNavesObjetos();
            ChangeGameState("escolhe_movimento");

        }
        else {

            conjunto_naves = ordem_naves[skill_ativo];

            for (int x = 0; x < conjunto_naves.Count; x++)
            {
                ship = (GameObject)conjunto_naves[x];
                if (!naves_jamoveram.Contains(ship))
                {
                    ship.GetComponent<Renderer>().material.color = Color.green;

                    shipClass = ((ShipScript)ship.GetComponent<ShipScript>());
                    shipClass.ativo_MovAtk = true;
                    shipClass.texto1.text = "Atirar";
                    shipClass.texto1.color = Color.red;
                    tem_nave_para_atirar = true;
                }
            }

            if (!(tem_nave_para_atirar) && skill_ativo > 0)
            {
                skill_ativo--;
                faseTiro();
            }
        }
    }

    #endregion


    #region botoes

    [HideInInspector]

    public void Botao_Go()
    {

        if (gameState == gamestates["realiza_movimento"])
        {
            if (SelectedPiece != null)
            {
                MethodInfo theMethod = script.GetMethod("movimento");
                theMethod.Invoke(SelectedPiece_script, null);
            }
        }

    }


    public void Botao_iniFazeMov()
    {
        if (gameState == gamestates["escolhe_movimento"])
        {
            limpaNavesObjetos();//cuidar para nao limpar o movimento armazenado
            prepFaseOrdem();
        }
    }

    public void storeMovimento()
    {
        if (SelectedPiece != null && gamestates["escolhe_movimento"] == gameState)
        {
            SelectedPiece.GetComponent<ShipScript>().setMovimento(dropMovimento.value);

        }

    }

    public void Botao_Shoot()
    {
        if (gameState == gamestates["fase_tiro"])
        {
            if (SelectedPiece != null)
            {
                MethodInfo theMethod = script.GetMethod("Shoot");
                object[] parametersArray = new object[] { SelectedPieceTarget };
                theMethod.Invoke(SelectedPiece_script, parametersArray);
                SelectPieceTarget(null);
            }
        }

    }


    public void Botao_CheckTargets()
    {

        if (SelectedPiece != null)
        {
            MethodInfo theMethod = script.GetMethod("Tiro_basic");
            theMethod.Invoke(SelectedPiece_script, null);

        }


    }

    #endregion


    #region UI

    public void limpaNavesObjetos()
    {
        GM gm = GameObject.Find("GM").GetComponent<GM>() as GM;
        gm.SelectPiece(null);
        gm.SelectPieceTarget(null);
        gm.SelectedPiece_script = null;
        gm.script = null;
        gm.naves_targets = null;
        GameObject ship;
        Component scriptnave;
        GameObject[] ships = GameObject.FindGameObjectsWithTag("Ship");
        gm.naves_jamoveram = new ArrayList();
        gm.ordem_naves = new Dictionary<int, ArrayList>();

        ShipScript shipScriptObj;
        Type script;
        for (int x = 0; x < ships.Length; x++)
        {
            ship = ships[x];
            shipScriptObj = ((ShipScript)ship.GetComponent<ShipScript>());
            script = Type.GetType(shipScriptObj.namescript);
            scriptnave = ship.GetComponent(script);
            MethodInfo theMethod = script.GetMethod("cleanStuff");
            theMethod.Invoke(scriptnave, null);

            //((ShipScript)ship.GetComponent<ShipScript>()).Uiship.SetActive(false);
            shipScriptObj.texto1.text = "";
            shipScriptObj.ativo_MovAtk = false;


        }

    }

    private void AtualizaInfo()
    {
        Infos inf = SelectedPiece_script.GetComponent<Infos>();
        String text = "";
        text = "Id: " + inf.id + "\n";
        text = text + "Piloto: " + inf.nome_piloto + "\n";
        text = text + "Skill Piloto: " + inf.skillpiloto + "\n";
        text = text + "Health: " + inf.health + "\n";
        text = text + "Shield: " + inf.shield + "\n";
        text = text + "Weapon: " + inf.nome_arma + "\n";
        text = text + "Damage: " + inf.atkmin + " ~ " + inf.atkmax + "\n";
        text = text + "SP: " + inf.SP + "% \n";
        text = text + "Evade: " + inf.evademod + "\n";
        text = text + "Precisao Mod: " + inf.baseprecision + "\n";


        infos_selected.text = text;
    }

    private void SelectPieceTarget(GameObject _PieceToSelect)
    {

        // Unselect the piece if it was already selected
        if (_PieceToSelect == null || _PieceToSelect == SelectedPieceTarget)
        {
            if (SelectedPieceTarget != null)
                SelectedPieceTarget.GetComponent<Renderer>().material.color = Color.white;

            SelectedPieceTarget = null;

        }
        else
        {
            if (naves_targets.Contains(_PieceToSelect))
            {
                // Change color of the selected piece to make it apparent. Put it back to white when the piece is unselected
                if (SelectedPieceTarget)
                {
                    SelectedPieceTarget.GetComponent<Renderer>().material.color = Color.white;
                }
                SelectedPieceTarget = _PieceToSelect;
                SelectedPieceTarget.GetComponent<Renderer>().material.color = Color.red;

            }
        }
    }


    private void SelectPiece(GameObject _PieceToSelect)
    {
        infos_selected.text = "";
        dropMovimento.ClearOptions();
        dropMovimento.value = 0;

        // Unselect the piece if it was already selected
        if (_PieceToSelect == null || _PieceToSelect == SelectedPiece)
        {
            if (SelectedPiece != null)
                SelectedPiece.GetComponent<Renderer>().material.color = Color.white;

            SelectedPiece = null;
            SelectedPiece_script = null;
            script = null;


        }
        else
        {
            // Change color of the selected piece to make it apparent. Put it back to white when the piece is unselected
            if (SelectedPiece)
            {
                SelectedPiece.GetComponent<Renderer>().material.color = Color.white;
            }
            SelectedPiece = _PieceToSelect;
            SelectedPiece.GetComponent<Renderer>().material.color = Color.blue;

            script = Type.GetType(SelectedPiece.GetComponent<ShipScript>().namescript);
            SelectedPiece_script = SelectedPiece.GetComponent(script);

            if (gameState == gamestates["escolhe_movimento"])
            {
                MethodInfo theMethod = script.GetMethod("OptionsMovimento");
                theMethod.Invoke(SelectedPiece_script, null);
            }

            if (gameState == gamestates["realiza_movimento"])
            {
                MethodInfo theMethod = script.GetMethod("loadMovimento");
                theMethod.Invoke(SelectedPiece_script, null);

            }



            AtualizaInfo();

        }
    }


    #endregion




    /*
   public void preFaseTarget() { //ordena as naves para o tiro


       GameObject ship;
       GameObject[] ships = GameObject.FindGameObjectsWithTag("Ship");
       Infos info;
       ordem_naves = new Dictionary<int, ArrayList>();
       ArrayList conjunto_naves;

       for (int x = 0; x < ships.Length; x++)
       {
           ship = ships[x];
           info = ship.GetComponent<Infos>();
           maiorSkillPiloto = info.skillpiloto > maiorSkillPiloto ? info.skillpiloto : maiorSkillPiloto;
       }
       skill_ativo = maiorSkillPiloto;  //skill_em_movimento neste caso vira
       for (int x = 0; x <= maiorSkillPiloto; x++)
       {
           conjunto_naves = new ArrayList();

           for (int p = 0; p < ships.Length; p++)
           {
               ship = ships[p];
               info = ship.GetComponent<Infos>();
               if (info.skillpiloto == maiorSkillPiloto - x)
               {
                   conjunto_naves.Add(ship);
               }

           }
           ordem_naves.Add(x+1, conjunto_naves);

       }

       faseTiro();
   }
   */

}
