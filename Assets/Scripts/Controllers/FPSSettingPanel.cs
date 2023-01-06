using QFramework;
using UnityEngine;
using UnityEngine.UI;

public class FPSSettingPanel : OOADFPS.OOADFPSController
{
    private IGameAudioModel gameAudio;
    private void Awake()
    {
        gameAudio = this.GetModel<IGameAudioModel>();
        transform.Find("确定").GetComponent<Button>().onClick.AddListener(OnCloseSelf);
        transform.Find("主页").GetComponent<Button>().onClick.AddListener(OnReturnMain);
        var bgm = transform.Find("音量").GetComponentInChildren<Slider>();
        bgm.value = gameAudio.BgmVolume.Value;
        bgm.onValueChanged.AddListener(OnBGMValueChanged);
        var sound = transform.Find("音效").GetComponentInChildren<Slider>();
        sound.value = gameAudio.SoundVolume.Value;
        sound.onValueChanged.AddListener(OnSoundValueChanged);

    }

    private void OnCloseSelf()
    {
        gameObject.GetComponent<Animator>().SetBool("open", false);
        gameObject.GetComponent<Animator>().SetBool("close", true);
        this.GetSystem<IAudioMgrSystem>().PlaySound("CloseSetting");
        this.GetModel<IPauseModel>().IsPause.Value = false;
    }

    private void OnReturnMain()
    {
        this.SendCommand(new NextLevelCommand("Menu 3D"));
        this.GetSystem<IAudioMgrSystem>().StopBgm(false);
    }

    private void OnBGMValueChanged(float value)
    {
        gameAudio.BgmVolume.Value = value;
    }

    private void OnSoundValueChanged(float value)
    {
        gameAudio.SoundVolume.Value = value;
    }
}
