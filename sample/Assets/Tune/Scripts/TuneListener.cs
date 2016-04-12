using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TuneSDK
{
    public class TuneListener : MonoBehaviour
    {
        public void trackerDidSucceed (string data)
        {
            #if UNITY_IOS
            print ("TuneListener trackerDidSucceed: " + DecodeFrom64 (data));
            #endif
            #if (UNITY_ANDROID || UNITY_WP8 || UNITY_METRO)
            print ("TuneListener trackerDidSucceed: " + data);
            #endif
        }

        public void trackerDidFail (string error)
        {
            print ("TuneListener trackerDidFail: " + error);
        }
        
        public void trackerDidEnqueueRequest (string refId)
        {
            print ("TuneListener trackerDidEnqueueRequest: " + refId);
        }

        public void trackerDidReceiveDeeplink (string url)
        {
            print ("TuneListener trackerDidReceiveDeeplink: " + url);

            // TODO: add your custom code to handle the deferred deeplink url
        }

        public void trackerDidFailDeeplink (string error)
        {
            print ("TuneListener trackerDidFailDeeplink: " + error);
        }

        public void onPowerHooksChanged (string empty)
        {
            print ("TuneListener onPowerHooksChanged");
        }

        public void onFirstPlaylistDownloaded (string empty)
        {
            print ("TuneListener onFirstPlaylistDownloaded");
        }

        /// <summary>
        /// The method to decode base64 strings.
        /// </summary>
        /// <param name="encodedData">A base64 encoded string.</param>
        /// <returns>A decoded string.</returns>
        public static string DecodeFrom64 (string encodedString)
        {
            string decodedString = null;

            #if !(UNITY_WP8) && !(UNITY_METRO)
            print ("TuneListener.DecodeFrom64(string)");

            //this line causes the following error when building for Windows 8 phones:
            //Error building Player: Exception: Error: method `System.String System.Text.Encoding::GetString(System.Byte[])` doesn't exist in target framework. It is referenced from Assembly-CSharp.dll at System.String MATDelegateScript::DecodeFrom64(System.String).
            //Because of this, I'm currently choosing to disable it when Windows 8 phones are used. I'll see if I can find 
            //something better later. Until then, I'll probably use an else branch to take care of the UNITY_WP8 case.
            decodedString = System.Text.Encoding.UTF8.GetString (System.Convert.FromBase64String (encodedString));
            #endif

            return decodedString;
        }
    }
}
