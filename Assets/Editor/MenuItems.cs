using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class MenuItems : MonoBehaviour 
{
    [MenuItem("Tools/Clear PlayerPrefs")]
    private static void ErasePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
