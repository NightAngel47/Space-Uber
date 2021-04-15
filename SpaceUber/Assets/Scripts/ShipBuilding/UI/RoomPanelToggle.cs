/* Frank Calabrese
 * RoomPanelToggle.cs
 * various controls for the shipbuilding, radio, and crew management UI
 */

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
            detailsMenu.UnHighlight();
            ClosePanel();
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
                tabs[i].SetActive(i == tabIndex);
            }
        }
        
        panelAnimator.SetBool(IsOpen, true);
        isOpen = true;

        if (tabs.Length > 0)
        {
            if (tabs[0].name == "Room Details")
            {
                //enables crew view when room details panel gets opened
                CrewViewManager.Instance.EnableCrewView();
            }
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

            if (comingFromOpenPanel == true && tabs[0].name == "Room Details")
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
