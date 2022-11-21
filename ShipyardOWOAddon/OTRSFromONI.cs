using OuterWildsOnline;
using OuterWildsOnline.SyncObjects;
using Sfs2X.Entities.Data;
using SlateShipyard.NetworkingInterface;
using System.Collections.Generic;

namespace ShipyardOWOAddon
{
    //ObjectToRecieveSync from ObjectNetworkingInterface
    public class OTRSFromONI : ObjectToRecieveSync
    {
        protected Dictionary<string, SyncObjectNetworkingInterface> objectNetworkingInterfaces = new();
        protected override void Start()
        {
            gameObject.AddComponent<SimpleRemoteInterpolation>();

            var objectNetworkingInterfaces = GetComponentsInChildren<ObjectNetworkingInterface>();
            for (int i = 0; i < objectNetworkingInterfaces.Length; i++)
            {
                var networkingInterface = objectNetworkingInterfaces[i];

                networkingInterface.IsPuppet = true;
                this.objectNetworkingInterfaces[networkingInterface.UniqueScriptID] = new()
                {
                    objectNetworkingInterface = networkingInterface,
                    syncableMembers = networkingInterface.GetValues(),
                };
            }

            base.Start();
        }
        //public override void UpdateObjectData(ISFSObject objectData)
        //{
        //    for(int i = 0; i < syncableMembers.Length; i++) 
        //    {
        //        SyncableMember member = syncableMembers[i];
        //        if (objectData.ContainsKey(member.SyncName))
        //        {
        //            var data = objectData.GetData(member.SyncName);
        //            objectNetworkingInterface.SetValue(member.SyncName, data.Data);
        //        }
        //    }

        //    base.UpdateObjectData(objectData);
        //}
        protected override void OnExtensionResponse(SFSObject responseParams)
        {
            foreach (var pair in objectNetworkingInterfaces)
            {
                var syncableMembers = pair.Value.syncableMembers;
                for (int i = 0; i < syncableMembers.Length; i++)
                {
                    SyncableMember member = syncableMembers[i];
                    string dataKey = string.Join("-", pair.Key, member.SyncName);
                    if (responseParams.ContainsKey(dataKey))
                    {
                        var data = responseParams.GetData(dataKey);
                        pair.Value.objectNetworkingInterface.SetValue(member.SyncName, data.Data);
                    }
                }
            }

            base.OnExtensionResponse(responseParams);
        }
    }
}
