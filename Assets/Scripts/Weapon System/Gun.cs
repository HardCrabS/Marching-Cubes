using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public System.Action onPickUp;

    private void Awake()
    {
        onPickUp += InitializeUponPicking;
    }

    void InitializeUponPicking()
    {
        //GetComponentInChildren<Collider>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
