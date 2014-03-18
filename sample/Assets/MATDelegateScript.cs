using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class MATDelegateScript : MonoBehaviour
{
    public void trackerDidSucceed (string data)
    {
		print ("MATDelegateScript trackerDidSucceed: " + DecodeFrom64 (data));
    }

    private void trackerDidFail (string error)
    {
		print ("MATDelegateScript trackerDidFail: " + error);
    }
    
    private void trackerDidEnqueueRequest (string refId)
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
		print ("MATDelegateScript.DecodeFrom64(string)");
        return System.Text.Encoding.UTF8.GetString (System.Convert.FromBase64String (encodedString));
    }
}
