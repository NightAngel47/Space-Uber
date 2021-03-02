/*
 * CharacterStats.cs
 * Author(s): 
 * Created on: 2/2/2021 (en-US)
 * Description: 
 */

using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    #region Variables
    public enum Characters
    {
        None = -1,
        Kuon,
        Lanri,
        Lexa,
        Mateo,
        Ripley
    }

    private int[] characterApprovals = new int[5];

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
        get => characterApprovals[(int) Characters.Kuon];
        set
        {
            characterApprovals[(int) Characters.Kuon] = value;
            Mathf.Clamp(characterApprovals[(int) Characters.Kuon], approvalMin, approvalMax);
        }
    }

    public int MateoApproval
    {
        get => characterApprovals[(int) Characters.Mateo];
        set
        {
            characterApprovals[(int) Characters.Mateo] = value;
            Mathf.Clamp(characterApprovals[(int) Characters.Mateo], approvalMin, approvalMax);
        }
    }
    public int LanriApproval
    {
        get => characterApprovals[(int) Characters.Lanri];
        set
        {
            characterApprovals[(int) Characters.Lanri] = value;
            Mathf.Clamp(characterApprovals[(int) Characters.Lanri], approvalMin, approvalMax);
        }
    }
    public int LexaApproval
    {
        get => characterApprovals[(int) Characters.Lexa];
        set
        {
            characterApprovals[(int) Characters.Lexa] = value;
            Mathf.Clamp(characterApprovals[(int) Characters.Lexa], approvalMin, approvalMax);
        }
    }
    public int RipleyApproval
    {
        get => characterApprovals[(int) Characters.Ripley];
        set 
        { 
            characterApprovals[(int) Characters.Ripley] = value;
            Mathf.Clamp(characterApprovals[(int) Characters.Ripley], approvalMin, approvalMax);
        }
    }

    #endregion

    private void Start()
    {
        if(SavingLoadingManager.instance.GetHasSave())
        {
            ResetCharacterStats();
        }
        else
        {
            characterApprovals[(int) Characters.Kuon] = kuonApprovalInit;
            characterApprovals[(int) Characters.Mateo] = mateoApprovalInit;
            characterApprovals[(int) Characters.Lexa] = lexaApprovalInit;
            characterApprovals[(int) Characters.Lanri] = lanriApprovalInit;
            characterApprovals[(int) Characters.Ripley] = ripleyApprovalInit;
            SaveCharacterStats();
        }
    }

    public int GetCharacterApproval(Characters character)
    {
        return characterApprovals[(int) character];
    }
    
    public void SaveCharacterStats()
    {
        SavingLoadingManager.instance.Save<int[]>("characterApprovals", characterApprovals);
    }
    
    public void ResetCharacterStats()
    {
        characterApprovals = SavingLoadingManager.instance.Load<int[]>("characterApprovals");
    }
}
