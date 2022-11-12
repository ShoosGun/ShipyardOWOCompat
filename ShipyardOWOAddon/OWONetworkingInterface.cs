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
            if (shipObject.TryGetComponent<ObjectNetworkingInterface>(out var objectNetworkingInterface))
            {
                objectNetworkingInterface.shipData = shipData;
                shipObject.AddComponent<OTSSFromONI>();
            }
        }
    }
}
