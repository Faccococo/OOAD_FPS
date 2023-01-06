using QFramework;
using UnityEngine.SceneManagement;

public class NextLevelCommand : AbstractCommand
{
    private readonly string mSceneName;

    public NextLevelCommand(string name)
    {
        mSceneName = name;
    }
    protected override void OnExecute()
    {
        SceneManager.LoadScene(mSceneName);

    }
}

