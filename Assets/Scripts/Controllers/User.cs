using UnityEngine;
using QFramework;
using static OOADFPS;

public class User : OOADFPSController
{
    public Animator panelController;
    public GameObject target;
    public GameObject icon;
    public bool isOpen, isClose;
    private IObjectPoolSystem objectPool;
    private IAudioMgrSystem audioMgr;
    private Transform mTarget;

    // Start is called before the first frame update
    void Start()
    {
        panelController.transform.SetAsLastSibling();
        panelController.updateMode = AnimatorUpdateMode.UnscaledTime;
        isOpen = false;
        isClose = false;
        panelController.SetBool("open", false);
        panelController.SetBool("close", false);
        objectPool = this.GetSystem<IObjectPoolSystem>();
        audioMgr = this.GetSystem<IAudioMgrSystem>();
        audioMgr.PlayBgm("bgm");
        mTarget = GameObject.FindGameObjectWithTag("Player").transform;
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
                this.GetModel<IPauseModel>().IsPause.Value = false;

            }
            else if (isOpen == false && isClose == true)
            {
                panelController.SetBool("open", true);
                panelController.SetBool("close", false);
                audioMgr.PlaySound("OpenSetting");
                this.GetModel<IPauseModel>().IsPause.Value = true;
            }
            else if (isOpen == false && isClose == false)
            {
                panelController.SetBool("open", true);
                panelController.SetBool("close", false);
                audioMgr.PlaySound("OpenSetting");
                this.GetModel<IPauseModel>().IsPause.Value = true;
            }
        }
    }

    private void LateUpdate()
    {
        icon.transform.localPosition = new Vector3(mTarget.position.x, icon.transform.position.y, mTarget.position.z);
        icon.transform.localRotation = Quaternion.Euler(-90, 0, 90 + target.transform.root.eulerAngles.y);
        //icon.transform.localRotation = new Quaternion(icon.transform.rotation.x, icon.transform.rotation.y, icon.transform.rotation.z + target.transform.rotation.z, icon.transform.localRotation.w);
    }
}
