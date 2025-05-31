using _Game.Signals;
using Zenject;

namespace Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<GameSignals.OnFailRequest>().OptionalSubscriber();
            Container.DeclareSignal<GameSignals.OnSuccessRequest>().OptionalSubscriber();
            Container.DeclareSignal<GameSignals.OnGameOver>().OptionalSubscriber();
            Container.DeclareSignal<GameSignals.OnGameWon>().OptionalSubscriber();
            Container.DeclareSignal<GameSignals.OnNextDay>().OptionalSubscriber();
        }
    }
}