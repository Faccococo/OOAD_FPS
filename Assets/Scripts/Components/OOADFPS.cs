using PlatformShoot;
using QFramework;
using UnityEngine;


public class OOADFPS : Architecture<OOADFPS>
{
    protected override void Init()
    {
        RegisterModel<IGameModel>(new GameModels());
        RegisterModel<IGameAudioModel>(new GameAudioModel());
        RegisterModel<IPauseModel>(new PauseModel());
        RegisterSystem<ITimerSystem>(new TimerSystem());
        RegisterSystem<ICameraSystem>(new CameraSystem());
        RegisterSystem<IAudioMgrSystem>(new AudioMgrSystem());
        RegisterSystem<IObjectPoolSystem>(new ObjectPoolSystem());
        RegisterSystem<IPlayerInputSystem>(new PlayerInputSystem());
    }

    public class OOADFPSController : MonoBehaviour, IController
    {
        public IArchitecture GetArchitecture()
        {
            return OOADFPS.Interface;
        }
    }
}


