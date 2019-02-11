using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Improbable.Worker.CInterop;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace BlankProject
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class ProcessPlayerResponseSystem : ComponentSystem
    {
        private struct PlayerResponseData
        {
            public readonly int Length;
            public ComponentDataArray<Player.PlayerAuth.CommandResponses.ReqPreauthValidate> ReceivedValidateAuthResponses;
        }

        [Inject] private PlayerResponseData playerResponseData;

        protected override void OnUpdate()
        {
            // Handle Launch Commands from players. Only allow if they have energy etc.
            for (var i = 0; i < playerResponseData.Length; i++)
            {
                var responses = playerResponseData.ReceivedValidateAuthResponses[i];
                Debug.Log("client received preauthResp" + responses);

                foreach(var validationResp in responses.Responses) {//handle if there are multi?
                    if(validationResp.StatusCode != StatusCode.Success) {
                        Debug.Log("client received !StatusCode.Success in response: " + validationResp);

                    }
                    else {
                        Debug.Log("client received successful auth response.");
                    }
                }
            }
        }
    }
}
