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
    private int currentTabIndex = -1;
    private bool isMouseOverObject;
    
    private static readonly int IsOpen = Animator.StringToHash("isOpen");

    private void Start()
    {
        panelAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && 
            GameManager.instance.currentGameState != InGameStates.ShipBuilding && 
            !ObjectScript.roomIsHovered && !isMouseOverObject)
        {
            ClosePanel();
        }

        if (!isOpen) Tutorial.Instance.UnHighlightScreenLocation();
    }

    public void TogglePanelVis(int tabIndex = -1)
    {
        if (isOpen)
        {
            ClosePanel(tabIndex);
        }
        else
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
        }

        if (isOpen) return;
        try
        {
            gameObject.GetComponentInChildren<CrewManagementRoomDetailsMenu>().toggleHighlight();//
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
            gameObject.GetComponentInChildren<CrewManagementRoomDetailsMenu>().toggleHighlight();//
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
