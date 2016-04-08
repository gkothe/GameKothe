using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
    [HideInInspector]
    public static int gameState = 0; //0 movimento, 1 tiro
    public GameObject board;
    public static Button btnGo;
    public static Button btnShoot;
    public static Button btnChecktarget;
    public Dropdown dropMovimento;
    private GameObject SelectedPiece;   // Selected Piece
    private Component SelectedPiece_script; // Selected Piece    

    private GameObject SelectedPieceTarget;   // Selected Piece
    public static ArrayList naves_targets = new ArrayList();
    public Camera PlayerCam;            // Camera used by the player
    private Type script; //o tipo é pego quando seleciona a nave
    public static int proxid_nave = 0;
    // Camera used by the player
    private Text infos_selected;

    public static int getIdparanave()
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
        btnChecktarget = GameObject.Find("btnCheckTargets").GetComponent<Button>() as Button;
        ChangeGameState(0);


    }

    public static void limpaNavesObjetos()
    {
        GM gm = GameObject.Find("GM").GetComponent<GM>() as GM;
        gm.SelectPiece(null);
        gm.SelectPieceTarget(null);
        gm.SelectedPiece_script = null;
        gm.script = null;
        GM.naves_targets = null;
        GameObject ship;
        Component scriptnave;
        GameObject[] ships = GameObject.FindGameObjectsWithTag("Ship");
        Type script;
        for (int x = 0; x < ships.Length; x++)
        {
            ship = ships[x];
            script = Type.GetType(ship.GetComponent<ShipScript>().namescript);
            scriptnave = ship.GetComponent(script);
            MethodInfo theMethod = script.GetMethod("cleanStuff");
            theMethod.Invoke(scriptnave, null);

        }
        

    }

    public static void ChangeGameState(int state)
    {

        if (state == 0)
        {
            limpaNavesObjetos();
            btnShoot.enabled = false;
            //      btnChecktarget.enabled = false;
            btnGo.enabled = true;

        }
        else if (state == 1)
        {
            btnShoot.enabled = true;
            //    btnChecktarget.enabled = true;
            btnGo.enabled = false;

        }



        gameState = state;

    }

    // Update is called once per frame
    void Update()
    {
        GetMouseInputs();
    }

    public void Botao_Go()
    {

        if (gameState == 0)
        {
            if (SelectedPiece != null)
            {
                MethodInfo theMethod = script.GetMethod("movimento");
                theMethod.Invoke(SelectedPiece_script, null);
            }
        }

    }


    public void Botao_Shoot()
    {
        if (gameState == 1)
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


    void GetMouseInputs()
    {
        Ray _ray;
        RaycastHit _hitInfo;


        if (gameState == 0)
        {
            // On Left Click
            if (Input.GetMouseButtonDown(0))
            {
                _ray = PlayerCam.ScreenPointToRay(Input.mousePosition); // Specify the ray to be casted from the position of the mouse click

                // Raycast and verify that it collided
                if (Physics.Raycast(_ray, out _hitInfo))
                {
                    // Select the piece if it has the good Tag

                    if (_hitInfo.collider.gameObject.tag == ("Ship"))
                    {

                        SelectPiece(_hitInfo.collider.gameObject);
                    }
                    else {
                        SelectPiece(null);
                    }
                }
                /* else {
                     SelectPiece(null);
                 }*/
            }
        }
        else if (gameState == 1)
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
            MethodInfo theMethod = script.GetMethod("OptionsMovimento");
            theMethod.Invoke(SelectedPiece_script, null);
            AtualizaInfo();

        }
    }

    private void AtualizaInfo()
    {
        Infos inf = SelectedPiece_script.GetComponent<Infos>();
        String text = "";
        text = "Id: " + inf.id + "\n";
        text = text + "Health: " + inf.health + "\n";
        text = text + "Shield: " + inf.shield + "\n";
        infos_selected.text = text;
    }

}
