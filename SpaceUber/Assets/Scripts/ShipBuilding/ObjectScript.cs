/*
 * ObjectScript.cs
 * Author(s): Sydney
 * Created on: #CREATIONDATE#
 * Description: Interface for object after its placed, checks if it is deleted or edited and sets that up
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using TMPro;

public class ObjectScript : MonoBehaviour
{
    [Foldout("Data")]
    public int rotAdjust = 1;
    public static Color c;

    public int shapeType;
    public int objectNum;

    public bool canRotate;  //true can rotate | false cannot rotate
    public bool nextToRoom; //true required next to x room | false no condition 
    public int nextToRoomNum;
    public bool needsSpecificLocation;
    public static bool CalledFromSpawn = false;

    public string[] mouseOverAudio;

    [SerializeField] private ShapeType shapeDataTemplate = null;

    [Foldout("Data")]
    public ShapeType shapeData = null;
    public ShapeTypes shapeTypes => shapeData.St;

    [SerializeField] private GameObject hoverUiPanel;
    [SerializeField] private TMP_Text roomNameUI;
    [SerializeField] private TMP_Text roomDescUI;
    [SerializeField] private TMP_Text roomPrice;
    [SerializeField] private Transform statsUI;
    [SerializeField] private GameObject resourceUI;
    

    [Foldout("Data")]
    public float boundsUp;
    [Foldout("Data")]
    public float boundsDown;
    [Foldout("Data")]
    public float boundsLeft;
    [Foldout("Data")]
    public float boundsRight;
    [Foldout("Data")]
    public float rotBoundsRight;
    [Foldout("Data")]
    public float rotBoundsUp;

    [Foldout("Data")]
    public Vector3 rotAdjustVal;

    public bool clickAgain = true;

    private void Start()
    {
        //rotAdjust = false;
        c = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        c.a = 1;
        //parentObj = transform.parent.gameObject;
        
        roomNameUI.text = gameObject.GetComponent<RoomStats>().roomName;
        roomDescUI.text = gameObject.GetComponent<RoomStats>().roomDescription;
        //roomPrice.text = gameObject.GetComponent<RoomStats>().price.ToString(); //Caused errors when trying to test, remember to un-comment

        foreach (var resource in gameObject.GetComponent<RoomStats>().resources)
        {
            GameObject resourceGO = Instantiate(resourceUI, statsUI);
            resourceGO.transform.GetChild(0).GetComponent<Image>().sprite = resource.resourceIcon; // resource icon
            resourceGO.transform.GetChild(1).GetComponent<TMP_Text>().text = resource.resourceType; // resource name
            resourceGO.transform.GetChild(2).GetComponent<TMP_Text>().text = resource.amount.ToString(); // resource amount
        }

        ResetData();
    }

    public void TurnOnClickAgain()
    {
        clickAgain = true;
        CalledFromSpawn = false;
    }

    public void TurnOffClickAgain()
    {
        clickAgain = false;

        if(nextToRoom == true && CalledFromSpawn == false)
        {
            bool check = SpotChecker.instance.NextToRoomCall(gameObject, rotAdjust);
            if (check == false)
            {
                Debug.Log("Room not placed next to required room, it has been auto removed");
                SpotChecker.instance.RemoveSpots(gameObject, rotAdjust);
                Destroy(gameObject);
            }
        }
    }

    public void OnMouseOver()
    {
        if (GameManager.instance.currentGameState == InGameStates.ShipBuilding && clickAgain == true) 
        {
            if (ObjectMover.hasPlaced == true)
            {
                hoverUiPanel.SetActive(true);
            }
            else if (hoverUiPanel.activeSelf)
            {
                hoverUiPanel.SetActive(false);
            }
            
            if (Input.GetMouseButton(0) && ObjectMover.hasPlaced == true)
            {
                //buttons.SetActive(true);
                gameObject.GetComponent<RoomStats>().SubtractRoomStats();
                AudioManager.instance.PlaySFX(mouseOverAudio[Random.Range(0, mouseOverAudio.Length - 1)]);
                Edit();
            }

            if (Input.GetMouseButton(1))
            {
                //buttons.SetActive(true);
                if (ObjectMover.hasPlaced == true)
                {
                    gameObject.GetComponent<RoomStats>().SubtractRoomStats();
                    AudioManager.instance.PlaySFX("Sell");
                }

                Delete();
            }
            
            //TODO might need to allow seeing room stats outside of ship building, however this was done to not have them show during events
            
        }

        if(GameManager.instance.currentGameState == InGameStates.CrewManagement || GameManager.instance.currentGameState == InGameStates.Events)
        {
            hoverUiPanel.SetActive(true);

            if (Input.GetMouseButton(0))
            {
                FindObjectOfType<CrewManagement>().UpdateRoom(gameObject);
            }
        }
    }


    public void OnMouseExit()
    {
        if (hoverUiPanel.activeSelf)
        {
            hoverUiPanel.SetActive(false);
        }
    }

    public void Edit()
    {
        c.a = .5f;
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = c;
        c.a = 1;
        SpotChecker.instance.RemoveSpots(gameObject, rotAdjust);
        gameObject.GetComponent<ObjectMover>().enabled = true;
        gameObject.GetComponent<ObjectMover>().TurnOnBeingDragged();
        ObjectMover.hasPlaced = false;

        ObjectScript[] otherRooms = FindObjectsOfType<ObjectScript>();
        foreach (ObjectScript r in otherRooms)
        {
            if (r != gameObject.GetComponent<ObjectScript>())
            {
                r.TurnOffClickAgain();
            }
        }
    }

    public void Delete()
    {
        //buttons.SetActive(false);
        SpotChecker.instance.RemoveSpots(gameObject, rotAdjust);
        ObjectMover.hasPlaced = true;
        ObjectScript[] otherRooms = FindObjectsOfType<ObjectScript>();
        foreach (ObjectScript r in otherRooms)
        {
            r.TurnOnClickAgain();
        }
        Destroy(gameObject);
    }

    private void ResetData()
    {
        shapeData = shapeDataTemplate.CloneData();
        boundsUp = shapeData.boundsUp;
        boundsDown = shapeData.boundsDown;
        boundsLeft = shapeData.boundsLeft;
        boundsRight = shapeData.boundsRight;
        rotBoundsRight = shapeData.rotBoundsRight;
        rotBoundsUp = shapeData.rotBoundsUp;

        rotAdjustVal = shapeData.rotAdjustVal;

        gameObject.GetComponent<ObjectMover>().UpdateMouseBounds(boundsDown, boundsUp, boundsLeft, boundsRight);
    }

    public void UpdateHoverUIData(string n, string d, string s)
    {
        hoverUiPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = n;
        hoverUiPanel.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = d;
        hoverUiPanel.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = s;
    }
}
