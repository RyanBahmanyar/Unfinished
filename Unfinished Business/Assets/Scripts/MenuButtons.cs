using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    // This class is used to access methods of the game manager script through
    // unity events.

    public void Quit ()
    {
        GameManager.instance.Quit();
    }

    public void Next ()
    {
        GameManager.instance.NextLevel();
    }

    public void Previous ()
    {
        GameManager.instance.PreviousLevel();
    }

    public void Restart ()
    {
        GameManager.instance.GameOver();
    }

    public void Unfreeze ()
    {
        GameManager.instance.Unfreeze();
    }

    public void Freeze ()
    {
        GameManager.instance.Freeze();
    }
}
