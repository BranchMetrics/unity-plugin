using UnityEngine;

namespace TuneSDK
{
    public class TuneListener : MonoBehaviour
    {
        /* Deeplink callbacks */

        public void DidReceiveDeeplink (string url)
        {
            print ("TuneListener DidReceiveDeeplink: " + url);
            // TODO: add your custom code to handle the deferred deeplink url
        }

        public void DidFailDeeplink (string error)
        {
            print ("TuneListener DidFailDeeplink: " + error);
        }
    }
}
