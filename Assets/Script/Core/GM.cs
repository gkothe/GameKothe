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
    public int player_ativo = 1;
    [HideInInspector]
    public int player_inciativa = 1;  //futuramente o valor acho q pode ser mudado dinamicamente
    [HideInInspector]
    public int player_seminciativa = 2;  //futuramente o valor acho q pode ser mudado dinamicamente
    public bool player1_pass = false;
    public bool player2_pass = false;
    public Dictionary<string, int> gamestates;
    public Camera PlayerCam;
    public GameObject board;

    public static Button btnGo;
    public static Button btnIniciaFazeMov;
    public static Button btnIniciaFazeMov2;
    public static Button btnShoot;
    public static Button btnChecktarget;
    public static Button btnAcao;

    public Prefabs prefabs;
    public Dropdown dropMovimento;
    private GameObject SelectedPiece;
    private Component SelectedPiece_script;
    private GameObject SelectedPieceTarget;
    public ArrayList naves_targets = new ArrayList();  //targets atual da nave selecionada, essa info contem dentro da nave tb. facil acesso aki
    public Dictionary<string, ArrayList> ordem_naves;
    public ArrayList naves_jamoveram = new ArrayList();
    private Type script; //o tipo é pego quando seleciona a nave

    private Text infos_selected;
    public int maiorSkillPiloto = 1;  //maior skill de piloto em jogo, o menor skill SEMPRE será 1.
    public int skill_ativo = 0;  //skill ativo na fase de movimento ou combate
    public bool emMovimento = false;
    public bool testeAcao = false;
    public int proxid_nave = 0; //gerador automatico para id das naves
    public int getIdparanave()
    {
        proxid_nave = proxid_nave + 1;
        return proxid_nave;
    }

    void Awake()
    {


        infos_selected = GameObject.Find("Info_selected").GetComponent<Text>();
        btnGo = GameObject.Find("btnGO").GetComponent<Button>() as Button;
        btnShoot = GameObject.Find("btnShoot").GetComponent<Button>() as Button;
        btnIniciaFazeMov = GameObject.Find("btnIniMovimento").GetComponent<Button>() as Button;
        btnIniciaFazeMov2 = GameObject.Find("btnIniMovimento2").GetComponent<Button>() as Button;
        btnChecktarget = GameObject.Find("btnCheckTargets").GetComponent<Button>() as Button;
        btnAcao = GameObject.Find("btnAcao").GetComponent<Button>() as Button;
        PlayerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        prefabs = GameObject.FindGameObjectWithTag("Prefab").GetComponent<Prefabs>();



        setaNaves();
        //  testeNaves();

    }
    // Use this for initialization
    void Start()
    {


        iniGamestates();
        ChangeGameState("escolhe_movimento");


    }

    void Update()
    {
        GetMouseInputs();
        testeFimMovimento();
    }

    void GetMouseInputs()
    {
        Ray _ray;
        RaycastHit _hitInfo;

        if (!emMovimento)
        {

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
                                SelectPieceTarget(null);
                            }
                            else if (naves_targets != null && naves_targets.Contains(_hitInfo.collider.gameObject) && SelectedPiece != null)
                            {

                                SelectPieceTarget(_hitInfo.collider.gameObject);
                                //SelectPiece(null);
                            }  //  )

                        }
                        else {
                            SelectPiece(null);
                            SelectPieceTarget(null);
                        }
                    }

                }
            }

        }
    }

    public void criaNave(String name, Vector3 posicao, Quaternion rot, String arma, String piloto, int player, GameObject prefab)
    {


        MethodInfo theMethod;
        Component comp;

        Type componente = Type.GetType(name);
        GameObject naveObj = (GameObject)Instantiate(prefab, posicao, rot);
        //GameObject naveObj = (GameObject)Instantiate(prefabs.nave, new Vector3(2,0.05f,1), Quaternion.Euler(0f, 0f, 0f));
        naveObj.AddComponent(componente);
        comp = naveObj.GetComponent(componente);  // Selected Piece    
        theMethod = componente.GetMethod("funStart");
        theMethod.Invoke(comp, null);

        ((ShipScript)naveObj.GetComponent<ShipScript>()).namescript = name;
        ((Infos)naveObj.GetComponent<Infos>()).player = player;
        ((Infos)naveObj.GetComponent<Infos>()).nome_armaObjeto = arma;
        ((Infos)naveObj.GetComponent<Infos>()).nome_pilotoObjeto = piloto;
        ((Infos)naveObj.GetComponent<Infos>()).carregaComponentes();



    }

    void setaNaves()
    {

      criaNave("Nave1", new Vector3(2, 0.05f, -0.8f), Quaternion.Euler(0f, 0f, 0f), "Weapon1", "Piloto2", 1, prefabs.nave);
        criaNave("Nave1", new Vector3(-0.12f, 0.05f, -0.03f), Quaternion.Euler(0f, 0f, 0f), "Weapon1", "Piloto2", 1, prefabs.nave);
      //  criaNave("Nave1", new Vector3(-1.72f, 0.05f, -3.03f), Quaternion.Euler(0f, 0f, 0f), "Weapon1", "Piloto2", 1, prefabs.nave);
        //   criaNave("Nave1", new Vector3(-3.72f, 0.05f, -3.03f), Quaternion.Euler(0f, 0f, 0f), "Weapon1", "Piloto3", 1, prefabs.nave);



        criaNave("Nave1", new Vector3(2, 0.05f, 1.94f), Quaternion.Euler(0f, 180f, 0f), "Weapon1", "Piloto1", 2, prefabs.nave);
        criaNave("Nave1", new Vector3(-0.12f, 0.05f, 0.94f), Quaternion.Euler(0f, 180f, 0f), "Weapon1", "Piloto2", 2, prefabs.nave);
       // criaNave("Nave1", new Vector3(-1.72f, 0.05f, 3.94f), Quaternion.Euler(0f, 180f, 0f), "Weapon1", "Piloto2", 2, prefabs.nave);
        //   criaNave("Nave1", new Vector3(-3.72f, 0.05f, 3.94f), Quaternion.Euler(0f, 180f, 0f), "Weapon1", "Piloto3", 2, prefabs.nave);


    }

    #region fases

    private void iniGamestates()
    {

        gamestates = new Dictionary<string, int>();
        gamestates.Add("escolhe_movimento", 0);
        gamestates.Add("realiza_movimento", 3);
        gamestates.Add("escolhe_acao", 5);
        gamestates.Add("fase_tiro", 10);
        gamestates.Add("fase_tiro_2", 12);

    }

    public void ChangeGameState(string state)
    {

        int gmstate = gamestates[state];

        if (state.Equals("escolhe_movimento"))
        {
            limpaNavesObjetos(true, true, true, true);

            btnGo.enabled = true;
            btnShoot.enabled = false;
            btnChecktarget.enabled = false;
            btnIniciaFazeMov.enabled = true;
            btnIniciaFazeMov2.enabled = true;

        }
        else if (state.Equals("fase_tiro"))
        {

            btnGo.enabled = false;
            btnShoot.enabled = true;
            btnChecktarget.enabled = true;
            btnIniciaFazeMov.enabled = false;
            btnIniciaFazeMov2.enabled = false;


        }
        else if (state.Equals("realiza_movimento"))
        {
            btnGo.enabled = true;
            btnShoot.enabled = false;
            btnChecktarget.enabled = false;
            btnIniciaFazeMov.enabled = false;
            btnIniciaFazeMov2.enabled = false;

        }
        else if (state.Equals("escolhe_acao"))
        {

            testeAcao = true;
            /*btnGo.enabled = true;
            btnShoot.enabled = false;
            btnChecktarget.enabled = false;
            btnIniciaFazeMov.enabled = false;
            btnIniciaFazeMov2.enabled = false;
            */
        }





        gameState = gmstate;

    }

    public void prepFaseOrdem() //prepara a ordem de movimentação dos pilotos a partir do skill, skill , serve tb
    {

        GameObject ship;
        GameObject[] ships = GameObject.FindGameObjectsWithTag("Ship");
        Infos info;
        ordem_naves = new Dictionary<string, ArrayList>();
        ArrayList conjunto_naves;
        maiorSkillPiloto = 0;
        bool teste = true;
        for (int x = 0; x < ships.Length; x++)
        {
            ship = ships[x];
            info = ship.GetComponent<Infos>();
            maiorSkillPiloto = info.skillpiloto > maiorSkillPiloto ? info.skillpiloto : maiorSkillPiloto;
            if (gameState == gamestates["escolhe_movimento"])
            {
                if (ship.GetComponent<ShipScript>().movimento_armazenado == 0)//se tiver nave sem movimento armenzado, nao pode continuar para a prox fase,
                {
                    teste = false;
                    break;
                }
            }
        }

        if (teste)
        {
            for (int x = 1; x <= maiorSkillPiloto; x++)
            {
                conjunto_naves = new ArrayList();

                for (int p = 0; p < ships.Length; p++)
                {
                    ship = ships[p];
                    info = ship.GetComponent<Infos>();
                    if (info.skillpiloto == x && info.player == player_inciativa)
                    {
                        conjunto_naves.Add(ship);
                    }

                }
                ordem_naves.Add(x + "_" + player_inciativa, conjunto_naves);

            }

            for (int x = 1; x <= maiorSkillPiloto; x++)
            {
                conjunto_naves = new ArrayList();

                for (int p = 0; p < ships.Length; p++)
                {
                    ship = ships[p];
                    info = ship.GetComponent<Infos>();
                    if (info.skillpiloto == x && info.player == player_seminciativa)
                    {
                        conjunto_naves.Add(ship);
                    }

                }
                ordem_naves.Add(x + "_" + player_seminciativa, conjunto_naves);

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
        else
        {
            player1_pass = false;
            player2_pass = false;


            ShipScript shipScriptObj;


            for (int x = 0; x < ships.Length; x++)
            {
                ship = ships[x];
                shipScriptObj = ship.GetComponent<ShipScript>();
                if (shipScriptObj.movimento_armazenado != 0)
                    shipScriptObj.setMovimento(shipScriptObj.movimento_armazenado);

            }


            Debug.Log("Naves sem movimento");
            //TODO msg na tela dizendo que tem naves sem movimento
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

            limpaNavesObjetos(true, true, false, false);
            ChangeGameState("fase_tiro");
            prepFaseOrdem();

            //fase de tiro de cada nave;
        }
        else {
            conjunto_naves = ordem_naves[skill_ativo + "_" + player_inciativa];
            for (int x = 0; x < conjunto_naves.Count; x++)
            {
                if (conjunto_naves[x] != null)
                {
                    ship = (GameObject)conjunto_naves[x];
                    if (!naves_jamoveram.Contains(ship))
                    {
                        ship.GetComponent<Renderer>().material.color = Color.green;

                        shipClass = ((ShipScript)ship.GetComponent<ShipScript>());
                        shipClass.ativo_MovAtk = true;
                        shipClass.texto1.text = "Mover";
                        shipClass.texto1.color = Color.green;
                        player_ativo = player_inciativa;
                        tem_nave_para_mover = true;
                    }
                }
            }
            if (!tem_nave_para_mover)
            {

                conjunto_naves = ordem_naves[skill_ativo + "_" + player_seminciativa];
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
                        player_ativo = player_seminciativa;
                        tem_nave_para_mover = true;
                    }
                }

            }

            if (!(tem_nave_para_mover) && skill_ativo <= maiorSkillPiloto)
            {
                skill_ativo++;
                FaseMovimento();
            }


        }

    }

    public void fimEscolhaMov(int player)
    {

        if (player == 1)
        {
            player1_pass = true;
        }

        if (player == 2)
        {
            player2_pass = true;
        }

        if (player2_pass && player1_pass)
        {
            if (gameState == gamestates["escolhe_movimento"])
            {
                limpaNavesObjetos(true, true, true, true);//cuidar para nao limpar o movimento armazenado
                prepFaseOrdem();
            }
        }

    }

    public void testeFimMovimento()
    {
        if (!emMovimento && gameState == gamestates["escolhe_acao"] && testeAcao)
        {
            testeAcao = false;
            MethodInfo theMethod = script.GetMethod("OptionsAcao");
            theMethod.Invoke(SelectedPiece_script, null);

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


            ChangeGameState("escolhe_movimento");

        }
        else {

            conjunto_naves = ordem_naves[skill_ativo + "_" + player_inciativa];

            for (int x = 0; x < conjunto_naves.Count; x++)
            {
                ship = (GameObject)conjunto_naves[x];
                shipClass = ((ShipScript)ship.GetComponent<ShipScript>());
                if (!naves_jamoveram.Contains(ship) && shipClass.testePodeAtacar())
                {
                    ship.GetComponent<Renderer>().material.color = Color.green;

                    shipClass.ativo_MovAtk = true;
                    shipClass.texto1.text = "Atirar";
                    shipClass.texto1.color = Color.red;
                    player_ativo = player_inciativa;
                    tem_nave_para_atirar = true;
                }
            }


            if (!tem_nave_para_atirar)
            {
                conjunto_naves = ordem_naves[skill_ativo + "_" + player_seminciativa];

                for (int x = 0; x < conjunto_naves.Count; x++)
                {
                    if (!(conjunto_naves[x].Equals("null")) && conjunto_naves[x] != null)
                    {
                        ship = (GameObject)conjunto_naves[x];
                        shipClass = ((ShipScript)ship.GetComponent<ShipScript>());
                        if (!naves_jamoveram.Contains(ship) && shipClass.testePodeAtacar())
                        {
                            ship.GetComponent<Renderer>().material.color = Color.green;


                            shipClass.ativo_MovAtk = true;
                            shipClass.texto1.text = "Atirar";
                            shipClass.texto1.color = Color.red;
                            player_ativo = player_seminciativa;
                            tem_nave_para_atirar = true;
                        }
                    }
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

    public void Botao_iniFazeMov() //player 1
    {
        fimEscolhaMov(1);
    }

    public void Botao_iniFazeMov2()  //player 2 , isso vai mudar qdo for feito qestao de online/lan
    {
        fimEscolhaMov(2);
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

    public void Botao_ConfAcao()
    {
        if (gameState == gamestates["escolhe_acao"])
        {
            if (SelectedPiece != null)
            {
                int acao = dropMovimento.value;
                MethodInfo theMethod = script.GetMethod("realizaAcao");
                object[] parametersArray = new object[] { acao };


                theMethod.Invoke(SelectedPiece_script, parametersArray);

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

    public void limpaNavesObjetos(bool bloco1, bool bloco2, bool limpaacao, bool limpacolisao)
    {
        GM gm = GameObject.Find("GM").GetComponent<GM>() as GM;
        GameObject ship;
        Component scriptnave;
        GameObject[] ships = GameObject.FindGameObjectsWithTag("Ship");

        if (bloco1)
        {

            gm.SelectPiece(null);
            gm.SelectPieceTarget(null);
            gm.SelectedPiece_script = null;
            gm.script = null;
            gm.naves_targets = null;
        }

        if (bloco2)
        {

            gm.naves_jamoveram = new ArrayList();
            gm.ordem_naves = new Dictionary<string, ArrayList>();

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
                if (limpacolisao)
                {
                    shipScriptObj.ultimaColisao = new ArrayList();
                }

                shipScriptObj.texto1.text = "";
                shipScriptObj.ativo_MovAtk = false;
                if (limpaacao)
                {
                    shipScriptObj.acao_armazenada = 0;
                }

                shipScriptObj.acao_able = true;




            }

        }


    }

    public void AtualizaInfo()
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

    public void SelectPieceTarget(GameObject _PieceToSelect)
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

    public void SelectPiece(GameObject _PieceToSelect)
    {
        infos_selected.text = "";
        dropMovimento.ClearOptions();
        dropMovimento.value = 0;

        // Unselect the piece if it was already selected
        if (_PieceToSelect == null || _PieceToSelect == SelectedPiece)
        {
            if (SelectedPiece != null)
            {
                SelectedPiece.GetComponent<Renderer>().material.color = Color.white;

                if (gameState == gamestates["fase_tiro"])
                {
                    SelectedPiece.GetComponent<ShipScript>().cleanTargets();
                }
            }
            SelectedPiece = null;
            SelectedPiece_script = null;
            script = null;
            naves_targets = null;

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

            if (gameState == gamestates["fase_tiro"])
            {
                MethodInfo theMethod = script.GetMethod("Tiro_basic");
                theMethod.Invoke(SelectedPiece_script, null);

            }


            AtualizaInfo();

        }
    }


    #endregion



}
