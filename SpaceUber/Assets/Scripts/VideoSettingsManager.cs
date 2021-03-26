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
    
    public enum DropdownSettings { NA = -1, Resolution, AspectRatio, TargetFrameRate}
    public enum CheckboxSettings { NA = -1, FullScreen, VSync}
    
    private int[] dropdownValues = new int[3];
    private bool[] checkboxValues = new bool[2];
    
    private List<int> resolutions = new List<int>();
    private List<float> ratios = new List<float>();
    
    private void Start()
    {
        for(int i = 0; i < dropdownMenus.Length; i++)
        {
            dropdownMenus[i].ClearOptions();
            List<string> options = new List<string>();
            
            switch(i)
            {
                case (int) DropdownSettings.Resolution:
                    int lastWidth = 0;
                    for (int j = 0; j < Screen.resolutions.Length; j++)
                    {
                        if(Screen.resolutions[j].width != lastWidth)
                        {
                            options.Add(Screen.resolutions[j].width + "p");
                            lastWidth = Screen.resolutions[j].width;
                            resolutions.Add(Screen.resolutions[j].width);
                        }
                    }
                    break;
                case (int) DropdownSettings.AspectRatio:
                    foreach (var t in Screen.resolutions)
                    {
                        bool exists = false;
                        foreach(float ratio in ratios)
                        {
                            if(t.width / (float) t.height == ratio)
                            {
                                exists = true;
                                break;
                            }
                        }
                        
                        if(!exists)
                        {
                            int gcd = GCD(t.width, t.height);
                            options.Add(t.width/gcd + ":" + t.height/gcd);
                            ratios.Add(t.width / (float) t.height);
                        }
                    }
                    break;
                case (int) DropdownSettings.TargetFrameRate:
                    for (int j = 0; j < frameRateOptions.Length; j++)
                    {
                        if(frameRateOptions[j] == -1)
                            options.Add("Unlimited");
                        else
                            options.Add(frameRateOptions[j] + " fps");
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
        Screen.SetResolution(resolutions[dropdownValues[(int) DropdownSettings.Resolution]], (int) (resolutions[dropdownValues[(int) DropdownSettings.Resolution]] / ratios[dropdownValues[(int) DropdownSettings.AspectRatio]]), Screen.fullScreen);
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
                    int lastWidth = 0;
                    for (int j = 0; j < Screen.resolutions.Length; j++)
                    {
                        if(Screen.resolutions[j].width != lastWidth)
                        {
                            lastWidth = Screen.resolutions[j].width;
                            resolutions.Add(Screen.resolutions[j].width);
                        }
                    }
                    break;
                case (int) DropdownSettings.AspectRatio:
                    for (int j = 0; j < Screen.resolutions.Length; j++)
                    {
                        bool exists = false;
                        foreach(float ratio in ratios)
                        {
                            if(Screen.resolutions[j].width / (float) Screen.resolutions[j].height == ratio)
                            {
                                exists = true;
                                break;
                            }
                        }
                        
                        if(!exists)
                        {
                            ratios.Add(Screen.resolutions[j].width / (float) Screen.resolutions[j].height);
                        }
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
            Screen.SetResolution(resolutions[dropdownSelections[(int) DropdownSettings.Resolution]], (int) (resolutions[dropdownSelections[(int) DropdownSettings.Resolution]] / ratios[dropdownSelections[(int) DropdownSettings.AspectRatio]]), Screen.fullScreen);
            QualitySettings.vSyncCount = checkboxSelections[(int) CheckboxSettings.VSync] ? 1 : 0;
            Application.targetFrameRate = frameRateOptions[dropdownSelections[(int) DropdownSettings.TargetFrameRate]];
        }
        else
        {
            Screen.fullScreen = checkboxDefaults[(int) CheckboxSettings.FullScreen];
            Screen.SetResolution(resolutions[dropdownDefaults[(int) DropdownSettings.Resolution]], (int) (resolutions[dropdownDefaults[(int) DropdownSettings.Resolution]] / ratios[dropdownDefaults[(int) DropdownSettings.AspectRatio]]), Screen.fullScreen);
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
        Screen.SetResolution(resolutions[dropdownValues[(int) DropdownSettings.Resolution]], (int) (resolutions[dropdownValues[(int) DropdownSettings.Resolution]] / ratios[dropdownValues[(int) DropdownSettings.AspectRatio]]), Screen.fullScreen);
    }
    
    public void UpdateAspectRatio()
    {
        dropdownValues[(int) DropdownSettings.AspectRatio] = dropdownMenus[(int) DropdownSettings.AspectRatio].value;
        SavingLoadingManager.instance.Save<int[]>("videoSettingsDropdowns", dropdownValues);
        Screen.SetResolution(resolutions[dropdownValues[(int) DropdownSettings.Resolution]], (int) (resolutions[dropdownValues[(int) DropdownSettings.Resolution]] / ratios[dropdownValues[(int) DropdownSettings.AspectRatio]]), Screen.fullScreen);
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
