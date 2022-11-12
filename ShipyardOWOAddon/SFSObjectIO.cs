using Sfs2X.Entities.Data;
using System;

namespace ShipyardOWOAddon
{
    public static class SFSObjectIO
    {
        public static void PutAsObject(SFSObject sFSObject, string key, object Object) //TODO for my sanity, make this function better
        {
            if (Object is byte)
            {
                sFSObject.Put(key, new SFSDataWrapper(SFSDataType.BYTE, Object));
            }
            else if (Object is short)
            {
                sFSObject.Put(key, new SFSDataWrapper(SFSDataType.SHORT, Object));
            }
            else if (Object is int)
            {
                sFSObject.Put(key, new SFSDataWrapper(SFSDataType.INT, Object));
            }
            else if (Object is long)
            {
                sFSObject.Put(key, new SFSDataWrapper(SFSDataType.LONG, Object));
            }

            else if (Object is float)
            {
                sFSObject.Put(key, new SFSDataWrapper(SFSDataType.FLOAT, Object));
            }
            else if (Object is double)
            {
                sFSObject.Put(key, new SFSDataWrapper(SFSDataType.DOUBLE, Object));
            }


            else if (Object is string)
            {
                sFSObject.Put(key, new SFSDataWrapper(SFSDataType.UTF_STRING, Object));
            }

            else if (Object is byte[])
            {
                sFSObject.Put(key, new SFSDataWrapper(SFSDataType.BYTE_ARRAY, Object));
            }
            else if (Object is int[])
            {
                sFSObject.Put(key, new SFSDataWrapper(SFSDataType.INT_ARRAY, Object));
            }
            else
                throw new ArgumentException(string.Format("The type {0} isn't currently supported by this function", Object.GetType()));
        }
    }
}
