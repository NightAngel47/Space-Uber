/*
 * JobSelectCanvas.cs
 * Author(s): Scott Acler
 * Created on: 10/20/2020 (en-US)
 * Description: Stores UI information about which text boxes and panels should be used by JobSelectScreen
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JobSelectCanvas : MonoBehaviour
{
    public GameObject detailsPanel;
    public TMP_Text titleBox;
    public TMP_Text descriptionBox;
    public TMP_Text rewardBox;
    public Transform buttonGroup;
}
