using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    public Slider healthSlider;
    public Image healthFillImage;
    public Slider armorSlider;
    public Image armorFillImage;
    
    void Start()
    {
        EventsDispatcher.Instance.onPlayerHealthUpdated += HandleHealthChange;
    }

    void HandleHealthChange(HealthData healthData)
    {
        armorSlider.value = (float)healthData.armorInfo.Item1 / healthData.armorInfo.Item2;
        if (armorSlider.value <= 0)
        {
            armorFillImage.enabled = false;
        }

        healthSlider.value = healthData.healthInfo.Item1 / healthData.healthInfo.Item2;
        if (healthSlider.value <= 0)
        {
            healthFillImage.enabled = false;
        }
    }
}
