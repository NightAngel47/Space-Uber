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
        if(SavingLoadingManager.instance.GetHasSave())
        {
            ResetStats();
        }
        else
        {
            kuonApproval = kuonApprovalInit;
            mateoApproval = mateoApprovalInit;
            lexaApproval = lexaApprovalInit;
            lanriApproval = lanriApprovalInit;
            ripleyApproval = ripleyApprovalInit;
            SaveStats();
        }
    }
    
    public void SaveStats()
    {
        SavingLoadingManager.instance.Save<int>("kuonApproval", kuonApproval);
        SavingLoadingManager.instance.Save<int>("mateoApproval", mateoApproval);
        SavingLoadingManager.instance.Save<int>("lexaApproval", lexaApproval);
        SavingLoadingManager.instance.Save<int>("lanriApproval", lanriApproval);
        SavingLoadingManager.instance.Save<int>("ripleyApproval", ripleyApproval);
    }
    
    public void ResetStats()
    {
        KuonApproval = SavingLoadingManager.instance.Load<int>("kuonApproval");
        MateoApproval = SavingLoadingManager.instance.Load<int>("mateoApproval");
        LexaApproval = SavingLoadingManager.instance.Load<int>("lexaApproval");
        LanriApproval = SavingLoadingManager.instance.Load<int>("lanriApproval");
        RipleyApproval = SavingLoadingManager.instance.Load<int>("ripleyApproval");
    }
}
