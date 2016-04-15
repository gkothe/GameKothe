using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;



public class ShipScript : MonoBehaviour
{


    protected Rigidbody rb;
    Collider shippcollider;
    Vector3 startPosition;
    protected Dropdown dropMovimento;
    protected float base_size;
    public float speed = 5f;
    private float distance = 5f;
    private float theAngle = 22f;
    private float segments = 75f;
    protected GM gm;
    int layerShip;
    int layerAsteroide;
    protected Transform turnleft1;
    protected Transform turnleft2;
    protected Transform turnleft3;
    protected Transform turnright1;
    protected Transform turnright2;
    protected Transform turnright3;
    protected Transform Bankleft1;
    protected Transform Bankleft2;
    protected Transform Bankleft3;
    protected Transform Bankright1;
    protected Transform Bankright2;
    protected Transform Bankright3;
    protected Slider BarHp;
    protected Slider BarShield;
    protected Transform spaw_movimento;
    protected Transform Shootingpoint_alpha;
    [HideInInspector]
    public Infos localinfo;

    public int HealthIni;
    public int ShieldIni;
    public Dictionary<string, int> movimentos;
    [HideInInspector]
    public int movimento_armazenado = 0;
    public Dictionary<string, int> acoes;
    [HideInInspector]
    public int acao_armazenada = 0;
    public bool acao_able = true;
    private ArrayList naves; //naves q estao dentro do angulo do arco
    public ArrayList ultimaColisao; //naves q colidiram no ultimo movimento anets do movimento sem colisao (para saber se a nave pode atacar, caso as naves tenham se pexado )

    [HideInInspector]
    public GameObject Uiship;
    [HideInInspector]
    public GameObject UiShip_HS;
    [HideInInspector]
    public Text texto1;
    public Text texto_player;
    //[HideInInspector]
    public string namescript;
    [HideInInspector]
    public bool ativo_MovAtk = false;



    // Use this for initialization
    void Start()
    {


    }

    void Update()
    {


    }

    public void cleanTargets()
    {

        Dictionary<string, object> nave = null;

        if (naves != null)
        {
            for (int i = 0; i < naves.Count; i++)
            {
                nave = (Dictionary<string, object>)naves[i];
                ((GameObject)nave["gameobject"]).GetComponent<ShipScript>().texto1.text = "";
                ((GameObject)nave["gameobject"]).GetComponent<Renderer>().material.color = Color.white;
            }
        }

        this.naves = null;
    }

    public void cleanStuff()
    {

        this.GetComponent<Renderer>().material.color = Color.white;
        this.cleanTargets();

        this.ativo_MovAtk = false;
        this.texto1.text = "";
    }

    protected void carregaComponentes()
    {


        rb = GetComponent<Rigidbody>();
        shippcollider = GetComponent<Collider>();
        dropMovimento = GameObject.Find("Movimentos").GetComponent<Dropdown>() as Dropdown;
        ultimaColisao = new ArrayList();

        layerShip = 1 << LayerMask.NameToLayer("Ship");

        layerAsteroide = 1 << LayerMask.NameToLayer("Asteroide");
        gm = GameObject.FindWithTag("GameController").GetComponent<GM>() as GM;
        localinfo = GetComponent<Infos>();
        namescript = GetComponent<Infos>().shipcript;

        Uiship = transform.Search("UiShip").gameObject;
        UiShip_HS = transform.Search("UiShip_HS").gameObject;

        texto1 = Uiship.transform.Search("text_perc").gameObject.GetComponent<Text>();
        texto1.text = "";

        texto_player = Uiship.transform.Search("text_player").gameObject.GetComponent<Text>();


        turnleft1 = transform.Search("TurnLeft1");
        turnleft2 = transform.Search("TurnLeft2");
        turnleft3 = transform.Search("TurnLeft3");
        turnright1 = transform.Search("TurnRight1");
        turnright2 = transform.Search("TurnRight2");
        turnright3 = transform.Search("TurnRight3");
        Bankleft1 = transform.Search("BankLeft1");
        Bankleft2 = transform.Search("BankLeft2");
        Bankleft3 = transform.Search("BankLeft3");
        Bankright1 = transform.Search("BankRight1");
        Bankright2 = transform.Search("BankRight2");
        Bankright3 = transform.Search("BankRight3");
        Shootingpoint_alpha = transform.Search("Shootingpoint_alpha");

        BarShield = UiShip_HS.transform.Search("ShieldBar").gameObject.GetComponent<Slider>();
        BarHp = UiShip_HS.transform.Search("Hpbar").gameObject.GetComponent<Slider>();

        BarHp.maxValue = HealthIni;
        BarShield.maxValue = ShieldIni;

        BarHp.value = HealthIni;
        BarShield.value = ShieldIni;

        spaw_movimento = transform.Search("spaw_movimento");

    }

    public float randomRangeGap(float min, float max, float min2, float max2)
    {
        float random = 0;

        if (Random.Range(1, 100) > 50)
        {
            random = Random.Range(min, max);
        }
        else {
            random = Random.Range(min2, max2);
        }


        return random;

    }


    #region target

    public bool testePodeAtacar()
    {
        Collider[] hitColliders = null;
        bool pode = true;
        hitColliders = Physics.OverlapBox(transform.position, Prefabs.halfextent_basico, transform.rotation, layerAsteroide);
        if (hitColliders.Length == 0)
        {
            pode = true;

        }
        else {
            pode = false;

        }
        return pode;
    }

    public void Tiro_basic()
    {



        //ids e naves q estao dentro do angulo do arco
        naves = new ArrayList();
        ArrayList ids = new ArrayList();
        RaycastSweep(ref naves, ref ids);
        TestaAsteroide(ref naves);
        calculaPercentagemAcerto(ref naves);

        /*   if (naves.Count > 0)
           {
               Dictionary<string, object> nave2 = (Dictionary<string, object>)naves[0];
               Debug.Log("linhas cast:" + nave2["linhascastadas"]);
               Debug.Log("linhas hit:" + nave2["n_hitlines"]);
               Debug.Log("linhas obstruida:" + nave2["linha_obstruida"]);

           }*/


        alvosUI(ref naves);
        gm.naves_targets = new ArrayList();
        Dictionary<string, object> nave;
        for (int i = 0; i < naves.Count; i++)
        {
            nave = (Dictionary<string, object>)naves[i];
            gm.naves_targets.Add((GameObject)nave["gameobject"]);
        }

        //   gm.ChangeGameState("fase_tiro");
    }

    protected void RaycastSweep(ref ArrayList naves, ref ArrayList ids)
    {
        Vector3 startPos = transform.position; // umm, start position !
        Vector3 targetPos = Vector3.zero; // variable for calculated end position


        float startAngle = -theAngle / 0.5f; // half the angle to the Left of the forward
        float finishAngle = theAngle / 0.5f; // half the angle to the Right of the forward

        Dictionary<string, object> nave;


        // the gap between each ray (increment)
        float inc = theAngle / segments;
        int layerMask = 1 << LayerMask.NameToLayer("Ship");
        int linhas_castadas = 0;
        int linhas = 0;
        ArrayList pontos = new ArrayList();
        RaycastHit hit = new RaycastHit();
        Infos info;

        // step through and find each target point
        for (float i = startAngle; i <= finishAngle; i += inc) // Angle from forward, 
        {
            targetPos = startPos + (Quaternion.Euler(0, i, 0) * transform.forward) * distance;
            linhas_castadas = linhas_castadas + 1;

            // linecast between points   
            if (Physics.Linecast(startPos, targetPos, out hit, layerMask))

            {
                info = hit.collider.gameObject.GetComponent<Infos>();
                if (info.player != localinfo.player && !(ultimaColisao.Contains(hit.collider.gameObject)))
                {
                    string id = info.id.ToString();
                    if (!ids.Contains(id))
                    {
                        nave = new Dictionary<string, object>();
                        pontos = new ArrayList();

                        nave.Add("id", id);
                        nave.Add("gameobject", hit.collider.gameObject);
                        nave.Add("n_hitlines", 1);
                        nave.Add("distancia", Vector3.Distance(startPos, targetPos));
                        nave.Add("linhascastadas", linhas_castadas);
                        nave.Add("angle_protiro", this.transform.rotation.eulerAngles.y + (i));
                        pontos.Add(hit.point);
                        nave.Add("pontos", pontos);


                        ids.Add(id);//array de id para teste só
                        naves.Add(nave);
                    }
                    else {
                        for (int p = 0; p < naves.Count; p++)
                        {
                            nave = (Dictionary<string, object>)naves[p];
                            if (nave["id"].Equals(id))
                            {

                                linhas = (int)nave["n_hitlines"] + 1;
                                nave.Remove("n_hitlines");
                                nave.Add("n_hitlines", linhas);

                                //lugar q hitou
                                pontos = (ArrayList)nave["pontos"];
                                nave.Remove("pontos");
                                pontos.Add(hit.point);
                                nave.Add("pontos", pontos);

                            }

                        }
                    }
                }

                //Debug.Log( "Hit " + hit.collider.gameObject.name );
            }


            for (int p = 0; p < naves.Count; p++)
            {
                nave = (Dictionary<string, object>)naves[p];

                nave.Remove("linhascastadas");
                nave.Add("linhascastadas", linhas_castadas);

            }

            // to show ray just for testing
            Debug.DrawLine(startPos, targetPos, Color.green);
        }
    }

    protected void TestaAsteroide(ref ArrayList naves)
    {
        Dictionary<string, object> nave;
        ArrayList points = new ArrayList();
        Vector3 posicaodanave = this.transform.position;
        int linha_obstruida = 0;
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("Asteroide");

        for (int p = 0; p < naves.Count; p++)
        {
            nave = (Dictionary<string, object>)naves[p];
            points = (ArrayList)nave["pontos"];
            linha_obstruida = 0;

            for (int x = 0; x < points.Count; x++)
            {

                if (Physics.Linecast(posicaodanave, (Vector3)points[x], out hit, layerMask))
                {
                    linha_obstruida = linha_obstruida + 1;
                    Debug.DrawLine(posicaodanave, hit.point, Color.blue);
                }

            }

            nave.Add("linha_obstruida", linha_obstruida);

        }

    }

    protected void calculaPercentagemAcerto(ref ArrayList naves)
    {
        Dictionary<string, object> nave;
        int qtdlinhas = 0;
        int qtdlinhas_obstruidas = 0;
        for (int p = 0; p < naves.Count; p++)
        {
            nave = (Dictionary<string, object>)naves[p];

            qtdlinhas_obstruidas = (int)nave["linha_obstruida"];
            qtdlinhas = (int)nave["n_hitlines"];

            nave.Add("perc_acerto", precision((GameObject)nave["gameobject"], qtdlinhas - qtdlinhas_obstruidas));

        }



    }

    public float precision(GameObject alvo, int qtd_linha)
    {

        Infos info = alvo.GetComponent<Infos>();

        float perc = 50 + (((float)qtd_linha / 50) * 10) + localinfo.baseprecision - info.evademod;

        if (perc > 95)
            perc = 95;

        return perc;
    }

    protected void alvosUI(ref ArrayList naves)
    {
        GameObject ship = null;
        Dictionary<string, object> nave;
        ShipScript alvo;
        for (int p = 0; p < naves.Count; p++)
        {
            nave = (Dictionary<string, object>)naves[p];
            ship = (GameObject)nave["gameobject"];
            ship.GetComponent<Renderer>().material.color = Color.yellow;
            alvo = ((ShipScript)ship.GetComponent<ShipScript>());

            alvo.texto1.text = "To hit: " + (float)nave["perc_acerto"] + "%";
            alvo.texto1.color = Color.yellow;


        }

        //setar no GM os targets avaible, criar no gm um array static para os targts e na hora q seleciona targer verificar se o objeto existe la dentro.

    }

    #endregion

    #region atk



    public void Shoot(GameObject nave_target)
    {

        StartCoroutine(Shoot_2(nave_target));
    }

    protected IEnumerator Shoot_2(GameObject nave_target)
    {
        Dictionary<string, float> danos;
        if (nave_target != null)
        {
            Dictionary<string, object> nave = null;
            float angulo = 0;
            for (int i = 0; i < naves.Count; i++)
            {
                nave = (Dictionary<string, object>)naves[i];
                if (((GameObject)nave["gameobject"]).Equals(nave_target))
                {
                    angulo = (float)nave["angle_protiro"];
                    break;
                }
            }

            danos = ataque(nave_target, (float)nave["perc_acerto"]);

            GameObject clone = (GameObject)Instantiate(gm.prefabs.shot, Shootingpoint_alpha.position, Quaternion.Euler(0f, angulo, 0f));
            if ((float)danos["dano"] == 0)
            {
                yield return StartCoroutine(shootMovement(clone, nave_target, true));

            }
            else {

                yield return StartCoroutine(shootMovement(clone, nave_target, false));
            }
            cleanStuff();
            gm.naves_jamoveram.Add(gameObject);
            gm.faseTiro();
        }
        else if (naves.Count == 0)
        {  //caso n tenha targets, passa adiante
            cleanStuff();
            gm.naves_jamoveram.Add(gameObject);
            gm.faseTiro();
        }
        else {//conssidera-se que sempre q tenha um target, ele VAI tentar atirar nele, se for o caso de poder escolher nao atirar, tem q criar um botao de "passar fase" que seta os targets para 0 e chama essa função de novo para cair na condição a cima
            Debug.Log("no target");
            gm.faseTiro();
        }

        // gm.ChangeGameState("escolhe_movimento");


    }

    public Dictionary<string, float> ataque(GameObject alvo, float prec)
    {

        //teste pra ve se acerta
        float danohull = 0;
        float danoshield = 0;
        float dano = 0;
        float sobra = 0;
        Infos infoalvo = alvo.GetComponent<Infos>();
        ShipScript scrip_alvo = alvo.GetComponent<ShipScript>();
        Dictionary<string, float> danos = new Dictionary<string, float>();


        if (Random.Range(0, 100f) < prec)
        {

            dano = Random.Range(localinfo.atkmin, localinfo.atkmax);
            danohull = dano * (localinfo.SP / 100);
            danoshield = dano - danohull;
            if (danoshield > infoalvo.shield)
            {//se o dano for maior q o shield
                sobra = danoshield - infoalvo.shield;
                danohull = danohull + sobra;
                danoshield = infoalvo.shield;
            }
            scrip_alvo.setHealth(infoalvo.health - danohull);
            scrip_alvo.setShield(infoalvo.shield - danoshield);
        }
        else {
        }

        danos.Add("danohull", danohull);
        danos.Add("danoshield", danoshield);
        danos.Add("dano", dano);


        return danos;

    }

    protected IEnumerator shootMovement(GameObject clone, GameObject nave_target, bool miss)
    {

        float sqrRemainingDistance = 0;

        Vector3 newPosition;
        Vector3 end = new Vector3(0f, 0f, 0f);
        if (miss)
        {
            end = nave_target.transform.position + new Vector3(randomRangeGap(-1, -0.5f, 0.5f, 1), 0f, randomRangeGap(-1, -0.5f, 0.5f, 1));

        }
        else {
            end = nave_target.transform.position;
        }


        //pra ter ctz q chego no ponto final
        sqrRemainingDistance = (clone.transform.position - end).sqrMagnitude; //usar transofrm do movimento?
        while (sqrRemainingDistance > float.Epsilon)
        {
            newPosition = Vector3.MoveTowards(clone.transform.position, end, 5 * Time.deltaTime);
            clone.transform.position = newPosition;
            sqrRemainingDistance = (clone.transform.position - end).sqrMagnitude; //usar transofrm do movimento?
            yield return null;

        }

        Destroy(clone);

    }

    public void setHealth(float value)
    {

        localinfo.health = value;
        if (localinfo.health <= 0)
        {
            Destroy(gameObject);
        }
        else {
            BarHp.value = value;
        }

    }

    public void setShield(float value)
    {
        localinfo.shield = value;
        BarShield.value = value;
    }

    #endregion


    #region movimento

    public void setMovimento(int movimento)
    {
        movimento_armazenado = movimento;
        if (movimento != 0)
        {
            texto1.text = "Ready";
            texto1.color = Color.green;
        }
        else {
            texto1.text = "";
        }

    }

    public void faseAcao()
    {
        if (acao_able)
        {
            gm.ChangeGameState("escolhe_acao");
        }
        else {
            gm.ChangeGameState("realiza_movimento");
            afterMovimento();
        }

    }

    public void realizaAcao(int acao)
    {

        acao_armazenada = acao;

        gm.ChangeGameState("realiza_movimento");
        afterMovimento();

    }


    public void addColisoesEmColididos() {

        GameObject shipObj;
        ShipScript shipScripObj;
        for (int i = 0; i < ultimaColisao.Count; i++)
        {
            shipObj = (GameObject)ultimaColisao[i];
            shipScripObj = shipObj.GetComponent<ShipScript>();
            shipScripObj.ultimaColisao.Add(gameObject);

        }

    }

    public void cleanColisoesOutrasNaves()
    {

        GameObject shipObj;
        GameObject shipObj2;
        ShipScript shipScripObj;
        
        for (int i = 0; i < ultimaColisao.Count; i++)
        {
            shipObj = (GameObject)ultimaColisao[i];
            shipScripObj = shipObj.GetComponent<ShipScript>();
            for (int x = 0; x < shipScripObj.ultimaColisao.Count; x++) {
                shipObj2 = (GameObject)shipScripObj.ultimaColisao[x];
                if (shipObj2.Equals(gameObject)) {
                    shipScripObj.ultimaColisao[x] = null;
                }
                
            }
            shipScripObj.ultimaColisao.TrimToSize();
        }

        ultimaColisao = new ArrayList();
    }


    public void afterMovimento()
    {

        addColisoesEmColididos();
        setMovimento(0);
        ativo_MovAtk = false;
        texto1.text = "";
        gm.naves_jamoveram.Add(gameObject);
        gm.testeAcao = false;
        gm.limpaNavesObjetos(true, false, false, false);
        gm.FaseMovimento();


    }

    public void loadMovimento()
    {
        dropMovimento.ClearOptions();

        string text = movimentos.FirstOrDefault(x => x.Value == movimento_armazenado).Key;
        dropMovimento.options.Add(new Dropdown.OptionData() { text = text });//  é soh informativo, este valor do index n vai ser usado
        dropMovimento.value = 0;
        dropMovimento.RefreshShownValue();

    }

    public ArrayList getPoints(int quantity, Vector3 ini, Vector3 end)
    {


        var points = new ArrayList();
        float ydiff = end.z - ini.z, xdiff = end.x - ini.x;
        float slope = (float)(end.z - ini.z) / (end.x - ini.x);
        float x, y;

        --quantity;

        for (float i = 0; i < quantity; i++)
        {
            y = slope == 0 ? 0 : ydiff * (i / quantity);
            x = slope == 0 ? xdiff * (i / quantity) : y / slope;
            //points[(int)i] = new Point((int)Math.Round(x) + p1.X, (int)Math.Round(y) + p1.Y);
            points.Add(new Vector3((x) + ini.x, 0, (y) + ini.z));
        }

        points.Add(end);
        return points;
    }

    public static bool AlmostEqual(Vector3 v1, Vector3 v2, float precision)
    {
        bool equal = true;

        if (Mathf.Abs(v1.x - v2.x) > precision) equal = false;
        if (Mathf.Abs(v1.y - v2.y) > precision) equal = false;
        if (Mathf.Abs(v1.z - v2.z) > precision) equal = false;

        return equal;
    }

    public IEnumerator move_foward(int foward)
    {

        startPosition = spaw_movimento.transform.position;

        int qtdpontos = 20 * foward;
        Vector3 end = startPosition + (this.transform.forward * ((base_size * foward)));
        ArrayList pontos = getPoints(qtdpontos, startPosition, end);

        end = testaPonto(pontos, end, "foward", 0f, 0, 0);
        pontos.TrimToSize();

        yield return StartCoroutine(fowardsmoothMovement(end));

    }

    public IEnumerator move_keyturn(int foward)
    {

        startPosition = spaw_movimento.transform.position;

        int qtdpontos = 20 * foward;
        Vector3 end = startPosition + (this.transform.forward * ((base_size * foward)));
        Vector3 endoriginal = end;
        ArrayList pontos = getPoints(qtdpontos, startPosition, end);

        end = testaPonto(pontos, end, "foward", 0f, 0, 0);
        pontos.TrimToSize();

        bool fazturn = false;
        if (AlmostEqual(end, endoriginal, 0f))
        {
            fazturn = true;
        }




        yield return StartCoroutine(fowardsmoothMovementKey(end, fazturn));

    }

    protected IEnumerator fowardsmoothMovementKey(Vector3 end, bool fazturn)
    {

        yield return StartCoroutine(fowardsmoothMovement(end));
        if (fazturn)
        {
            this.transform.Rotate(new Vector3(0, 180, 0));
        }

        gm.emMovimento = false;
    }

    public IEnumerator move_turn(string lado, Transform centro)
    {

        startPosition = spaw_movimento.localPosition; // new Vector3(0,0,0);


        float myAngleInDegrees = 90f * Mathf.Deg2Rad;

        Vector3 centrocircul = centro.transform.localPosition;

        float raio = Vector2.Distance(new Vector2(centrocircul.x, centrocircul.z), new Vector2(startPosition.x, startPosition.z));

        Vector3 randomCircle = new Vector3(Mathf.Cos(myAngleInDegrees), 0, Mathf.Sin(myAngleInDegrees));
        Vector3 end = centro.transform.TransformPoint(randomCircle * raio);

        //Instantiate (esfera, end, Quaternion.identity);  

        int angulo = 0;
        int xval = 0;
        int divisorangulo = 2;

        if (lado.Equals("direita"))
        {
            angulo = 180;
            xval = 90;
        }
        else {
            xval = 0;
            angulo = 90;
        }
      ;


        ArrayList pontos = new ArrayList();
        Vector3 novoponto = new Vector3(0, 0, 0);
        for (float x = (xval * divisorangulo); x < (angulo * divisorangulo); x++)
        {
            randomCircle = new Vector3(Mathf.Cos((x / divisorangulo) * Mathf.Deg2Rad), 0, Mathf.Sin((x / divisorangulo) * Mathf.Deg2Rad));
            novoponto = centro.transform.TransformPoint(randomCircle * (raio));
            //	Instantiate (cubo, novoponto, Quaternion.identity);  
            pontos.Add(novoponto);

        }


        if (lado.Equals("direita"))
        {
            pontos.Reverse();
        }


        end = testaPonto(pontos, end, lado, 90f, 0, divisorangulo);
        pontos.TrimToSize();
        yield return StartCoroutine(fowardAngLeMovement(end, pontos, lado, divisorangulo));


    }

    public IEnumerator move_Bank(string lado, Transform centro)
    {

        startPosition = spaw_movimento.localPosition; // new Vector3(0,0,0);


        float raio = 0;
        Vector3 centrocircul = centro.transform.localPosition;
        raio = Vector2.Distance(new Vector2(centrocircul.x, centrocircul.z), new Vector2(startPosition.x, startPosition.z));

        float myAngleInDegrees = 0;

        Vector3 randomCircle = new Vector3(0, 0, 0);
        Vector3 end = new Vector3(0, 0, 0);

        int angulo = 0;
        int xval = 0;
        int divisorangulo = 3;


        ArrayList pontos = new ArrayList();
        Vector3 novoponto = new Vector3(0, 0, 0);

        if (lado.Equals("direita"))
        {

            myAngleInDegrees = 135f * Mathf.Deg2Rad;
            randomCircle = new Vector3(Mathf.Cos(myAngleInDegrees), 0, Mathf.Sin(myAngleInDegrees));
            end = centro.transform.TransformPoint(randomCircle * raio);

            xval = 135;
            angulo = 180;


        }
        else {
            myAngleInDegrees = 45f * Mathf.Deg2Rad;
            randomCircle = new Vector3(Mathf.Cos(myAngleInDegrees), 0, Mathf.Sin(myAngleInDegrees));
            end = centro.transform.TransformPoint(randomCircle * raio);


            xval = 0;
            angulo = 45;


        };

        for (float x = (xval * divisorangulo); x < (angulo * divisorangulo); x++)
        {  //para maior precisa, aumentar qtidade de pontos

            randomCircle = new Vector3(Mathf.Cos((x / divisorangulo) * Mathf.Deg2Rad), 0, Mathf.Sin((x / divisorangulo) * Mathf.Deg2Rad));
            novoponto = centro.transform.TransformPoint(randomCircle * (raio));
            pontos.Add(novoponto);

        }

        if (lado.Equals("direita"))
        {
            pontos.Reverse();
        }

        end = testaPonto(pontos, end, lado, 45f, 0, divisorangulo);
        pontos.TrimToSize();


        yield return StartCoroutine(fowardAngLeMovement(end, pontos, lado, divisorangulo));


    }

    public Vector3 testaPonto(ArrayList pontos, Vector3 end, string lado, float graus, int interacao, int divisorangulo)
    {


        //Debug.Log ("interação: " + interacao);
        //o graus vai ser o count do array de pontos, para assim fazer o caclulo contrario da rotação
        // a interação vai  servir para saber qdo dos graus diminuir, se interessação for 0, qer dize q esta testando o end point, e a rotação é a rot maxima final ( 90 ou 45)


        Quaternion rot = Quaternion.Euler(0, 0, 0);//soh pra inicizaliza


        if (lado.Equals("direita"))
        {
            rot = Quaternion.Euler(0, (transform.rotation.eulerAngles.y + (graus - (((float)interacao) / (float)divisorangulo))), 0);
        }
        else if (lado.Equals("esquerda"))
        {
            rot = Quaternion.Euler(0, (transform.rotation.eulerAngles.y - (graus - (((float)interacao) / (float)divisorangulo))), 0);
        }
        else {
            rot = this.transform.rotation;
        }

        Collider[] hitColliders = null;
        if (interacao == 0)
        { //se a interação for zero, vai castar uma box no ponto final pra ve se colide com alguma nave
          //hitColliders = Physics.OverlapBox (end, halfextent, rot, LayerShip, QueryTriggerInteraction.Ignore);

            hitColliders = Physics.OverlapBox(end, Prefabs.halfextent_basico, rot, layerShip);
        }
        else if (interacao != (graus * divisorangulo) || (lado.Equals("foward")))
        {    //senao vai castar uma box no ponto em qestao pra ve se colide com alguma nave
            hitColliders = Physics.OverlapBox((Vector3)pontos[pontos.Count - (interacao) - 1], Prefabs.halfextent_basico, rot, layerShip);
        }
        else {
            return spaw_movimento.transform.position;
        }


        bool colide = false;


        for (int x = 0; x < hitColliders.Length; x++)
        {
            if (!hitColliders[x].Equals(shippcollider))
            {
                ultimaColisao = new ArrayList();
                break;
            }
        }



        for (int x = 0; x < hitColliders.Length; x++)
        {
            if (!hitColliders[x].Equals(shippcollider))
            {
                colide = true;
                ultimaColisao.Add(hitColliders[x].gameObject);
            }
        }



        if (colide)
        { //se ele acho um collider q é diferente do collider da ship q esta movendo, ele vai tenta outro ponto

            pontos[pontos.Count - (interacao) - 1] = null;
            acao_able = false;

            return testaPonto(pontos, end, lado, graus, (interacao) + 1, divisorangulo);
        }
        else {
            if (interacao == 0)
            {
                return end;
            }
            else {
                return (Vector3)pontos[pontos.Count - (interacao) - 1];
            }
        }


    }

    protected IEnumerator fowardsmoothMovement(Vector3 end)
    {

        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {

            Vector3 newPosition = Vector3.MoveTowards(this.transform.position, end, speed * Time.deltaTime);
            //Instantiate (cubo, newPosition, Quaternion.identity) ; 
            //rb.MovePosition (newPosition);
            transform.position = newPosition;
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;

        }
        gm.emMovimento = false;

    }

    protected IEnumerator fowardAngLeMovement(Vector3 end, ArrayList pontos, string lado, int divisorangulo)
    {

        float sqrRemainingDistance = 0;
        Quaternion rot;
        Vector3 newPosition;



        for (int x = 0; x < pontos.Count; x++)
        {
            if (pontos[x] != null)
            {


                Vector3 novoponto = (Vector3)pontos[x];
                sqrRemainingDistance = (transform.position - novoponto).sqrMagnitude;  //usar transofrm do movimento?

                while (sqrRemainingDistance > float.Epsilon)
                {

                    newPosition = Vector3.MoveTowards(this.transform.position, novoponto, speed * Time.deltaTime);
                    transform.position = newPosition;
                    sqrRemainingDistance = (transform.position - novoponto).sqrMagnitude; //usar transofrm do movimento?
                    yield return null;

                }


                if (lado.Equals("direita"))
                {
                    rot = Quaternion.Euler(0, (transform.rotation.eulerAngles.y + (1f / (float)divisorangulo)), 0);
                }
                else {
                    rot = Quaternion.Euler(0, (transform.rotation.eulerAngles.y + (-1f / (float)divisorangulo)), 0);
                }

                transform.rotation = rot;

            }
        }


        //pra ter ctz q chego no ponto final
        sqrRemainingDistance = (transform.position - end).sqrMagnitude; //usar transofrm do movimento?
        while (sqrRemainingDistance > float.Epsilon)
        {

            newPosition = Vector3.MoveTowards(this.transform.position, end, speed * Time.deltaTime);
            transform.position = newPosition;
            sqrRemainingDistance = (transform.position - end).sqrMagnitude; //usar transofrm do movimento?
            yield return null;

        }

        gm.emMovimento = false;

    }

    #endregion

    void OnTriggerEnter(Collider other)
    {
        /*
            if (other.CompareTag ("Shipp"))   {
                colidiu = true;
                return;
            }
        */

    }










    /*
protected void NaveTestaTiroRange(Dictionary<string,object> nave){


    Vector3 posicaodanave = ((GameObject)nave["gameobject"]).transform.position;
    Vector3 posicao_shootingpoint = new Vector3(0,0,0);
    Vector3 posicao_shootingpoint_menordist = new Vector3(0,0,0);
    GameObject child;
    RaycastHit hit; 
    float distanciamenor = -1f;
    float distancia_calculada = -1f;
    int layerMask = 1 << LayerMask.NameToLayer("Ship");




    while (Shootingpoint_alpha.transform.localPosition.x <= 0.475f) {

        hit = new RaycastHit() ;

        posicao_shootingpoint = Shootingpoint_alpha.transform.position;
        if ( Physics.Linecast( posicao_shootingpoint , posicaodanave,out hit, layerMask ) )
        {
            distancia_calculada = Vector3.Distance (posicao_shootingpoint,hit.point);

            if (distanciamenor == -1f) {
                distanciamenor = distancia_calculada;
                posicao_shootingpoint_menordist = Shootingpoint_alpha.transform.localPosition;
            } else if (distancia_calculada < distanciamenor) {
                distanciamenor = distancia_calculada;
                posicao_shootingpoint_menordist = Shootingpoint_alpha.transform.localPosition;
            }


            //Debug.Log( "Hit " + hit.collider.gameObject.name );
        }  

        Debug.DrawLine( posicao_shootingpoint, hit.point, Color.blue );    

        Shootingpoint_alpha.transform.localPosition =   new Vector3(Shootingpoint_alpha.transform.localPosition.x + 0.025f,0f, Shootingpoint_alpha.transform.localPosition.z);
    }

    Shootingpoint_alpha.transform.localPosition = new Vector3(-0.475f,0, Shootingpoint_alpha.transform.localPosition.z);

    //retornar range, se hita ou nao outras coisas, ponto de saida(shooting) com a menor distancia do ponto hitado

} */





}
