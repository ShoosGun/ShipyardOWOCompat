using OuterWildsOnline.SyncObjects;
using Sfs2X.Entities.Data;
using SlateShipyard.NetworkingInterface;
using System.Collections.Generic;
using SlateShipyard.ShipSpawner;
using UnityEngine;
using static ShipyardOWOAddon.OWONetworkingInterface;

namespace ShipyardOWOAddon
{
    //ObjectToSendSync from ObjectNetworkingInterface
    public class OTSSFromONI : ObjectToSendSync
    {
        protected Dictionary<string, SyncObjectNetworkingInterface> objectNetworkingInterfaces = new();
        protected override void Awake()
        {
            var objectNetworkingInterfaces = GetComponentsInChildren<ObjectNetworkingInterface>();
            var shipData = GetComponent<ShipDataAttached>();

            objectName = shipData.shipData.name;

            for(int i =0; i< objectNetworkingInterfaces.Length; i++) 
            {
                var networkingInterface = objectNetworkingInterfaces[i];

                this.objectNetworkingInterfaces[networkingInterface.UniqueScriptID] = new()
                {
                    objectNetworkingInterface = networkingInterface,
                    syncableMembers = networkingInterface.GetValues(),
                };
            }

            base.Awake();
        }
        //TODO add a way to "check" if the object has fields that need to be synced OnSync or when it changes
        protected override void OnSync(SFSObject syncData)
        {
            foreach(var pair in objectNetworkingInterfaces) 
            {
                var syncableMembers = pair.Value.syncableMembers;
                for (int i = 0; i < syncableMembers.Length; i++)
                {
                    SyncableMember member = syncableMembers[i];
                    SFSObjectIO.PutAsObject(syncData, string.Join("-",pair.Key, member.SyncName), member.GetValue());
                }
            }            

            base.OnSync(syncData);
        }
    }
    public class SyncObjectNetworkingInterface
    {
        public ObjectNetworkingInterface objectNetworkingInterface;
        public SyncableMember[] syncableMembers;
    }

    public class ErrorNetworkingInterface : ObjectNetworkingInterface
    {
        bool isPuppet = false;
        public override bool IsPuppet { get => isPuppet; set => isPuppet = value; }

        public override string UniqueScriptID => "error";
    }
}
