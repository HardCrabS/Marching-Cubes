using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public virtual void Kill()
    {
        Destroy(gameObject);
    }
}
