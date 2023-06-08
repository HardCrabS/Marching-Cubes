using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureCross : MonoBehaviour
{
    [Tooltip("Distance to player when cross should become invisible")]
    public float disappearDistance = 3f;

    Transform playerTransform;
    Image crossImage; 

    void Start()
    {
        playerTransform = Player.Instance.transform;
        crossImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        crossImage.enabled = Vector3.Distance(transform.position, playerTransform.position) > disappearDistance;
    }
}
