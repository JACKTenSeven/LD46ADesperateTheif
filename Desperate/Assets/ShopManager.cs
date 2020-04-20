using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ShopManager : MonoBehaviour
{
    public Text moneyT;

    void Awake()
    {
        
    }

    public void buyAmmo()
    {
        if(PlayerManager.getMoney() > 150)
        {
            PlayerManager.addAmmo();
        }
    }

    public void nextLevel()
    {
        SceneManager.LoadScene(PlayerManager.getLevel());
    }

    void Update()
    {
        moneyT.text = "Money: $" + PlayerManager.getMoney();
    }
}
