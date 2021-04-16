using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoSettingsManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown[] dropdownMenus;
    [SerializeField] private Toggle[] checkboxes;
    [SerializeField] private int[] dropdownDefaults;
    [SerializeField] private bool[] checkboxDefaults;
    
    [SerializeField] private int[] frameRateOptions;
    
    public enum DropdownSettings { NA = -1, Resolution, TargetFrameRate}
    public enum CheckboxSettings { NA = -1, FullScreen, VSync}
    
    private int[] dropdownValues = new int[2];
    private bool[] checkboxValues = new bool[2];
    
    private List<Vector2> resolutions = new List<Vector2>();
    
    private void Start()
    {
        for(int i = 0; i < dropdownMenus.Length; i++)
        {
            dropdownMenus[i].ClearOptions();
            List<string> options = new List<string>();
            
            switch(i)
            {
                case (int) DropdownSettings.Resolution:
                    int lastW = 0;
                    int lastH = 0;
                    foreach (Resolution resolution in Screen.resolutions)
                    {
                        if (resolution.width == lastW && resolution.height == lastH) continue;
                        options.Add(resolution.width + "\tx   " + resolution.height);
                        resolutions.Add(new Vector2(resolution.width, resolution.height));
                        lastW = resolution.width;
                        lastH = resolution.height;
                    }
                    break;
                case (int) DropdownSettings.TargetFrameRate:
                    foreach (int frameRate in frameRateOptions)
                    {
                        if(frameRate == -1)
                            options.Add("Unlimited FPS");
                        else
                            options.Add(frameRate + "\tFPS");
                    }
                    break;
                default:
                    break;
            }
            
            dropdownMenus[i].AddOptions(options);
        }
        
        if(SavingLoadingManager.instance.GetHasSettingsSaved())
        {
            int[] dropdownSelections = SavingLoadingManager.instance.Load<int[]>("videoSettingsDropdowns");
            for(int i = 0; i < dropdownMenus.Length; i++)
            {
                dropdownMenus[i].value = dropdownSelections[i];
                dropdownValues = dropdownSelections;
            }
            
            bool[] checkboxSelections = SavingLoadingManager.instance.Load<bool[]>("videoSettingsCheckboxes");
            for(int i = 0; i < checkboxes.Length; i++)
            {
                checkboxes[i].isOn = checkboxSelections[i];
                checkboxValues = checkboxSelections;
            }
        }
        else
        {
            for (var i = 0; i < resolutions.Count; i++)
            {
                Vector2 resolution = resolutions[i];
                if (resolution.x == Display.main.systemWidth && resolution.y == Display.main.systemHeight)
                {
                    dropdownDefaults[(int) DropdownSettings.Resolution] = i;
                    break;
                }
            }
            
            for(int i = 0; i < dropdownMenus.Length; i++)
            {
                dropdownMenus[i].value = dropdownDefaults[i];
                dropdownValues = dropdownDefaults;
            }
            
            SavingLoadingManager.instance.Save<int[]>("videoSettingsDropdowns", dropdownDefaults);
            
            for(int i = 0; i < checkboxes.Length; i++)
            {
                checkboxes[i].isOn = checkboxDefaults[i];
                checkboxValues = checkboxDefaults;
            }
            
            SavingLoadingManager.instance.Save<bool[]>("videoSettingsCheckboxes", checkboxDefaults);
            
            SavingLoadingManager.instance.NewSettingsSave();
        }
        
        Screen.fullScreen = checkboxValues[(int) CheckboxSettings.FullScreen];
        Screen.SetResolution((int) resolutions[dropdownValues[(int) DropdownSettings.Resolution]].x, (int) resolutions[dropdownValues[(int) DropdownSettings.Resolution]].y, Screen.fullScreen);
        QualitySettings.vSyncCount = checkboxValues[(int) CheckboxSettings.VSync] ? 1 : 0;
        Application.targetFrameRate = frameRateOptions[dropdownValues[(int) DropdownSettings.TargetFrameRate]];
    }
    
    private int GCD(int a, int b)
    {
        int limit = (int) Mathf.Min((float) a, (float) b);
        for(int i = limit; i > 0; i--)
        {
            if(a%i == 0 && b%i == 0)
            {
                return i;
            }
        }
        return 1;
    }
    
    public void UpdateSettingsEarly()
    {
        for(int i = 0; i < dropdownMenus.Length; i++)
        {
            switch(i)
            {
                case (int) DropdownSettings.Resolution:
                    int lastW = 0;
                    int lastH = 0;
                    foreach (Resolution resolution in Screen.resolutions)
                    {
                        if (resolution.width == lastW && resolution.height == lastH) continue;
                        resolutions.Add(new Vector2(resolution.width, resolution.height));
                        lastW = resolution.width;
                        lastH = resolution.height;
                    }
                    break;
                case (int) DropdownSettings.TargetFrameRate:
                    break;
                default:
                    break;
            }
        }
        
        if(SavingLoadingManager.instance.GetHasSettingsSaved())
        {
            int[] dropdownSelections = SavingLoadingManager.instance.Load<int[]>("videoSettingsDropdowns");
            bool[] checkboxSelections = SavingLoadingManager.instance.Load<bool[]>("videoSettingsCheckboxes");
            
            Screen.fullScreen = checkboxSelections[(int) CheckboxSettings.FullScreen];
            Screen.SetResolution((int) resolutions[dropdownSelections[(int) DropdownSettings.Resolution]].x, (int) resolutions[dropdownSelections[(int) DropdownSettings.Resolution]].y, Screen.fullScreen);
            QualitySettings.vSyncCount = checkboxSelections[(int) CheckboxSettings.VSync] ? 1 : 0;
            Application.targetFrameRate = frameRateOptions[dropdownSelections[(int) DropdownSettings.TargetFrameRate]];
        }
        else
        {
            
            for (var i = 0; i < resolutions.Count; i++)
            {
                Vector2 resolution = resolutions[i];
                if (resolution.x == Display.main.systemWidth && resolution.y == Display.main.systemHeight)
                {
                    dropdownDefaults[(int) DropdownSettings.Resolution] = i;
                    break;
                }
            }
            
            Screen.fullScreen = checkboxDefaults[(int) CheckboxSettings.FullScreen];
            Screen.SetResolution((int) resolutions[dropdownDefaults[(int) DropdownSettings.Resolution]].x, (int) resolutions[dropdownDefaults[(int) DropdownSettings.Resolution]].y, Screen.fullScreen);
            QualitySettings.vSyncCount = checkboxDefaults[(int) CheckboxSettings.VSync] ? 1 : 0;
            Application.targetFrameRate = frameRateOptions[dropdownDefaults[(int) DropdownSettings.TargetFrameRate]];
        }
    }
    
    public void UpdateFullScreen()
    {
        checkboxValues[(int) CheckboxSettings.FullScreen] = checkboxes[(int) CheckboxSettings.FullScreen].isOn;
        SavingLoadingManager.instance.Save<bool[]>("videoSettingsCheckboxes", checkboxValues);
        Screen.fullScreen = checkboxValues[(int) CheckboxSettings.FullScreen];
    }
    
    public void UpdateResolution()
    {
        dropdownValues[(int) DropdownSettings.Resolution] = dropdownMenus[(int) DropdownSettings.Resolution].value;
        SavingLoadingManager.instance.Save<int[]>("videoSettingsDropdowns", dropdownValues);
        Screen.SetResolution((int) resolutions[dropdownValues[(int) DropdownSettings.Resolution]].x, (int) resolutions[dropdownValues[(int) DropdownSettings.Resolution]].y, Screen.fullScreen);
    }
    
    public void UpdateVSync()
    {
        checkboxValues[(int) CheckboxSettings.VSync] = checkboxes[(int) CheckboxSettings.VSync].isOn;
        SavingLoadingManager.instance.Save<bool[]>("videoSettingsCheckboxes", checkboxValues);
        QualitySettings.vSyncCount = checkboxValues[(int) CheckboxSettings.VSync] ? 1 : 0;
    }
    
    public void UpdateFrameRate()
    {
        dropdownValues[(int) DropdownSettings.TargetFrameRate] = dropdownMenus[(int) DropdownSettings.TargetFrameRate].value;
        SavingLoadingManager.instance.Save<int[]>("videoSettingsDropdowns", dropdownValues);
        Application.targetFrameRate = frameRateOptions[dropdownValues[(int) DropdownSettings.TargetFrameRate]];
    }
}
