using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Image fillImage;
    public Gradient healthGradient;

    public void SetMaxHealth(int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        fillImage.color = healthGradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        healthSlider.value = health;
        fillImage.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }

    void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
