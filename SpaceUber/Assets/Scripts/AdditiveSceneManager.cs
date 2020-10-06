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
    public bool addSceneOnStart;
    
    /// <summary>
    /// The build settings id of the scene that is loaded if <c>addSceneOnStart</c> is set to true.
    /// </summary>
    public int startingAddedScene;
    
    /// <summary>
    /// When set to true, the objects from <c>startingAddedScene</c> are moved to the base scene if <c>startingAddedScene</c> is loaded at the start.
    /// </summary>
    public bool moveStartingObjects;
    
    // The index of the second loaded scene
    private int addedSceneIndex;
    
    // The index of the base scene
    private int baseSceneIndex;
    
    // True if a second scene is loaded, false if the base scene is the only scene loaded
    private bool addedSceneLoaded;
    
    // Keeps track of objects from added scenes that were moved to the base scene so unloading the added scene can still remove the objects
    private GameObject[] addedObjects;
    
    // Start is called before the first frame update
    void Start()
    {
        // The scene containing this script's game object is the base scene
        baseSceneIndex = this.gameObject.scene.buildIndex;
        
        // Unloads all scenes that are not supposed to be loaded, just in case.  Doesn't unload scenes that are supposed to be loaded
        for(int i = SceneManager.sceneCount-1; i >= 0; i--)
        {
            int buildIndex = SceneManager.GetSceneAt(i).buildIndex;
            if(buildIndex != baseSceneIndex && (buildIndex != startingAddedScene || !addSceneOnStart))
            {
                SceneManager.UnloadSceneAsync(buildIndex);
            }
        }
        
        // Sets the private variables
        addedSceneLoaded = false;
        addedObjects = null;
        
        // Automatically loads the second scene if it is set to be loaded
        if(addSceneOnStart)
        {
            LoadScene(startingAddedScene, moveStartingObjects);
        }
    }
    
    /// <summary>
    /// Loads the specified scene without moving objects to the base scene, which is probably better for performance but can cause issues if objects from different scenes are supposed to interract.  The base scene stays loaded and any other scenes are unloaded first.
    /// </summary>
    /// <param name="scene">The integer id of the scene in the build settings.</param>
    public void LoadSceneSeperate(int scene)
    {
        LoadScene(scene, false);
    }
    
    /// <summary>
    /// Loads the specified scene and moves its objects to the base scene, allowing objects from different scenes to easily interract.  The base scene stays loaded and any other scenes are unloaded first.
    /// </summary>
    /// <param name="scene">The integer id of the scene in the build settings.</param>
    public void LoadSceneMerged(int scene)
    {
        LoadScene(scene, true);
    }
    
    // Actually contains the scene loading code.  I made this function private and split it into two public ones because I don't think UI buttons can be linked to functions with multiple parameters
    private void LoadScene(int scene, bool moveObjects)
    {
        if(!SceneManager.GetSceneByBuildIndex(scene).isLoaded) // If the scene's not already loaded, unload any other added scene and load the new scene
        {
            UnloadScene();
            
            SceneManager.LoadScene(scene, LoadSceneMode.Additive);
            
            addedSceneLoaded = true;
            addedSceneIndex = scene;
        }
        else if(scene != baseSceneIndex) // If the scene is loaded and not the base scene, make sure the variables keeping track of which scene is loaded are still updated
        {
            addedSceneIndex = scene;
            addedSceneLoaded = true;
        }
        
        if(moveObjects && scene != baseSceneIndex) // Whether or not the scene was already loaded previously, move its objects to the base scene if moveObjects is true
        {
            StartCoroutine(MoveObjects(scene));
        }
    }
    
    // Waits for the scene to load and then moves its objects to the base scene
    private IEnumerator MoveObjects(int scene)
    {
        yield return new WaitUntil(() => SceneManager.GetSceneByBuildIndex(scene).isLoaded);
        GameObject[] newAddedObjects = SceneManager.GetSceneByBuildIndex(scene).GetRootGameObjects();
        foreach(GameObject obj in newAddedObjects)
        {
            SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByBuildIndex(baseSceneIndex));
        }
        
        // Keeps track of all moved objects to be unloaded when the scene is unloaded, including objects moved previously in case someone tries to move objects from the same scene multiple times in a row and as such the scene isn't unloaded first
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
    
    /// <summary>
    /// If there is a second scene loaded on top of the base scene, unloads that scene and and any objects moved to the base scene from it while keeping the base scene loaded.
    /// </summary>
    public void UnloadScene()
    {
        if(addedSceneLoaded)
        {
            if(addedObjects != null)
            {
                foreach(GameObject obj in addedObjects)
                {
                    if(obj != null) // In case something else destroyed the object first
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
