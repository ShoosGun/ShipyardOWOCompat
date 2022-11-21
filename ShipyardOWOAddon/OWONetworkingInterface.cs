using SlateShipyard.NetworkingInterface;
using SlateShipyard.ShipSpawner;
using UnityEngine;

namespace ShipyardOWOAddon
{
    public class OWONetworkingInterface : NetworkingInterface
    {
        public override void InvokeMethod(ObjectNetworkingInterface sender, string methodName, params object[] parameters)
        {

        }

        public override void SpawnRemoteShip(ShipData shipData, GameObject shipObject)
        {
            ShipyardOWOAddon.modHelper.Console.WriteLine($"Spawning remote {shipData.name}");
            ObjectNetworkingInterface aNetworkingInterface = shipObject.GetComponentInChildren<ObjectNetworkingInterface>();
            if (aNetworkingInterface != null) //Means there is atleast one ObjectNetworkingInterface on the ship
            {
                shipObject.AddComponent<ShipDataAttached>().shipData = shipData;
                shipObject.AddComponent<OTSSFromONI>();
            }
        }

        public class ShipDataAttached : MonoBehaviour 
        {
            public ShipData shipData;
        }
    }
}
