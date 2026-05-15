using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public int MenuSceneIndex;
    public int GameSceneIndex;

    public void ToMenu()
    {

        SceneManager.LoadScene(MenuSceneIndex);

    }

    public void ToGame()
    {

        SceneManager.LoadScene(GameSceneIndex);

    }
}