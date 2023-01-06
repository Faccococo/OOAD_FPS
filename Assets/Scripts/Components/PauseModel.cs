using UnityEngine;

namespace QFramework
{
    public interface IPauseModel: IModel
    {
        BindableProperty<bool> IsPause { get; }
    }

    public class PauseModel : AbstractModel, IPauseModel
    {
        public BindableProperty<bool> IsPause { get; } = new BindableProperty<bool>(false);
        protected override void OnInit()
        {

        }
    }
}
