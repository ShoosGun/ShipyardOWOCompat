using OuterWildsOnline.SyncObjects;
using Sfs2X.Entities.Data;
using SlateShipyard.NetworkingInterface;
using SlateShipyard.ShipSpawner;
using UnityEngine;

namespace ShipyardOWOAddon
{
    //ObjectToSendSync from ObjectNetworkingInterface
    public class OTSSFromONI : ObjectToSendSync
    {
        ObjectNetworkingInterface objectNetworkingInterface;
        SyncableMember[] syncableMembers;
        protected override void Awake()
        {
            objectNetworkingInterface = GetComponent<ObjectNetworkingInterface>();

            objectName = objectNetworkingInterface.shipData.name;

            syncableMembers = objectNetworkingInterface.GetValues();

            base.Awake();
        }
        //TODO add a way to "check" if the object has fields that need to be synced OnSync or when it changes
        protected override void OnSync(SFSObject syncData)
        {
            for (int i = 0; i < syncableMembers.Length; i++)
            {
                SyncableMember member = syncableMembers[i];
                SFSObjectIO.PutAsObject(syncData, member.SyncName, member.GetValue());
            }

            base.OnSync(syncData);
        }
    }

    public class ShipNetworkingInterface : ObjectNetworkingInterface
    {
        bool isPuppet = false;
        public override bool IsPuppet { get => isPuppet; set => isPuppet = value; }
    }
}
