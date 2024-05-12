using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthManagerEnemyScript : MonoBehaviour
{
    public Slider Slider;

    public Vector3 offset;

    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {
        Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }

    public void SetHealth(int health , int maxHealth){

        Slider.gameObject.SetActive(health < maxHealth);
        Slider.value = health;
        Slider.maxValue = maxHealth;
        Color green = new Color(0, 1, 0, 1);
        Color red = new Color(1, 0, 0, 1);
        Slider.gameObject.transform.Find("Background").GetComponent<Image>().color = Color.Lerp(red, green, Slider.normalizedValue);

    }
}
