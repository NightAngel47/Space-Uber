using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ACTools.Saving;
using UnityEngine.SceneManagement;

public class SavingLoadingManager : MonoBehaviour
{
    public static SavingLoadingManager instance;
    private static string projectName = "CogInTheCosmicMachine";
    private bool hasSave;

    [SerializeField] private List<GameObject> roomPrefabs = new List<GameObject>();
    
    private void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        
        hasSave = LoadData.FromBinaryFile<bool>(projectName, "hasSave");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8) && hasSave) // kill it with fire at your own convenience
        {
            SetHasSaveFalse();
            if (SceneManager.GetSceneByName("Main_Menu").isLoaded)
            {
                SceneManager.LoadScene("Main_Menu");
            }
        }
    }

    public void NewSave()
    {
        if(!hasSave)
        {
            SaveData.ToBinaryFile<bool>(projectName, "hasSave", true);
        }
    }

    public T Load<T>(string name)
    {
        return LoadData.FromBinaryFile<T>(projectName, name);
    }
    
    public void Save<T>(string name, T data)
    {
        SaveData.ToBinaryFile<T>(projectName, name, data);
    }
    
    public bool GetHasSave()
    {
        return hasSave;
    }

    public void SetHasSaveFalse()
    {
        SaveData.ToBinaryFile<bool>(projectName, "hasSave", false);
        Debug.LogWarning("Save file has been deleted.");
    }
    
    public void SaveRooms()
    {
        RoomStats[] rooms = FindObjectsOfType<RoomStats>();
        RoomData[] data = new RoomData[rooms.Length];
        for(int i = 0; i < rooms.Length; i++)
        {
            data[i] = ConvertRoomToData(rooms[i].gameObject);
        }
        
        Save<RoomData[]>("roomData", data);
    }
    
    public void LoadRooms()
    {
        RoomData[] data = Load<RoomData[]>("roomData");
        foreach (var roomData in data)
        {
            ConvertDataToRoom(roomData);
        }
    }
    
    private RoomData ConvertRoomToData(GameObject room)
    {
        RoomData roomData = new RoomData();
        var position = room.transform.position;
        roomData.x = (int) position.x;
        roomData.y = (int) position.y;
        ObjectScript os = room.GetComponent<ObjectScript>();
        roomData.rotation = os.rotAdjust;
        roomData.isPrePlaced = os.preplacedRoom;
        roomData.objectNum = os.objectNum;
        RoomStats roomStats = room.GetComponent<RoomStats>();
        roomData.crew = roomStats.currentCrew;
        roomData.usedRoom = roomStats.usedRoom;

        return roomData;
    }
    
    private void ConvertDataToRoom(RoomData roomData)
    {
        ObjectMover.hasPlaced = false;

        GameObject room = null;
        foreach (GameObject roomPrefab in roomPrefabs.Where(roomPrefab => roomPrefab.GetComponent<ObjectScript>().objectNum == roomData.objectNum))
        {
            room = Instantiate(roomPrefab, new Vector3(roomData.x, roomData.y, 0), Quaternion.identity);
            break;
        }

        ObjectMover om = room.GetComponent<ObjectMover>();
        ObjectScript os = room.GetComponent<ObjectScript>();
        RoomStats roomStats = room.GetComponent<RoomStats>();
        
        os.ResetData();
        
        om.TurnOffBeingDragged();
        os.preplacedRoom = roomData.isPrePlaced;
        roomStats.usedRoom = roomData.usedRoom;
        if (roomStats.flatOutput)
        {
            roomStats.currentCrew = roomData.crew;
        }
        else
        {
            StartCoroutine(UpdateRoomActiveAmount(roomStats, roomData.crew));
        }

        room.transform.GetChild(0).transform.Rotate(0, 0, -90 * (roomData.rotation - 1));
        
        if ((os.shapeType != 0 || os.shapeType != 1 || os.shapeType != 3) && (os.rotAdjust == 1 || os.rotAdjust == 3) && (roomData.rotation == 2 || roomData.rotation == 4))
        {
            room.transform.GetChild(0).transform.position += os.rotAdjustVal;
        }
        else if ((os.shapeType != 0 || os.shapeType != 1 || os.shapeType != 3) && (os.rotAdjust == 2 || os.rotAdjust == 4) && (roomData.rotation == 1 || roomData.rotation == 3))
        {
            room.transform.GetChild(0).transform.position -= os.rotAdjustVal;
        }
        
        os.rotAdjust = roomData.rotation;
        om.UpdateMouseBounds(os.boundsDown, os.boundsUp, os.boundsLeft, os.boundsRight);
        
        ObjectMover.hasPlaced = true;
        
        om.enabled = false;
        
        room.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
        
        StartCoroutine(UpdateSpotChecker(room, roomData.isPrePlaced, roomData.rotation));
    }
    
    private IEnumerator UpdateSpotChecker(GameObject room, bool isPrePlaced, int rotation)
    {
        yield return new WaitUntil(() => FindObjectOfType<SpotChecker>());
        if(isPrePlaced)
        {
            SpotChecker.instance.FillPreplacedSpots(room);
        }
        else
        {
            SpotChecker.instance.FillSpots(room, rotation);
        }
    }

    private IEnumerator UpdateRoomActiveAmount(RoomStats roomStats, int crewCount)
    {
        print(roomStats.roomName);
        yield return new WaitUntil(() => roomStats.GetComponent<Resource>() && FindObjectOfType<CrewManagement>());
        CrewManagement crewManagement = FindObjectOfType<CrewManagement>();
        crewManagement.UpdateRoom(roomStats.gameObject);
        for (int i = 0; i < crewCount; ++i)
        {
            crewManagement.AddCrew(true);
        }
    }
    
    [Serializable]
    public struct RoomData
    {
        public int x;
        public int y;
        public int rotation;
        public int crew;
        public bool isPrePlaced;
        public int objectNum;
        public bool usedRoom;
    }
}
