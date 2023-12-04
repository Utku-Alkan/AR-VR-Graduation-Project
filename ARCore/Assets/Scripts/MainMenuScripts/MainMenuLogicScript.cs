using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogicScript : MonoBehaviour
{
    public void ChangeToGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}
