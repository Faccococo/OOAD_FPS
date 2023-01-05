using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CountDown : MonoBehaviour
{
    private Text txtTimer;
    public float second;

    private void Start()
    {
        // 1.查找组件引用
        txtTimer = this.GetComponent<Text>();
        // 重复调用（被执行的方法名称，第一次执行时间，每次执行间隔时间）
        InvokeRepeating("Timer", 1, 1);
    }

    private void Timer()
    {
        second = second - 1;
        txtTimer.text = string.Format("{0:d2}:{1:d2}", (int)second / 60, (int)second % 60);

        if (second <= 3)
        {
            txtTimer.color = Color.red;
            if (second <= 0)
            {
                // 取消调用
                CancelInvoke("Timer");
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene(11);
            }
        }
    }
}