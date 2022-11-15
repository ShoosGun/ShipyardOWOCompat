using OWML.Common;
using OWML.ModHelper;

using HarmonyLib;

using UnityEngine;

using OuterWildsOnline;

using Sfs2X.Entities.Data;
using SlateShipyard.ShipSpawner;
using SlateShipyard.NetworkingInterface;

namespace ShipyardOWOAddon
{
    [HarmonyPatch]
    public class ShipyardOWOAddon : ModBehaviour
    {
        public static IModHelper modHelper;
        public static GameObject errorPrefab;
        private void Awake()
        {
            SlateShipyard.SlateShipyard.SetNetworkingInterface(new OWONetworkingInterface());

           Harmony harmony = new("ShipyardOWOAddon.locochoco");
           harmony.PatchAll(typeof(ShipyardOWOAddon));
        }
        private void Start()
        {
            AssetBundle bundle = ModHelper.Assets.LoadBundle("AssetBundles/error");
            errorPrefab = bundle.LoadAsset<GameObject>("error.prefab");

            modHelper = ModHelper;
        }

        //private void LoadToOWOPrefabList() 
        //{
        //    for (int i = 0; i < ShipSpawnerManager.ShipAmount(); i++)
        //    {
        //        ShipData data = ShipSpawnerManager.GetShipData(i);
        //         if (data.prefab.TryGetComponent<ObjectNetworkingInterface>(out var networkingInterface))
        //        {
        //            data.prefab.AddComponent<OTRSFromONI>();
        //            var prefabForOWO = Instantiate(data.prefab);
        //            prefabForOWO.SetActive(false);
        //            prefabForOWO.AddComponent<OTRSFromONI>();
        //            DontDestroyOnLoad(prefabForOWO);
        //            RemoteObjects.CloneStorage.Add(data.name, prefabForOWO);
        //        }
        //        else 
        //        {
        //            modHelper.Console.WriteLine($"Ship with name {data.name} doesn't have an ObjectNetworkingInterface");
        //        }
        //    }
        //}

        [HarmonyPostfix]
        [HarmonyPatch(typeof(ConnectionController),"SpawnRemoteObject")]
        static void SpawnRemoteObjectPostfix(int userID, ISFSObject data)
        {
            string objName = data.GetUtfString("name");
            int objId = data.GetInt("id");


            if (RemoteObjects.GetObject(userID, objName, objId, out _))
                return;

            modHelper.Console.WriteLine($"[ShipyardOWOAddon] Checking if object is from shipyard");


            if (!ShipSpawnerManager.TryGetShipData(objName, out ShipData shipData))
            {
                modHelper.Console.WriteLine($"We don't have a ship for the object with name {objName}");
                return;
            }

            GameObject remoteObject = shipData.prefab;

            if (!remoteObject.TryGetComponent<ObjectNetworkingInterface>(out var networkingInterface))
            {
                modHelper.Console.WriteLine($"Ship with name {objName} doesn't have an ObjectNetworkingInterface");

                remoteObject = errorPrefab;
                networkingInterface = remoteObject.AddComponent<ErrorNetworkingInterface>();

            }
            remoteObject = Instantiate(remoteObject);
            networkingInterface.shipData = shipData;
            var comp = remoteObject.AddComponent<OTRSFromONI>();

            comp.Init(objName, userID, objId);

            if (RemoteObjects.AddNewObject(comp))
            {
                modHelper.Console.WriteLine($"New Object is named {comp.ObjectName} ({comp.ObjectId}) from {userID}");
                comp.UpdateObjectData(data);
            }
            else //If we can't add a newly spawned one this means that this is, somehow, a duplicate, so destroy the latest duplicate
            {
                modHelper.Console.WriteLine($"There is no user with this id ({comp.UserId}) or a object with the same name ({comp.ObjectName}) and id ({comp.ObjectId})");
                Destroy(remoteObject);
            }
        }
    }
}