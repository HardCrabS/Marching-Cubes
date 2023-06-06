using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Island selection/Island")]
public class IslandData : ScriptableObject
{
    public string title;
    [TextArea(4, 10)]
    public string description;
    public int reward;
    public int price;
}
