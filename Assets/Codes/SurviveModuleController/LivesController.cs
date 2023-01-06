using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LivesController : MonoBehaviour
{
    public int maxLives = 1;
    public Text cur_lives;

    private int live_leaves;
    private void Start()
    {
        live_leaves = maxLives;
    }
    private void Update()
    {
        cur_lives.text = "You have " + live_leaves.ToString() + " lives remain now";
        if (live_leaves <= 0)
        {
            CancelInvoke("Timer");
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(11);
        }
    }
    public void reduceLives()
    {
        live_leaves -= 1;
    }
}
