using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasePage : MonoBehaviour
{
    private UIManager uiManager;
    private GameManager gameManager;

    public GameObject healthSlider;
    public GameObject shieldSlider;
    public GameObject endurenceSlider;
    public GameObject shotcut;

    public GameObject icon0;
    public GameObject icon1;
    public GameObject icon2;
    public GameObject icon3;
    // Start is called before the first frame update
    void Start()
    {
        uiManager = UIManager.Instance;
        gameManager = GameManager.Instance;
        uiManager.registerBasePageCtrl(this);
        changeShotcut0(0);
    }

    public void changeHealth(float health, float shield)
    {
        float healthValue = (health - shield) / TamaBaseInfo.MAX_HEALTH;
        float shieldValue = health / TamaBaseInfo.MAX_HEALTH;
        healthSlider.GetComponent<Slider>().value = healthValue;
        shieldSlider.GetComponent<Slider>().value = shieldValue;
    }

    public void changeEndurence(float endurence) {
        float endurenceValue = endurence / TamaBaseInfo.MAX_ENDURENCE;
        endurenceSlider.GetComponent<Slider>().value = endurenceValue;
    }

    public void changeShotcut0(int index)
    {
        Sprite icon = gameManager.resource.getAbilityIcon(index);
        icon0.GetComponent<Image>().sprite = icon;
    }
    public void changeShotcut1(int index)
    {

    }
    public void changeShotcut2(int index)
    {

    }
    public void changeShotcut3(int index)
    {

    }

    public void changeTime(string time)
    {

    }

    public void changeTemprature(int temperature) { 
    }

    public void changeDetector(int value)
    {

    }
}
