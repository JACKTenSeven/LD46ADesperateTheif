using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Text moneyT;
  
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
