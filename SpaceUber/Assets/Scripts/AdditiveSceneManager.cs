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
    private int baseSceneIndex;
    private bool addedSceneLoaded;
    private GameObject[] addedObjects;
    
    // Start is called before the first frame update
    void Start()
    {
        baseSceneIndex = this.gameObject.scene.buildIndex;
        
        for(int i = SceneManager.sceneCount-1; i >= 0; i--)
        {
            int buildIndex = SceneManager.GetSceneAt(i).buildIndex;
            if(buildIndex != baseSceneIndex && (buildIndex != startingAddedScene || !addSceneOnStart))
            {
                SceneManager.UnloadSceneAsync(buildIndex);
            }
        }
        
        addedSceneLoaded = false;
        addedObjects = null;
        
        if(addSceneOnStart)
        {
            LoadScene(startingAddedScene, moveStartingObjects);
        }
    }
    
    public void LoadSceneSeperate(int scene)
    {
        LoadScene(scene, false);
    }
    
    public void LoadSceneMerged(int scene)
    {
        LoadScene(scene, true);
    }
    
    private void LoadScene(int scene, bool moveObjects)
    {
        if(!SceneManager.GetSceneByBuildIndex(scene).isLoaded)
        {
            UnloadScene();
            
            SceneManager.LoadScene(scene, LoadSceneMode.Additive);
            
            addedSceneLoaded = true;
            addedSceneIndex = scene;
        }
        else if(scene != baseSceneIndex)
        {
            addedSceneIndex = scene;
            addedSceneLoaded = true;
        }
        
        if(moveObjects)
        {
            StartCoroutine(MoveObjects(scene));
        }
    }
    
    private IEnumerator MoveObjects(int scene)
    {
        yield return new WaitUntil(() => SceneManager.GetSceneByBuildIndex(scene).isLoaded);
        GameObject[] newAddedObjects = SceneManager.GetSceneByBuildIndex(scene).GetRootGameObjects();
        foreach(GameObject obj in newAddedObjects)
        {
            SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByBuildIndex(baseSceneIndex));
        }
        
        if(addedObjects != null)
        {
            GameObject[] oldAddedObjects = addedObjects;
            int totalLength = newAddedObjects.Length + oldAddedObjects.Length;
            addedObjects = new GameObject[totalLength];
            for(int i = 0; i < addedObjects.Length; i++)
            {
                if(i < oldAddedObjects.Length)
                {
                    addedObjects[i] = oldAddedObjects[i];
                }
                else
                {
                    addedObjects[i] = newAddedObjects[i-oldAddedObjects.Length];
                }
            }
        }
        else
        {
            addedObjects = newAddedObjects;
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
