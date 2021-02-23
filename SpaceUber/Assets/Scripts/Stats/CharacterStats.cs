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
        None,
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
    
    private int kuonSuccesses;
    private int lanriSuccesses;
    private int lexaSuccesses;
    private int mateoSuccesses;
    private int ripleySuccesses;

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
    
    public int KuonSuccesses
    {
        get { return kuonSuccesses; }
        set
        {
            kuonSuccesses = value;
        }
    }
    public int MateoSuccesses
    {
        get { return mateoSuccesses; }
        set
        {
            mateoSuccesses = value;
        }
    }
    public int LanriSuccesses
    {
        get { return lanriSuccesses; }
        set
        {
            lanriSuccesses = value;
        }
    }
    public int LexaSuccesses
    {
        get { return lexaSuccesses; }
        set
        {
            lexaSuccesses = value;
        }
    }
    public int RipleySuccesses
    {
        get { return ripleySuccesses; }
        set
        {
            ripleySuccesses = value;
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
        
        kuonSuccesses = 0;
        mateoSuccesses = 0;
        lexaSuccesses = 0;
        lanriSuccesses = 0;
        ripleySuccesses = 0;
    }
}
