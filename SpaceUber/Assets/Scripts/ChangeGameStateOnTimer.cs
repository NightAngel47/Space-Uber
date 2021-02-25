using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGameStateOnTimer : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private InGameStates state;
    
    void Start()
    {
        StartCoroutine("Timer");
    }
    
    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(time);
        GameManager.instance.ChangeInGameState(state);
    }
}
