using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSignatureBehaviour : MonoBehaviour
{
    [SerializeField] private CharacterStats.Characters character;
    [SerializeField] private int threashold;
    
    private CharacterStats cStats;
    
    void Start()
    {
        cStats = FindObjectOfType<CharacterStats>();
        
        int successes = 0;
        switch (character)
        {
            case CharacterStats.Characters.KUON:
                successes = cStats.KuonSuccesses;
                break;
            case CharacterStats.Characters.LANRI:
                successes = cStats.LanriSuccesses;
                break;
            case CharacterStats.Characters.LEXA:
                successes = cStats.LexaSuccesses;
                break;
            case CharacterStats.Characters.MATEO:
                successes = cStats.MateoSuccesses;
                break;
            case CharacterStats.Characters.RIPLEY:
                successes = cStats.RipleySuccesses;
                break;
        }
        
        if(successes < threashold)
        {
            gameObject.SetActive(false);
        }
    }
}
