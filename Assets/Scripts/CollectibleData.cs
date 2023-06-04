using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Collectibles")]
public class CollectibleData : ScriptableObject
{
    public string title;
    [TextArea(2, 5)] public string description;
}
