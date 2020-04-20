using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject MainScreen;
    public GameObject OptionScreen;

    private double volume;
    public double getVolume(){
        return volume;
    }
    public void setVolume(double vol){
        volume = vol;
    }

    public void optionButtonPress()
    {
        OptionScreen.SetActive(true);
        MainScreen.SetActive(false);
    }
    public void optionBackButtonPress()
    {
        OptionScreen.SetActive(false);
        MainScreen.SetActive(true);
    }
    public void playButtonPress()
    {
        SceneManager.LoadScene(2);
    }

}
