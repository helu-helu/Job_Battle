using UnityEngine;
using UnityEngine.UIElements;

public class UIPlayerStatus : MonoBehaviour
{
    private int totalRedEnergyGemPicked = 0;
    private int totalBlueEnergyGemPicked = 0;
    private int totalGreenEnergyGemPicked = 0;
    void OnEnable()
    {
        EnergyGemController.OnGemCollected += UpdateGemCount;
    }
    void OnDisable()
    {
        EnergyGemController.OnGemCollected -= UpdateGemCount;
    }
    private void UpdateGemCount(int gemType)
    {
        switch (gemType)
        {
            case 0:
                totalRedEnergyGemPicked++;
                totalRedEnergyGemPickedLabel.Q<Label>().text = totalRedEnergyGemPicked.ToString();
                break;
            case 1:
                totalBlueEnergyGemPicked++;
                totalBlueEnergyGemPickedLabel.Q<Label>().text = totalBlueEnergyGemPicked.ToString();
                break;
            case 2:
                totalGreenEnergyGemPicked++;
                totalGreenEnergyGemPickedLabel.Q<Label>().text = totalGreenEnergyGemPicked.ToString();
                break;
        }
    }
    private VisualElement playerStatusPanel;

    private VisualElement energyGemStatus;
    private VisualElement redEnergyGem;
    private VisualElement blueEnergyGem;
    private VisualElement greenEnergyGem;
    private VisualElement totalRedEnergyGemPickedLabel;
    private VisualElement totalBlueEnergyGemPickedLabel;
    private VisualElement totalGreenEnergyGemPickedLabel;
    void Awake()
    {
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        playerStatusPanel = root.Q<VisualElement>("playerStatusPanel");
        energyGemStatus = playerStatusPanel.Q<VisualElement>("energyGemStatus");
        redEnergyGem = energyGemStatus.Q<VisualElement>("redEnergyGem");
        blueEnergyGem = energyGemStatus.Q<VisualElement>("blueEnergyGem");
        greenEnergyGem = energyGemStatus.Q<VisualElement>("greenEnergyGem");
        totalRedEnergyGemPickedLabel = redEnergyGem.Q<VisualElement>("totalRedEnergyGemPickedLabel");
        totalBlueEnergyGemPickedLabel = blueEnergyGem.Q<VisualElement>("totalBlueEnergyGemPickedLabel");
        totalGreenEnergyGemPickedLabel = greenEnergyGem.Q<VisualElement>("totalGreenEnergyGemPickedLabel");
    }


}
