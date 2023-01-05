using UnityEngine;
using QFramework;
using static OOADFPS;

public class User : OOADFPSController
{
    public Animator panelController;
    public bool isOpen, isClose;
    private IObjectPoolSystem objectPool;
    private IAudioMgrSystem audioMgr;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        isClose = false;
        panelController.SetBool("open", false);
        panelController.SetBool("close", false);
        objectPool = this.GetSystem<IObjectPoolSystem>();
        audioMgr = this.GetSystem<IAudioMgrSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        isOpen = panelController.GetBool("open");
        isClose = panelController.GetBool("close");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOpen == true && isClose == false)
            {
                panelController.SetBool("open", false);
                panelController.SetBool("close", true);
                audioMgr.PlaySound("CloseSetting");
            }
            else if (isOpen == false && isClose == true)
            {
                panelController.SetBool("open", true);
                panelController.SetBool("close", false);
                audioMgr.PlaySound("OpenSetting");
                Time.timeScale = 1;
            }
            else if (isOpen == false && isClose == false)
            {
                panelController.SetBool("open", true);
                panelController.SetBool("close", false);
                audioMgr.PlaySound("OpenSetting");
                Time.timeScale = 1;
            }
        }
        //if (isOpen == true && isClose == false)
        //{
        //    AnimatorStateInfo info = panelController.GetCurrentAnimatorStateInfo(0);
        //    if (info.normalizedTime > 1.0f && info.IsName("Open"))
        //    {
        //        Time.timeScale = 0;
        //    }
        //}
    }
}