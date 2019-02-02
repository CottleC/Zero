using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Worker.CInterop;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk.TransformSynchronization;

namespace BlankProject
{
    public class UnityClientConnector : DefaultWorkerConnector
    {
        public const string WorkerType = "UnityClient";
        
        private async void Start()
        {
            await Connect(WorkerType, new ForwardingDispatcher()).ConfigureAwait(false);
        }

        protected override void HandleWorkerConnectionEstablished()
        {
            WorkerUtils.AddClientSystems(Worker.World);
            PlayerLifecycleConfig.CreatePlayerEntityTemplate = PlayerTemplate.CreatePlayerEntityTemplate;

        }

        protected override string SelectDeploymentName(DeploymentList deployments)
        {
            return deployments.Deployments[0].DeploymentName;
        }

    }
}
