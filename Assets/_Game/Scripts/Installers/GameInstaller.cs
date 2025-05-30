using _Game.Signals;
using Zenject;

namespace Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<GameSignals.OnCameraLeft>().OptionalSubscriber();
        }
    }
}