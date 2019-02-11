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
        private struct PlayerRequestData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentDataArray<Player.PlayerAuth.Component> PlayerData;
            [ReadOnly] public ComponentDataArray<Player.PlayerAuth.CommandRequests.ReqPreauthValidate> Requests;
            public ComponentDataArray<Player.PlayerAuth.CommandResponders.ReqPreauthValidate> CommandResponders;
        }

        [Inject] private PlayerRequestData playerRequestData;

        protected override void OnUpdate()
        {
            // Handle Launch Commands from players. Only allow if they have energy etc.
            for (var i = 0; i < playerRequestData.Length; i++)
            {
                var requests = playerRequestData.Requests[i].Requests;
                var responders = playerRequestData.CommandResponders[i];
                var pd = playerRequestData.PlayerData[i];
                Debug.Log("received preauthReq" + requests);
                foreach(var preAuthRequest in requests) {//handle if there are multi?
                    var payload = preAuthRequest.Payload;
                    string theToken = payload.PreAuthToken;
                    var validationResponse = Player.PlayerAuth.ReqPreauthValidate.CreateResponse(preAuthRequest, new Player.ValidatePreauthResponse());
                    responders.ResponsesToSend.Add(validationResponse);//queue this response
                    pd.PlayerName = "Gypaetus";
                    playerRequestData.PlayerData[i] = pd;//do the server-side assigning
                    Debug.Log("Set pd "+ pd +" "+ playerRequestData.PlayerData[i].PlayerName);
                }
                Debug.Log("Responding to preauth request");
                playerRequestData.CommandResponders[i] = responders;//assign this response to the processor!
            }
        }
    }
}
