using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleView : MonoBehaviour
{
    public SequentialText titleText;
    public SequentialText descrText;

    void Start()
    {
        Clear();

        EventsDispatcher.Instance.onCollectibleFound += (cData) => { StartCoroutine(AnimateText(cData)); };
        EventsDispatcher.Instance.onEscape += Clear;
    }

    IEnumerator AnimateText(CollectibleData collectibleData)
    {
        titleText.PlayMessage(collectibleData.title);
        yield return new WaitUntil(() => !titleText.PlayingMessage);
        descrText.PlayMessage(collectibleData.description);
    }

    void Clear()
    {
        titleText.Clear();
        descrText.Clear();
    }
}
