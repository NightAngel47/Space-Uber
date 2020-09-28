using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveSceneManager : MonoBehaviour
{
    public bool addSceneOnStart;
    public int startingAddedScene;
    public bool moveStartingObjects;
    
    private int addedSceneIndex;
    private bool addedSceneLoaded;
    private GameObject[] addedObjects;
    
    // Start is called before the first frame update
    void Start()
    {
        if(addSceneOnStart)
        {
            LoadScene(startingAddedScene, moveStartingObjects);
        }
        else
        {
            addedSceneLoaded = false;
            addedObjects = null;
        }
    }
    
    public void LoadScene(int scene, bool moveObjects)
    {
        if(!SceneManager.GetSceneByBuildIndex(scene).isLoaded)
        {
            UnloadScene();
            
            SceneManager.LoadScene(scene, LoadSceneMode.Additive);
            
            addedSceneLoaded = true;
            addedSceneIndex = scene;
        }
        
        if(moveObjects)
        {
            addedObjects = SceneManager.GetSceneByBuildIndex(scene).GetRootGameObjects();
            foreach(GameObject obj in addedObjects)
            {
                SceneManager.MoveGameObjectToScene(obj, SceneManager.GetActiveScene());
            }
        }
    }
    
    public void UnloadScene()
    {
        if(addedSceneLoaded)
        {
            if(addedObjects != null)
            {
                foreach(GameObject obj in addedObjects)
                {
                    if(obj != null)
                    {
                        Destroy(obj);
                    }
                }
                
                addedObjects = null;
            }
            
            SceneManager.UnloadSceneAsync(addedSceneIndex);
            addedSceneLoaded = false;
        }
    }
}
