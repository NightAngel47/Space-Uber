using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACTools.Saving;

public class SavingLoadingManager : MonoBehaviour
{
    public static SavingLoadingManager instance;
    
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
        
        hasSave = LoadData.FromBinaryFile<bool>("CogInTheCosmicMachine", "hasSave");
        
        if(!hasSave)
        {
            SaveData.ToBinaryFile<bool>("CogInTheCosmicMachine", "hasSave", true);
        }
    }
    
    private void Start()
    {
        if(hasSave)
        {
            LoadRooms();
        }
        else
        {
            SaveRooms();
        }
    }
    
    public T Load<T>(string name)
    {
        return LoadData.FromBinaryFile<T>("CogInTheCosmicMachine", name);
    }
    
    public void Save<T>(string name, T data)
    {
        SaveData.ToBinaryFile<T>("CogInTheCosmicMachine", name, data);
    }
    
    public bool GetHasSave()
    {
        return hasSave;
    }
    
    public static void DeleteSave()
    {
        SaveData.ToBinaryFile<bool>("CogInTheCosmicMachine", "hasSave", false);
    }
    
    public enum RoomType {None, ArmorPlating, Armory, Brig, Bunks, CoreChargingTerminal, EnergyCannon, Hydroponics, Medbay, Pantry, PhotonTorpedoes, PowerCore, ShieldGenerator, Storage, TeleportationStation, VIPLounge, WarpDrive}
    
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
        
        for(int i = 0; i < data.Length; i++)
        {
            ConvertDataToRoom(data[i]);
        }
        
        
    }
    
    private RoomData ConvertRoomToData(GameObject room)
    {
        RoomData roomData = new RoomData();
        roomData.x = (int) room.transform.position.x;
        roomData.y = (int) room.transform.position.y;
        roomData.rotation = room.GetComponent<ObjectScript>().rotAdjust;
        roomData.crew = room.GetComponent<RoomStats>().currentCrew;
        roomData.isPrePlaced = room.GetComponent<ObjectScript>().preplacedRoom;
        
        switch(room.GetComponent<RoomStats>().roomName)
        {
            case "Armor Plating":
                roomData.type = RoomType.ArmorPlating;
                break;
            case "Armory":
                roomData.type = RoomType.Armory;
                break;
            case "Brig":
                roomData.type = RoomType.Brig;
                break;
            case "Bunks":
                roomData.type = RoomType.Bunks;
                break;
            case "Core Charging Terminal":
                roomData.type = RoomType.CoreChargingTerminal;
                break;
            case "Energy Cannon":
                roomData.type = RoomType.EnergyCannon;
                break;
            case "Hydroponics":
                roomData.type = RoomType.Hydroponics;
                break;
            case "Medbay":
                roomData.type = RoomType.Medbay;
                break;
            case "Pantry":
                roomData.type = RoomType.Pantry;
                break;
            case "Photon Torpedoes":
                roomData.type = RoomType.PhotonTorpedoes;
                break;
            case "Power Core":
                roomData.type = RoomType.PowerCore;
                break;
            case "Shield Generator":
                roomData.type = RoomType.ShieldGenerator;
                break;
            case "Storage":
                roomData.type = RoomType.Storage;
                break;
            case "Teleportation Station":
                roomData.type = RoomType.TeleportationStation;
                break;
            case "VIP Lounge":
                roomData.type = RoomType.VIPLounge;
                break;
            case "Warp Drive":
                roomData.type = RoomType.WarpDrive;
                break;
            default:
                roomData.type = RoomType.None;
                break;
        }
        
        return roomData;
    }
    
    private void ConvertDataToRoom(RoomData roomData)
    {
        ObjectMover.hasPlaced = false;
        
        GameObject room;
        switch(roomData.type)
        {
            case RoomType.ArmorPlating:
                room = Instantiate(roomPrefabs[0], new Vector3(roomData.x, roomData.y, 0), Quaternion.identity);
                break;
            case RoomType.Armory:
                room = Instantiate(roomPrefabs[1], new Vector3(roomData.x, roomData.y, 0), Quaternion.identity);
                break;
            case RoomType.Brig:
                room = Instantiate(roomPrefabs[2], new Vector3(roomData.x, roomData.y, 0), Quaternion.identity);
                break;
            case RoomType.Bunks:
                room = Instantiate(roomPrefabs[3], new Vector3(roomData.x, roomData.y, 0), Quaternion.identity);
                break;
            case RoomType.CoreChargingTerminal:
                room = Instantiate(roomPrefabs[4], new Vector3(roomData.x, roomData.y, 0), Quaternion.identity);
                break;
            case RoomType.EnergyCannon:
                room = Instantiate(roomPrefabs[5], new Vector3(roomData.x, roomData.y, 0), Quaternion.identity);
                break;
            case RoomType.Hydroponics:
                room = Instantiate(roomPrefabs[6], new Vector3(roomData.x, roomData.y, 0), Quaternion.identity);
                break;
            case RoomType.Medbay:
                room = Instantiate(roomPrefabs[7], new Vector3(roomData.x, roomData.y, 0), Quaternion.identity);
                break;
            case RoomType.Pantry:
                room = Instantiate(roomPrefabs[8], new Vector3(roomData.x, roomData.y, 0), Quaternion.identity);
                break;
            case RoomType.PhotonTorpedoes:
                room = Instantiate(roomPrefabs[9], new Vector3(roomData.x, roomData.y, 0), Quaternion.identity);
                break;
            case RoomType.PowerCore:
                room = Instantiate(roomPrefabs[10], new Vector3(roomData.x, roomData.y, 0), Quaternion.identity);
                break;
            case RoomType.ShieldGenerator:
                room = Instantiate(roomPrefabs[11], new Vector3(roomData.x, roomData.y, 0), Quaternion.identity);
                break;
            case RoomType.Storage:
                room = Instantiate(roomPrefabs[12], new Vector3(roomData.x, roomData.y, 0), Quaternion.identity);
                break;
            case RoomType.TeleportationStation:
                room = Instantiate(roomPrefabs[13], new Vector3(roomData.x, roomData.y, 0), Quaternion.identity);
                break;
            case RoomType.VIPLounge:
                room = Instantiate(roomPrefabs[14], new Vector3(roomData.x, roomData.y, 0), Quaternion.identity);
                break;
            case RoomType.WarpDrive:
                room = Instantiate(roomPrefabs[15], new Vector3(roomData.x, roomData.y, 0), Quaternion.identity);
                break;
            default:
                return;
        }
        
        ObjectMover om = room.GetComponent<ObjectMover>();
        ObjectScript os = room.GetComponent<ObjectScript>();
        
        os.ResetData();
        
        om.TurnOffBeingDragged();
        os.preplacedRoom = roomData.isPrePlaced;
        room.GetComponent<RoomStats>().currentCrew = roomData.crew;
        
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
        yield return new WaitWhile(() => FindObjectOfType<SpotChecker>() == null);
        
        if(isPrePlaced)
        {
            FindObjectOfType<SpotChecker>().FillPreplacedSpots(room);
        }
        else
        {
            FindObjectOfType<SpotChecker>().FillSpots(room, rotation);
        }
    }
    
    [System.Serializable]
    public struct RoomData
    {
        public int x;
        public int y;
        public int rotation;
        public int crew;
        public bool isPrePlaced;
        public RoomType type;
    }
}
