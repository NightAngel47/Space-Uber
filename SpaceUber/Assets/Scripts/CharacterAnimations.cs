/*
 * CharacterAnimations.cs
 * Author(s): Steven Drovie []
 * Created on: 3/29/2021 (en-US)
 * Description: Randomly picks character idle animations to play at random time intervals.
 */

using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    private Animator animator;

    [SerializeField, MinMaxSlider(0f, 10f), Tooltip("The range in seconds that will be randomly selected as the duration between poses.")]
    private Vector2 timeBetweenPoses = new Vector2(0.5f, 2f);
    
    void Start()
    {
        animator = GetComponent<Animator>();
        
        StartCoroutine(PlayCharacterAnimation());
    }
    
    /// <summary>
    /// Waits for a random period of time before play a random animation from the listed bool parameters in the character's animation controller
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayCharacterAnimation()
    {
        // wait for random amount of seconds
        yield return new WaitForSeconds(Random.Range(timeBetweenPoses.x, timeBetweenPoses.y));
        
        // make sure we're still event is still active
        if (!EventSystem.instance.eventActive) yield break;
        
        // turn on random bool parameter to play
        int randomPos = Random.Range(0, animator.parameters.Length);
        animator.SetBool(animator.parameters[randomPos].name, true);
        
        // wait for a non-looping animation
        yield return new WaitUntil(() => !animator.GetCurrentAnimatorStateInfo(0).loop);
        
        animator.SetBool(animator.parameters[randomPos].name, false);
       
        // wait for a looping animation
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).loop);
        
        // start next wait period if still in event
        if (EventSystem.instance.eventActive) StartCoroutine(PlayCharacterAnimation());
    }
}
