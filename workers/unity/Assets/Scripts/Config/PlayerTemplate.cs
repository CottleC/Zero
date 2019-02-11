using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;

namespace BlankProject
{
    public static class PlayerTemplate
    {
        public static EntityTemplate CreatePlayerEntityTemplate(string workerId, Improbable.Vector3f position)
        {
            var clientAttribute = $"workerId:{workerId}";

            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), clientAttribute);
            template.AddComponent(new Metadata.Snapshot { EntityType = "User" }, UnityGameLogicConnector.WorkerType);
            
            template.AddComponent(new Player.PlayerInput.Snapshot(), clientAttribute);
            /*
            template.AddComponent(new Launcher.Snapshot { EnergyLeft = 100, RechargeTimeLeft = 0 },
                UnityGameLogicConnector.WorkerType);
            template.AddComponent(new Score.Snapshot(), WorkerUtils.UnityGameLogic);
            template.AddComponent(new CubeSpawner.Snapshot { SpawnedCubes = new List<EntityId>() },
                UnityGameLogicConnector.WorkerType);
            */
            template.AddComponent(new Player.PlayerAuth.Snapshot { IsAuthed = false, PlayerName="unauthorized player" },
                "GRPCManager");
            TransformSynchronizationHelper.AddTransformSynchronizationComponents(template, clientAttribute);
            PlayerLifecycleHelper.AddPlayerLifecycleComponents(template, workerId, clientAttribute,
                UnityGameLogicConnector.WorkerType);

            template.SetReadAccess(UnityClientConnector.WorkerType, UnityGameLogicConnector.WorkerType, "GRPCManager");
            template.SetComponentWriteAccess(EntityAcl.ComponentId, UnityGameLogicConnector.WorkerType);

            return template; 
        }
    }
}
