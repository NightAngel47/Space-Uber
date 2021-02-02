/*
 * CharacterStats.cs
 * Author(s): 
 * Created on: 2/2/2021 (en-US)
 * Description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    #region Variables
    public enum Characters
    {
        KUON,
        LANRI,
        LEXA,
        MATEO,
        RIPLEY
    }

    private int kuonApproval;
    private int lanriApproval;
    private int lexaApproval;
    private int mateoApproval;
    private int ripleyApproval;

    private int approvalMax = 10;
    private int approvalMin = -10;

    [SerializeField, Tooltip("The starting value for Kuon's approval")]
    private int kuonApprovalInit;

    [SerializeField, Tooltip("The starting value for Kuon's approval")]
    private int lanriApprovalInit;

    [SerializeField, Tooltip("The starting value for Kuon's approval")]
    private int lexaApprovalInit;

    [SerializeField, Tooltip("The starting value for Kuon's approval")]
    private int mateoApprovalInit;

    [SerializeField, Tooltip("The starting value for Kuon's approval")]
    private int ripleyApprovalInit;
    #endregion


    #region Getters/Setters
    public int KuonApproval
    {
        get { return kuonApproval; }
        set
        {
            kuonApproval = value;
            Mathf.Clamp(kuonApproval, approvalMin, approvalMax);
        }
    }
    public int MateoApproval
    {
        get { return mateoApproval; }
        set
        {
            mateoApproval = value;
            Mathf.Clamp(mateoApproval, approvalMin, approvalMax);
        }
    }
    public int LanriApproval
    {
        get { return lanriApproval; }
        set
        {
            lanriApproval = value;
            Mathf.Clamp(lanriApproval, approvalMin, approvalMax);
        }
    }
    public int LexaApproval
    {
        get { return lexaApproval; }
        set
        {
            lexaApproval = value;
            Mathf.Clamp(lexaApproval, approvalMin, approvalMax);
        }
    }
    public int RipleyApproval
    {
        get { return ripleyApproval; }
        set 
        { 
            ripleyApproval = value;
            Mathf.Clamp(ripleyApproval, approvalMin, approvalMax);
        }
    }

    #endregion

    private void Start()
    {
        kuonApproval = kuonApprovalInit;
        mateoApproval = mateoApprovalInit;
        lexaApproval = lexaApprovalInit;
        lanriApproval = lanriApprovalInit;
        ripleyApproval = ripleyApprovalInit;

    }
}
