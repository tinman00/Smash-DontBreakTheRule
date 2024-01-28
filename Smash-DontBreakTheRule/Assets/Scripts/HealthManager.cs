using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Image Healthbar1;
    public Image Healthbar2;
    public GameObject player1;
    public GameObject player2;
    private float maxhealth1;
    private float maxhealth2;
    private float health1;
    private float health2;
    public Text PlayerLifeText1;
    public Text PlayerLifeText2;
    private int playerlife1;
    private int playerlife2;
    void Start()
    {
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        HealthManagement();
    }
    void HealthManagement() {
        
        playerlife1 = player1.GetComponent<Player>().Getotallifecount();
        playerlife2 = player2.GetComponent<Player>().Getotallifecount();
        maxhealth1 = player1.GetComponent<Player>().GetMaxHP();
        maxhealth2 = player2.GetComponent<Player>().GetMaxHP();
        health1 = player1.GetComponent<Player>().GetHP();
        health2 = player2.GetComponent<Player>().GetHP();
        Healthbar1.fillAmount = health1 / maxhealth1;
        Healthbar2.fillAmount = health2 / maxhealth2;
        PlayerLifeText1.text = playerlife1.ToString();
        PlayerLifeText2.text = playerlife2.ToString();

    }
}
