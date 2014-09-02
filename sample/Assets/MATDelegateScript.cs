using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

/* Attach this script to an empty object to retrieve information about the server request.
 * iOS and Android builds are designed to send messages to this script. For Windows Phone 8, 
 * create the class shown below and pass an instance of it into MATBinding.SetMATResponse
 * 
public class SampleMATResponse : MATWP8.MATResponse
{
    //Make sure to attach MATDelegateScript to the empty "MobileAppTracker" object
    MATDelegateScript message_receiver = GameObject.Find("MobileAppTracker").GetComponent<MATDelegateScript>();

    public void DidSucceedWithData(string response)
    {
        if(message_receiver != null)
            message_receiver.trackerDidSucceed("" + response);
    }
    
    public void DidFailWithError(string error)
    {
        if(message_receiver != null)
            message_receiver.trackerDidFail("" + error);
    }
    
    public void EnqueuedActionWithRefId(string refId)
    {
        if(message_receiver != null)
            message_receiver.trackerDidEnqueueRequest("" + refId);
    }
}

 */
public class MATDelegateScript : MonoBehaviour
{
    public void trackerDidSucceed (string data)
    {
        #if UNITY_IPHONE
        print ("MATDelegateScript trackerDidSucceed: " + DecodeFrom64 (data));
        #endif
        #if (UNITY_ANDROID || UNITY_WP8)
        print ("MATDelegateScript trackerDidSucceed: " + data);
        #endif
    }

    public void trackerDidFail (string error)
    {
        print ("MATDelegateScript trackerDidFail: " + error);
    }
    
    public void trackerDidEnqueueRequest (string refId)
    {
        print ("MATDelegateScript trackerDidEnqueueRequest: " + refId);
    }


    /// <summary>
    /// The method to decode base64 strings.
    /// </summary>
    /// <param name="encodedData">A base64 encoded string.</param>
    /// <returns>A decoded string.</returns>
    public static string DecodeFrom64 (string encodedString)
    {
        string decodedString = null;

        #if !(UNITY_WP8)
        print ("MATDelegateScript.DecodeFrom64(string)");

        //this line causes the following error when building for Windows 8 phones:
        //Error building Player: Exception: Error: method `System.String System.Text.Encoding::GetString(System.Byte[])` doesn't exist in target framework. It is referenced from Assembly-CSharp.dll at System.String MATDelegateScript::DecodeFrom64(System.String).
        //Because of this, I'm currently choosing to disable it when Windows 8 phones are used. I'll see if I can find 
        //something better later. Until then, I'll probably use an else branch to take care of the UNITY_WP8 case.
        decodedString = System.Text.Encoding.UTF8.GetString (System.Convert.FromBase64String (encodedString));
        #endif

        return decodedString;
    }

}
