using System;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectRepresentation;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace BlankProject
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class PlayerCommandsSystem : ComponentSystem
    {
        private struct PlayerData
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<SpatialEntityId> SpatialEntity;
            [ReadOnly] public ComponentDataArray<Authoritative<Player.PlayerInput.Component>> PlayerInputAuthority;
            public ComponentDataArray<Player.PlayerData.CommandSenders.ReqPreauthValidate> Sender;
        }

        [Inject] private PlayerData playerData;

        protected override void OnUpdate()
        {
            Debug.Log("PlayerData Command Sender");
            if (playerData.Length == 0)
            {
                return;
            }

            if (playerData.Length > 1)
            {
                throw new InvalidOperationException($"Expected at most 1 playerData but got {playerData.Length}");
            }

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("mouse command");

            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("space command");
            }
            else
            { 
                return;
            }

            var sender = playerData.Sender[0];
            var playerId = playerData.SpatialEntity[0].EntityId;

            sender.RequestsToSend.Add(Player.PlayerData.ReqPreauthValidate.CreateRequest(playerId,
                new Player.ValidatePreauthRequest("test", playerId)));

            playerData.Sender[0] = sender;
        }
    }
}