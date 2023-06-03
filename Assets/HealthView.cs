using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    public Slider healthSlider;
    public Image healthFillImage;
    public Slider armorSlider;
    
    void Start()
    {
        EventsDispatcher.Instance.onPlayerHealthUpdated += HandleHealthChange;
    }

    void HandleHealthChange(float currHp, float maxHp)
    {
        healthSlider.value = currHp / maxHp;
        if (healthSlider.value <= 0)
        {
            healthFillImage.enabled = false;
        }
    }
}
