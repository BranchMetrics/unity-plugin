using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class AppToAppSample : MonoBehaviour {
	
	#if UNITY_ANDROID

	// Pass the name of the plugin's dynamic library.
	// Import any functions we will be using from the MAT lib.
	// (I've listed them all here)
	[DllImport ("mobileapptracker")]
	private static extern void initNativeCode(string advertiserId, string advertiserKey);

	[DllImport ("mobileapptracker")]
	private static extern void setCurrencyCode(string currencyCode);
	[DllImport ("mobileapptracker")]
	private static extern void setDebugMode(bool debugMode);
	[DllImport ("mobileapptracker")]
	private static extern void setPackageName(string packageName);
	[DllImport ("mobileapptracker")]
	private static extern void setSiteId(string siteId);
	[DllImport ("mobileapptracker")]
	private static extern void setTRUSTeId(string trusteId);
	[DllImport ("mobileapptracker")]
	private static extern void setUserId(string userId);

	[DllImport ("mobileapptracker")]
	private static extern void startAppToAppTracking(string targetAppId, string advertiserId, string offerId, string publisherId, bool shouldRedirect);

	[DllImport ("mobileapptracker")]
	private static extern void trackAction(string action, bool isId, double revenue, string currencyCode);

	[DllImport ("mobileapptracker")]
	private static extern int trackInstall();
	[DllImport ("mobileapptracker")]
	private static extern int trackUpdate();
	[DllImport ("mobileapptracker")]	
	private static extern void trackInstallWithReferenceId(string refId);
	[DllImport ("mobileapptracker")]
	private static extern void trackUpdateWithReferenceId(string refId);

	// iOS-only functions that are imported for cross-platform coding convenience
	[DllImport ("mobileapptracker")]
	private static extern void setAllowDuplicates(bool allowDuplicates);
	[DllImport ("mobileapptracker")]	
	private static extern void setDelegate(bool enable);	
	[DllImport ("mobileapptracker")]
	private static extern void setDeviceId(string device_id);
	[DllImport ("mobileapptracker")]	
	private static extern void setOpenUDID(string open_udid);
	[DllImport ("mobileapptracker")]	
	private static extern void setRedirectUrl(string redirectUrl);	
	[DllImport ("mobileapptracker")]	
    private static extern void setShouldAutoGenerateVendorIdentifier(bool shouldAutoGenerate);
	[DllImport ("mobileapptracker")]	
	private static extern void setUseCookieTracking(bool useCookieTracking);
	[DllImport ("mobileapptracker")]	
	private static extern void setAdvertiserIdentifier(string advertiserIdentifier);
	[DllImport ("mobileapptracker")]	
	private static extern void setVendorIdentifier(string vendorIdentifier);
	[DllImport ("mobileapptracker")]	
	private static extern string getSDKDataParameters();

	#endif
	
	#if UNITY_IPHONE
	
	// Import any functions we will be using from the MAT lib.
	[DllImport ("__Internal")]
	private static extern void initNativeCode(string advertiserId, string advertiserKey);
	[DllImport ("__Internal")]
	private static extern void setAllowDuplicates(bool allowDuplicates);
	[DllImport ("__Internal")]
	private static extern void setCurrencyCode(string currency_code);
	[DllImport ("__Internal")]
	private static extern void setDebugMode(bool debugMode);
	[DllImport ("__Internal")]
	private static extern void setDeviceId(string device_id);
	[DllImport ("__Internal")]
	private static extern void setOpenUDID(string open_udid);
	[DllImport ("__Internal")]
	private static extern void setPackageName(string package_name);
	[DllImport ("__Internal")]
	private static extern void setRedirectUrl(string redirectUrl);
	[DllImport ("__Internal")]
	private static extern void setSiteId(string site_id);
	[DllImport ("__Internal")]
	private static extern void setTRUSTeId(string truste_tpid);
	[DllImport ("__Internal")]
	private static extern void setUserId(string user_id);
	[DllImport ("__Internal")]
	private static extern void setUseCookieTracking(bool useCookieTracking);
	[DllImport ("__Internal")]
	private static extern void setAdvertiserIdentifier(string advertiserIdentifier);
	[DllImport ("__Internal")]
	private static extern void setVendorIdentifier(string vendorIdentifier);
	[DllImport ("__Internal")]
	private static extern void setDelegate(bool enable);
	
	[DllImport ("__Internal")]
	private static extern void startAppToAppTracking(string targetAppId, string advertiserId, string offerId, string publisherId, bool shouldRedirect);
	
	[DllImport ("__Internal")]
	private static extern void trackAction(string action, bool isId, double revenue, string  currencyCode);
		
	[DllImport ("__Internal")]
	private static extern void trackInstall();
	[DllImport ("__Internal")]
	private static extern void trackUpdate();
	
	#endif
	
	void Awake () {
		//initNativeCode("877", "5afe3bc434184096023e3d8b2ae27e1c"); // Android
		initNativeCode("877", "8c14d6bbe466b65211e781d62e301eec"); // iOS
		setDebugMode(true);
		setDelegate(true);

		#if UNITY_IPHONE
			setAppleAdvertisingIdentifier(iPhone.advertisingIdentifier);
		#endif

		return;
	}
	
	void OnGUI()
	{
		if (GUI.Button(new Rect(10, 10, 350, 100), "App-to-App Tracking Click"))
		{
        	print("App-to-App Tracking button clicked");
			
			string targetAppId = "com.hasoffers.MATUnitySample";
			string advertiserId = "877";
			string offerId = "244760";
			//string offerId = "245086"; // Android
			string publisherId = "10852";
			bool shouldRedirect = true;
			
			startAppToAppTracking(targetAppId, advertiserId, offerId, publisherId, shouldRedirect);
		}
	}
	
	/// <summary>
	/// The method to handle MobileAppTracker success callback.
	/// </summary>
	/// <param name='data'>
	/// Data.
	/// </param>
	public void trackerDidSucceed(string data)
	{
		print("trackerDidSucceed: " + DecodeFrom64(data));
	}
	
	/// <summary>
	/// The method to handle MobileAppTracker failure callback.
	/// </summary>
	/// <param name='error'>
	/// Error.
	/// </param>
	private void trackerDidFail(string error)
	{
		print("trackerDidFail: " + error);
	}
	
	/// <summary>
	/// The method to decode base64 strings.
	/// </summary>
	/// <param name="encodedData">A base64 encoded string.</param>
	/// <returns>A decoded string.</returns>
	public static string DecodeFrom64(string encodedString)
	{
		print("MATSampleScript.DecodeFrom64(string)");
	    return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(encodedString));
	}
}
