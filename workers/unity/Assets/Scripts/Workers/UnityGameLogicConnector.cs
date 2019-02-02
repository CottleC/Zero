using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Gdk.GameObjectCreation;

namespace BlankProject
{
    public class UnityGameLogicConnector : DefaultWorkerConnector
    {
        public const string WorkerType = "UnityGameLogic";
        
        private async void Start()
        {
            PlayerLifecycleConfig.CreatePlayerEntityTemplate = PlayerTemplate.CreatePlayerEntityTemplate;
            await Connect(WorkerType, new ForwardingDispatcher()).ConfigureAwait(false);
        }

        protected override void HandleWorkerConnectionEstablished()
        {
            WorkerUtils.AddGameLogicSystems(Worker.World);
        }
    }
}
