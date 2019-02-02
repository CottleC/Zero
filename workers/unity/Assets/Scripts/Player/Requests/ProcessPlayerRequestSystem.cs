using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
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
    public class ProcessPlayerRequestSystem : ComponentSystem
    {
        private const float RechargeTime = 2.0f;

        private struct PlayerRequestData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentDataArray<Player.PlayerData.Component> PlayerData;
            [ReadOnly] public ComponentDataArray<Player.PlayerData.CommandRequests.ReqPreauthValidate> Requests;
            public ComponentDataArray<Player.PlayerData.CommandResponders.ReqPreauthValidate> CommandResponders;
        }

        [Inject] private PlayerRequestData playerRequestData;

        protected override void OnUpdate()
        {
            // Handle Launch Commands from players. Only allow if they have energy etc.
            for (var i = 0; i < playerRequestData.Length; i++)
            {
                var requests = playerRequestData.Requests[i].Requests;
                var responders = playerRequestData.CommandResponders[i];
                Debug.Log("received preauthReq" + requests);

                foreach(var preAuthRequest in requests) {//handle if there are multi?
                    var validationResponse = Player.PlayerData.ReqPreauthValidate.CreateResponse(preAuthRequest, new Player.ValidatePreauthResponse(true,"Gypeatus"));
                    Debug.Log("Responding to preauth request");
                    responders.ResponsesToSend.Add(validationResponse);//queue this response
                }

                playerRequestData.CommandResponders[i] = responders;//assign this response to the processor!
            }
        }
    }
}
