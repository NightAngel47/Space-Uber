/*
 * SlotReel.cs
 * Author(s): #Greg Brandt#
 * Created on: 11/12/2020 (en-US)
 * Description: "Rotates" reels and adjusts them to snap to a position when stopped
 */

using NaughtyAttributes;
using System.Collections;
using UnityEngine;



public class SlotReel : MonoBehaviour
{
    [SerializeField] RectTransform upperHalfReel;
    [SerializeField] Transform upperLayoutGroup;
    [SerializeField] RectTransform lowerHalfReel;
    [SerializeField] Transform lowerLayoutGroup;
    [SerializeField] private GameObject[] slotTypes;

    [Tooltip("Transforms who's position is used to snap a reel to a value when stopped")]
    [SerializeField] private Transform[] slots;

    [Tooltip("Y value for lower half reel to move to be top half reel.")]
    [SerializeField] private float adjustPositionThreshold = -300;
    [Tooltip("Distance slot must be from focus before snaping in place.")]
    [SerializeField] private float snapDistanceThreshold = 0.75f;
    Transform selectedSlot;
    
    Slot slot;
    float reelSpeed;
    float spinAfterStopTime;
    float x;
    bool spin = false;
    bool stopped = false;
    bool gameStarted;

    void Start()
    {
        x = upperHalfReel.anchoredPosition.x;
        SpawnSlots();
    }
    void Update()
    {
        if (spin)
        {
            Spin();
        }
        
        AdjustPosition();
    }

    public bool Spinning() { return spin; }
    public Slot GetSlot() { return slot; }
    public void SetSpeed(float speed) { reelSpeed = speed; }
    public void SetSpinAfterStopTime(float time) { spinAfterStopTime = time; }

    /// <summary>
    /// "Rotates" reels
    /// <para>direction: 1 to "Rotate" down. -1 to "Rotate" up</para>
    /// </summary>
    /// <param name="speedAdjustment"></param>
    /// <param name="direction"></param>
    void Spin(float speedAdjustment = 1, int direction = 1 )
	{
        if(direction > 1) { direction = 1; }
        if(direction < -1) { direction = -1; }
        if(direction == 0) { direction = 1; }
        float lowerY = lowerHalfReel.anchoredPosition.y;
        float upperY = upperHalfReel.anchoredPosition.y;
        Vector2 upperPos = new Vector2(x, upperY - (direction * speedAdjustment * reelSpeed * Time.deltaTime));
        Vector2 lowerPos = new Vector2(x, lowerY - (direction * speedAdjustment * reelSpeed * Time.deltaTime));
        upperHalfReel.anchoredPosition = upperPos;
        lowerHalfReel.anchoredPosition = lowerPos;
    }

    /// <summary>
    /// Spawns all slots in a random order
    /// </summary>
    private void SpawnSlots()
    {
        bool[] instantiated = new bool[slotTypes.Length];
        int overallIndex = 0;

        //auto assign all to false from the start
        for (int i = 0; i < instantiated.Length; i++)
        {
            instantiated[i] = false;
        }

        //instantiate upper slots
        for (int i = 0; i < slotTypes.Length; i++)
        {
            //reroll until we find a slot type we haven't already spawned
            int randNum = Random.Range(0, slotTypes.Length);
            
            while (instantiated[randNum] != false)
            {
                randNum = Random.Range(0, slotTypes.Length);
            }

            Transform newSlot = GameObject.Instantiate(slotTypes[randNum], upperLayoutGroup).transform;
            slots[overallIndex] = newSlot;
            overallIndex++;
            instantiated[randNum] = true;
        }

        //reset instantiated bool
        for (int i = 0; i < instantiated.Length; i++)
        {
            instantiated[i] = false;
        }

        //instantiate lower slots
        for (int i = 0; i < slotTypes.Length; i++)
        {
            //reroll until we find a slot type we haven't already spawned
            int randNum = Random.Range(0, slotTypes.Length);
            while (instantiated[randNum] != false)
            {
                randNum = Random.Range(0, slotTypes.Length);
            }
            instantiated[randNum] = true;
            Transform newSlot = GameObject.Instantiate(slotTypes[randNum], lowerLayoutGroup).transform;
            slots[overallIndex] = newSlot;
            overallIndex++;
        }
    }

    void AdjustPosition()
	{
        if(upperHalfReel.anchoredPosition.y < adjustPositionThreshold)
		{
            float y = upperHalfReel.anchoredPosition.y;
            y += lowerHalfReel.rect.height * 2;
            upperHalfReel.anchoredPosition = new Vector2(x, y);
		}
        if (lowerHalfReel.anchoredPosition.y < adjustPositionThreshold)
        {
            float y = lowerHalfReel.anchoredPosition.y;
            y += (upperHalfReel.rect.height * 2);
            lowerHalfReel.anchoredPosition = new Vector2(x, y);
        }
    }

    public void StartSpinning() 
    { 
        spin = true; 
        gameStarted = true;
    }

    public void StopSpinning()
	{
        if (!stopped && gameStarted)
        {
            stopped = true;
            StartCoroutine(SlowSpinningToHalt());
        }
	}

    IEnumerator SlowSpinningToHalt()
	{
        float originalSpeed = reelSpeed;
        float speedMultiplier = 1f;
        for(int i = 0; i < 10; i++)
		{
            speedMultiplier -= 0.05f;
            reelSpeed = reelSpeed * speedMultiplier;
            yield return new WaitForSeconds(spinAfterStopTime/10);
        }
        reelSpeed = originalSpeed;
        spin = false;
        float distance = Vector3.Distance(transform.position, slots[0].position);
        selectedSlot = slots[0];
        foreach(Transform slot in slots)
		{
            float newDistance = Vector3.Distance(transform.position, slot.position);
            if (newDistance < distance)
			{
                selectedSlot = slot;
                distance = newDistance;
			}
		}
        slot = selectedSlot.GetComponent<Slot>();
        bool inPosition = false;
        while(!inPosition)
		{
            distance = Vector3.Distance(transform.position, selectedSlot.position);
            if (distance < snapDistanceThreshold)
            {
                inPosition = true;
                transform.position = selectedSlot.position;
            }
            else
            {
                //Adjust reel up or down at a slower speed to get closer to snap point
                if (selectedSlot.position.y > transform.position.y) { Spin(0.4f); }
                else { Spin(0.4f, -1); }
            }
            yield return new WaitForEndOfFrame();
		}
	}

}