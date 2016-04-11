using UnityEngine;
using System.Collections;

public class PilotoBase : MonoBehaviour
{



    public string nomePiloto;
    public float baseprecision;
    public int skillpiloto;

    public string getNomePiloto()
    {
        return nomePiloto;
    }

    public float getBaseprecision()
    {
        return baseprecision;
    }

    public int getSkillpiloto()
    {
        return skillpiloto;
    }
}
