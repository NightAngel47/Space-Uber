using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACTools.Saving;

public class SavingLoadingManager : MonoBehaviour
{
    public static SavingLoadingManager instance;
    
    private bool hasSave;
    
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
    
    public enum RoomType {None, AromorPlating, Armory, Brig, Bunks, CoreChargingTerminal, EnergyCannon, Hydroponics, Medbay, Pantry, PhotonTorpedoes, PowerCore, ShieldGenerator, Storage, TeleportationStation, VIPLounge, WarpDrive}
    
    public struct RoomData
    {
        public int x;
        public int y;
        public int rotation;
        public int crew;
        public RoomType type;
    }
}
