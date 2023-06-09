using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IslandButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public IslandData islandData;
    public Quest Quest { get; private set; }

    Image image;
    Vector3 initSize;

    public void Initialize()
    {
        image = GetComponent<Image>();
        initSize = transform.localScale;

        UpdateButton(isHighlighted: false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UpdateButton(isHighlighted: true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UpdateButton(isHighlighted: false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var viewData = CreateViewData();
        GetComponentInParent<IslandSelectView>().onIslandClicked?.Invoke(viewData);
    }

    IslandViewData CreateViewData()
    {
        IslandViewData islandViewData = new IslandViewData();
        islandViewData.isUnlocked = IsUnlocked();
        islandViewData.islandData = islandData;
        islandViewData.islandButton = this;

        return islandViewData;
    }

    public void UpdateButton(bool isHighlighted)
    {
        transform.localScale = isHighlighted ? initSize * 1.3f : initSize;
        bool isUnlocked = IsUnlocked();
        image.color = isUnlocked ? Color.white : Color.black;
        if (isUnlocked)
        {
            if (Quest == null)
                Quest = QuestSystem.Instance.GenerateQuest();
        }
    }

    bool IsUnlocked()
    {
        return islandData.price == 0 || PlayerPrefsController.IsIslandUnlocked(islandData.title);
    }
}
