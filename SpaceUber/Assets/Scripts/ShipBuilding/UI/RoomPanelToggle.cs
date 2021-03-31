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
    [SerializeField] GameObject[] tabs;
    private int currentTabIndex = -1;
    private bool isMouseOverObject;
    private CrewManagementRoomDetailsMenu detailsMenu;
    
    private static readonly int IsOpen = Animator.StringToHash("isOpen");

    private void Start()
    {
        panelAnimator = GetComponent<Animator>();
        detailsMenu = gameObject.GetComponentInChildren<CrewManagementRoomDetailsMenu>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && 
            GameManager.instance.currentGameState != InGameStates.ShipBuilding && 
            !ObjectScript.roomIsHovered && !isMouseOverObject && !Tutorial.Instance.GetTutorialActive())
        {
            detailsMenu.unHighlight();
            ClosePanel();
        }

        if (!isOpen) Tutorial.Instance.UnHighlightScreenLocation();
    }

    public void TogglePanelVis(int tabIndex = -1)
    {
        if (isOpen && currentTabIndex == tabIndex)
        {
            ClosePanel(tabIndex);
        }
        else if(currentTabIndex != tabIndex)
        {
            OpenPanel(tabIndex);
        }
    }

    public void OpenPanel(int tabIndex = -1, bool isRoomDetails = false)
    {
        if (OverclockController.instance.overclocking) return;

        if (!isRoomDetails || currentTabIndex != 0)
        {
            SetSelectedTab(tabIndex);

            //swap between radio and room details
            if(GameManager.instance.currentGameState != InGameStates.ShipBuilding)
            {
                for (int i = 0; i < tabs.Length; i++)
                {
                    if (i == tabIndex)
                    {
                        if (tabs[i] == tabs[0]) detailsMenu.highlight();
                        tabs[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        if (tabs[i] == tabs[0]) detailsMenu.unHighlight();
                        tabs[i].gameObject.SetActive(false);
                    }
                }
                
            }
            
        }

        if (isOpen) return;
        try
        {
            if (tabIndex == 0) detailsMenu.highlight();//
        }
        catch(System.NullReferenceException)
        {
            Debug.LogError("The room details menu doesn't exist. You're probably in ship building.");
        }
        

        panelAnimator.SetBool(IsOpen, true);
        isOpen = true;
    }

    public void ClosePanel(int tabIndex = -1)
    {
        if (!isOpen) return;

        try
        {
            if (tabIndex == 0) detailsMenu.unHighlight();//
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError("The room details menu doesn't exist. You're probably in ship building.");
        }

        panelAnimator.SetBool(IsOpen, false);
        isOpen = false;
        
        if (tabIndex != -1)
        {
            SetSelectedTab(tabIndex);
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

    private void SetSelectedTab(int tabIndex)
    {
        if (currentTabIndex == tabIndex)
        {
            panelTabs[currentTabIndex].sprite = blackButton;
            currentTabIndex = -1;
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
