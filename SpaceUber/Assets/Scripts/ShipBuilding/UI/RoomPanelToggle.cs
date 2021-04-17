/* Frank Calabrese
 * RoomPanelToggle.cs
 * various controls for the shipbuilding, radio, and crew management UI
 */

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomPanelToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator panelAnimator;
    private bool isOpen;

    [SerializeField] Sprite blackButton;
    [SerializeField] Sprite redButton;
    [SerializeField] private Image[] panelTabs = new Image[0];
    [SerializeField] GameObject[] tabs = new GameObject[0];
    
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private GameObject helpButton;
    
    private int currentTabIndex = -1;
    private bool isMouseOverObject;
    private CrewManagementRoomDetailsMenu detailsMenu;
    
    private static readonly int IsOpen = Animator.StringToHash("isOpen");

    private void Awake()
    {
        panelAnimator = GetComponent<Animator>();
        detailsMenu = FindObjectOfType<CrewManagementRoomDetailsMenu>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && 
            ObjectMover.hasPlaced &&
            !ObjectScript.roomIsHovered && 
            !isMouseOverObject && 
            !Tutorial.Instance.GetTutorialActive())
        {
            ClosePanel();
            
            if (tabs.Length > 0 && tabs[0].name == "Room Details")
            {
                detailsMenu.UnHighlight();
                CrewViewManager.Instance.DisableCrewView();
                detailsMenu.selectedRoom = null;
            }
        }

        if (isOpen && EventSystem.instance.eventActive)
        {
            ClosePanel();
        }

        if (!isOpen) Tutorial.Instance.UnHighlightScreenLocation();
    }

    public bool GetIsOpen()
    {
        return isOpen;
    }

    public void TogglePanelVis(int tabIndex = -1)
    {
        if (isOpen && currentTabIndex == tabIndex)
        {
            ClosePanel(tabIndex);
            //disables crew view when room details panel gets closed
            CrewViewManager.Instance.DisableCrewView();
            detailsMenu.UnHighlight();
        }
        else if(currentTabIndex != tabIndex)
        {
            OpenPanel(tabIndex);
        }
    }

    public void OpenPanel(int tabIndex = -1)
    {
        if (OverclockController.instance.overclocking) return;

        SetSelectedTab(tabIndex, true);

        if (tabs.Length > 0)
        {
            for (int i = 0; i < tabs.Length; i++)
            {
                if (i == 0)
                {
                    //disables only the UI imagery, not the object with the script on it
                    tabs[0].gameObject.transform.GetChild(0).gameObject.SetActive(i == tabIndex);
                }
                else
                {
                    tabs[i].SetActive(i == tabIndex);
                }
            }
        }
        
        panelAnimator.SetBool(IsOpen, true);
        isOpen = true;

        if (tabs.Length > 0 && tabs[0].name == "Room Details" && tabIndex == 0)
        {
            //enables crew view when room details panel gets opened
            CrewViewManager.Instance.EnableCrewView();
                
            // set header text and help button for room details for when selecting a room with the mouse
            if(headerText) headerText.text = tabs[0].name; 
            if(helpButton) helpButton.SetActive(true);
        }
    }

    public void ClosePanel(int tabIndex = -1)
    {
        if (!isOpen) return;
        
        try
        {
            if (tabIndex == 0) detailsMenu.UnHighlight();
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError("The room details menu doesn't exist. You're probably in ship building.");
        }

        panelAnimator.SetBool(IsOpen, false);
        isOpen = false;
        
        if (tabIndex != -1)
        {
            SetSelectedTab(tabIndex, false);
        }
        else
        {
            foreach (Image panelTab in panelTabs)
            {
                panelTab.sprite = blackButton;
                currentTabIndex = -1;
            }
        }
    }

    private void SetSelectedTab(int tabIndex, bool comingFromOpenPanel)
    {
        if (currentTabIndex == tabIndex)
        {

            if (comingFromOpenPanel == true && tabs[0].name == "Room Details" && tabIndex == 0)
            {
                //for room details panel, don't turn off active tab button
            }
            else
            {
                panelTabs[currentTabIndex].sprite = blackButton;
                currentTabIndex = -1;
            }
        }
        else
        {
            if(currentTabIndex != -1) panelTabs[currentTabIndex].sprite = blackButton;
            currentTabIndex = tabIndex;
            panelTabs[currentTabIndex].sprite = redButton;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOverObject = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOverObject = false;
    }
}
