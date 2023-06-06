using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RegionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public RegionData regionData;

    [Header("Colors")]
    public Color lockedColor = Color.gray;
    public Color lockedHighlightedColor = Color.white;
    public Color unlockedColor = Color.blue;
    public Color unlockedHighlightedColor = Color.cyan;

    Image image;
    IslandsContainer islandsContainer;

    private void Start()
    {
        image = GetComponent<Image>();
        image.alphaHitTestMinimumThreshold = 0.5f;
        islandsContainer = GetComponentInChildren<IslandsContainer>();

        UpdateRegion();

        GetComponentInChildren<LockButton>().SetTooltip(regionData.islandsRequiredToUnlock.ToString());
        GetComponentInParent<IslandSelectView>().onIslandUnlocked += (islandData) => {
            UpdateRegion(); 
        };
    }

    // Called when the mouse pointer enters the image area
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = GetColor(isHighlighted: true);
    }

    // Called when the mouse pointer exits the image area
    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = GetColor(isHighlighted: false);
    }

    // Called when the image is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        // Perform your click action here
        Debug.Log("Image clicked!");
    }

    void UpdateRegion()
    {
        image.color = GetColor(isHighlighted: false);
        bool isUnlocked = IsUnlocked();
        if (isUnlocked)
        {
            GetComponentInChildren<LockButton>().gameObject.SetActive(false);
            islandsContainer.gameObject.SetActive(true);
            islandsContainer.InitIslands();
        }
        else
        {
            islandsContainer.gameObject.SetActive(false);
        }
    }

    bool IsUnlocked()
    {
        return PlayerPrefsController.GetUnlockedIslandsCount() >= regionData.islandsRequiredToUnlock;
    }

    Color GetColor(bool isHighlighted)
    {
        if (!IsUnlocked())
            return isHighlighted ? lockedHighlightedColor : lockedColor;

        return isHighlighted ? unlockedHighlightedColor : unlockedColor;
    }
}
