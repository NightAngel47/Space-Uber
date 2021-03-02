using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSignatureBehaviour : MonoBehaviour
{
    [SerializeField] private CharacterStats.Characters character;
    [SerializeField] private int threashold;
    
    void Start()
    {
        if(FindObjectOfType<CharacterStats>().GetCharacterApproval(character) < threashold)
        {
            gameObject.SetActive(false);
        }
    }
}
