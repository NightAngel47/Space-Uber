/*
 * AdditiveSceneManager.cs
 * Author(s): Lachlan Duncan
 * Created on: 29 September 2020
 * Description: Streamlines the process of loading and unloading scenes on top of an always loaded base scene, with options to automatically load a second scene when the base scene loads and to move objects to the base scene, allowing objects in different scenes to easily interract
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <remarks>
/// Place this script on an empty game object in the scene you want to serve as the base scene.  Set the public variables in the inspector if you want a second scene to be loaded automatically when the base scene is loaded, and use the public methods to load, unload, and change which scene is loaded on top of the base scene later on.
/// </remarks>
public class AdditiveSceneManager : MonoBehaviour
{
    /// <summary>
    /// When set to true, a second scene is loaded as soon as the base scene is loaded.
    /// </summary>
    public bool addScenesOnStart;
    
    /// <summary>
    /// The build settings id of the scene that is loaded if <c>addSceneOnStart</c> is set to true.
    /// </summary>
    public string[] startingAddedScenes;
    
    /// <summary>
    /// When set to true, the objects from <c>startingAddedScene</c> are moved to the base scene if <c>startingAddedScene</c> is loaded at the start.
    /// </summary>
    public bool[] moveStartingObjects;
    
    /// <summary>
    /// Max number of scenes that can be loaded at once.
    /// </summary>
    private int maxAddedScenes = 10;
    
    // The names of the second loaded scene
    private string[] addedSceneNames;
    
    // The the base scene
    private string baseSceneName;
    
    // True if a second scene is loaded, false if the base scene is the only scene loaded
    private int numAddedScenesLoaded;
    
    // Keeps track of objects from added scenes that were moved to the base scene so unloading the added scene can still remove the objects
    private GameObject[][] addedObjects;
    
    // Start is called before the first frame update
    void Start()
    {
        if(startingAddedScenes.Length > maxAddedScenes)
        {
            string[] oldStartingAddedScenes = startingAddedScenes;
            startingAddedScenes = new string[maxAddedScenes];
            for(int i = 0; i < maxAddedScenes; i++)
            {
                startingAddedScenes[i] = oldStartingAddedScenes[i];
            }
        }
        
        // The scene containing this script's game object is the base scene
        baseSceneName = gameObject.scene.name;
        
        // Unloads all scenes that are not supposed to be loaded, just in case.  Doesn't unload scenes that are supposed to be loaded
        for(int i = SceneManager.sceneCount-1; i >= 0; i--)
        {
            int buildIndex = SceneManager.GetSceneAt(i).buildIndex;
            bool shouldExist = false;
            foreach(string scene in startingAddedScenes)
            {
                if(buildIndex == SceneManager.GetSceneByName(scene).buildIndex)
                {
                    shouldExist = true;
                }
            }
            if(buildIndex != SceneManager.GetSceneByName(baseSceneName).buildIndex && (!shouldExist || !addScenesOnStart))
            {
                SceneManager.UnloadSceneAsync(buildIndex);
            }
        }
        
        // Sets the private variables
        addedSceneNames = new string[maxAddedScenes];
        numAddedScenesLoaded = 0;
        addedObjects = new GameObject[maxAddedScenes][];
        
        // Automatically loads the second scene if it is set to be loaded
        if(addScenesOnStart)
        {
            LoadScenes(startingAddedScenes, moveStartingObjects);
        }
    }
    
    /// <summary>
    /// Loads the specified scene without moving objects to the base scene, which is probably better for performance but can cause issues if objects from different scenes are supposed to interract.  The base scene stays loaded and any other scenes are unloaded first.
    /// </summary>
    /// <param name="scene">The integer id of the scene in the build settings.</param>
    public void LoadScenesSeperate(string[] scenes)
    {
        bool[] moveObjects = new bool[scenes.Length];
        for(int i = 0; i < moveObjects.Length; i++)
        {
            moveObjects[i] = false;
        }
        LoadScenes(scenes, moveObjects);
    }
    
    public void LoadSceneSeperate(string scene)
    {
        string[] scenes = new string[1];
        scenes[0] = scene;
        LoadScenesSeperate(scenes);
    }
    
    /// <summary>
    /// Loads the specified scene and moves its objects to the base scene, allowing objects from different scenes to easily interract.  The base scene stays loaded and any other scenes are unloaded first.
    /// </summary>
    /// <param name="scene">The integer id of the scene in the build settings.</param>
    public void LoadScenesMerged(string[] scenes)
    {
        bool[] moveObjects = new bool[scenes.Length];
        for(int i = 0; i < moveObjects.Length; i++)
        {
            moveObjects[i] = true;
        }
        LoadScenes(scenes, moveObjects);
    }
    
    public void LoadSceneMerged(string scene)
    {
        string[] scenes = new string[1];
        scenes[0] = scene;
        LoadScenesMerged(scenes);
    }
    
    // Actually contains the scene loading code.  I made this function private and split it into two public ones because I don't think UI buttons can be linked to functions with multiple parameters
    private void LoadScenes(string[] scenes, bool[] moveObjects)
    {
        for(int i = 0; i < scenes.Length; i++)
        {
            if(!SceneManager.GetSceneByName(scenes[i]).isLoaded) // If the scene's not already loaded, unload any other added scene and load the new scene
            {
                if(numAddedScenesLoaded == maxAddedScenes)
                {
                    UnloadScene();
                }
                
                SceneManager.LoadScene(scenes[i], LoadSceneMode.Additive);
                
                if(i < moveObjects.Length && moveObjects[i])
                {
                    StartCoroutine(MoveObjects(scenes[i], numAddedScenesLoaded));
                }
                
                addedSceneNames[numAddedScenesLoaded] = scenes[i];
                numAddedScenesLoaded++;
            }
            else if(!scenes[i].Equals(baseSceneName)) // If the scene is loaded and not the base scene, make sure the variables keeping track of which scene is loaded are still updated
            {
                int addedSceneArrayIndex = FindIndex(scenes[i]);
                if(addedSceneArrayIndex == -1)
                {
                    if(i < moveObjects.Length && moveObjects[i])
                    {
                        StartCoroutine(MoveObjects(scenes[i], numAddedScenesLoaded));
                    }
                    
                    addedSceneNames[numAddedScenesLoaded] = scenes[i];
                    numAddedScenesLoaded++;
                }
                else
                {
                    if(i < moveObjects.Length && moveObjects[i])
                    {
                        StartCoroutine(MoveObjects(scenes[i], addedSceneArrayIndex));
                    }
                }
            }
        }
    }
    
    private int FindIndex(string scene)
    {
        for(int i = 0; i < numAddedScenesLoaded; i++)
        {
            if(addedSceneNames[i].Equals(scene))
            {
                return i;
            }
        }
        
        return -1;
    }
    
    // Waits for the scene to load and then moves its objects to the base scene
    private IEnumerator MoveObjects(string scene, int objectArrayArrayIndex)
    {
        yield return new WaitUntil(() => SceneManager.GetSceneByName(scene).isLoaded);
        GameObject[] newAddedObjects = SceneManager.GetSceneByName(scene).GetRootGameObjects();
        foreach(GameObject obj in newAddedObjects)
        {
            SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByName(baseSceneName));
        }
        
        // Keeps track of all moved objects to be unloaded when the scene is unloaded, including objects moved previously in case someone tries to move objects from the same scene multiple times in a row and as such the scene isn't unloaded first
        if(addedObjects[objectArrayArrayIndex] != null)
        {
            GameObject[] oldAddedObjects = addedObjects[objectArrayArrayIndex];
            int totalLength = newAddedObjects.Length + oldAddedObjects.Length;
            addedObjects[objectArrayArrayIndex] = new GameObject[totalLength];
            for(int i = 0; i < addedObjects[objectArrayArrayIndex].Length; i++)
            {
                if(i < oldAddedObjects.Length)
                {
                    addedObjects[objectArrayArrayIndex][i] = oldAddedObjects[i];
                }
                else
                {
                    addedObjects[objectArrayArrayIndex][i] = newAddedObjects[i-oldAddedObjects.Length];
                }
            }
        }
        else
        {
            addedObjects[objectArrayArrayIndex] = newAddedObjects;
        }
    }
    
    /// <summary>
    /// If there is a second scene loaded on top of the base scene, unloads that scene and and any objects moved to the base scene from it while keeping the base scene loaded.
    /// </summary>
    private void UnloadScene()
    {
        if(numAddedScenesLoaded > 0)
        {
            if(addedObjects[0] != null)
            {
                foreach(GameObject obj in addedObjects[0])
                {
                    if(obj != null) // In case something else destroyed the object first
                    {
                        Destroy(obj);
                    }
                }
                
                for(int i = 0; i < maxAddedScenes-1; i++)
                {
                    addedObjects[i] = addedObjects[i+1];
                }
                
                addedObjects[maxAddedScenes-1] = null;
            }
            
            SceneManager.UnloadSceneAsync(addedSceneNames[0]);
            numAddedScenesLoaded--;
            for(int i = 0; i < maxAddedScenes-1; i++)
            {
                addedSceneNames[i] = addedSceneNames[i+1];
            }
        }
    }
    
    public void UnloadScenes(string[] scenes)
    {
        foreach(string scene in scenes)
        {
            UnloadScene(scene);
        }
    }
    
    public void UnloadScene(string scene)
    {
        int addedSceneArrayIndex = FindIndex(scene);
        if(addedSceneArrayIndex != -1)
        {
            if(addedObjects[addedSceneArrayIndex] != null)
            {
                foreach(GameObject obj in addedObjects[addedSceneArrayIndex])
                {
                    if(obj != null) // In case something else destroyed the object first
                    {
                        Destroy(obj);
                    }
                }
                
                for(int i = addedSceneArrayIndex; i < maxAddedScenes-1; i++)
                {
                    addedObjects[i] = addedObjects[i+1];
                }
                
                addedObjects[maxAddedScenes-1] = null;
            }
            
            SceneManager.UnloadSceneAsync(addedSceneNames[addedSceneArrayIndex]);
            numAddedScenesLoaded--;
            for(int i = addedSceneArrayIndex; i < maxAddedScenes-1; i++)
            {
                addedSceneNames[i] = addedSceneNames[i+1];
            }
        }
        else
        {
            //Debug.LogWarning($"Scene: {scene} was not loaded, thus could not be unloaded.");
        }
    }
}
