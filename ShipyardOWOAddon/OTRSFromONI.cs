using OuterWildsOnline;
using OuterWildsOnline.SyncObjects;
using Sfs2X.Entities.Data;
using SlateShipyard.NetworkingInterface;

namespace ShipyardOWOAddon
{
    //ObjectToRecieveSync from ObjectNetworkingInterface
    public class OTRSFromONI : ObjectToRecieveSync
    {
        ObjectNetworkingInterface objectNetworkingInterface;
        SyncableMember[] syncableMembers;
        protected override void Start()
        {
            objectNetworkingInterface = gameObject.GetComponent<ObjectNetworkingInterface>();
            gameObject.AddComponent<SimpleRemoteInterpolation>();
            objectNetworkingInterface.IsPuppet = true;
            syncableMembers = objectNetworkingInterface.GetValues();

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
            for (int i = 0; i < syncableMembers.Length; i++)
            {
                SyncableMember member = syncableMembers[i];
                if (responseParams.ContainsKey(member.SyncName))
                {
                    var data = responseParams.GetData(member.SyncName);
                    objectNetworkingInterface.SetValue(member.SyncName, data.Data);
                }
            }

            base.OnExtensionResponse(responseParams);
        }
    }
}
