using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class MATSampleScript : MonoBehaviour {
	
	#if UNITY_ANDROID || UNITY_IPHONE
	string MAT_ADVERTISER_ID = null;
	string MAT_CONVERSION_KEY = null;
	//string MAT_PACKAGE_NAME = "com.hasoffers.unitytestapp";
	string MAT_SITE_ID = null;
	#endif
	
	void Awake ()
	{
		#if UNITY_ANDROID || UNITY_IPHONE
		MAT_ADVERTISER_ID = "877";
		MAT_CONVERSION_KEY = "8c14d6bbe466b65211e781d62e301eec";
		#if UNITY_ANDROID
		//MAT_SITE_ID = "52048";
		#elif UNITY_IPHONE
		MAT_SITE_ID = "52096";
		#endif
		print ("Awake called: " + MAT_ADVERTISER_ID + ", " + MAT_CONVERSION_KEY + ", " + MAT_SITE_ID);
		#endif
		
		return;
	}
	
	void OnGUI ()
	{
		GUIStyle headingLabelStyle = new GUIStyle();
		headingLabelStyle.fontStyle = FontStyle.Bold;
		headingLabelStyle.fontSize = 50;
		headingLabelStyle.alignment = TextAnchor.MiddleCenter;
		headingLabelStyle.normal.textColor = Color.white;
		
		GUI.Label(new Rect(10, 5, Screen.width - 20, 90), "MAT Unity Test App", headingLabelStyle);
		
		GUI.skin.button.fontSize = 40;
		
		if (GUI.Button (new Rect (10, 110, Screen.width - 20, 90), "Start MAT"))
		{
			print ("Start MAT clicked");
			#if UNITY_ANDROID || UNITY_IPHONE
			MATBinding.Init(MAT_ADVERTISER_ID, MAT_CONVERSION_KEY);

			#if UNITY_IPHONE
			MATBinding.SetAppleAdvertisingIdentifier(iPhone.advertisingIdentifier, iPhone.advertisingTrackingEnabled);
			#endif
			#endif
		}
		
		else if (GUI.Button (new Rect (10, 210, Screen.width - 20, 90), "Set Delegate"))
		{
			print ("Set Delegate clicked");
			#if UNITY_ANDROID || UNITY_IPHONE
			MATBinding.SetDelegate(true);
			#endif
		}
		
		else if (GUI.Button (new Rect (10, 310, Screen.width - 20, 90), "Enable Debug Mode"))
		{
			print ("Enable Debug Mode clicked");
			#if UNITY_ANDROID || UNITY_IPHONE
			// NOTE: !!! ONLY FOR DEBUG !!!
			// !!! Make sure you set to false 
			//     OR 
			//     remove the setDebugMode and setAllowDuplicates calls for Production builds !!!
			MATBinding.SetDebugMode(true);
			#endif
		}
		
		else if (GUI.Button (new Rect (10, 410, Screen.width - 20, 90), "Allow Duplicates"))
		{
			print ("Allow Duplicates clicked");
			#if UNITY_ANDROID || UNITY_IPHONE
			// NOTE: !!! ONLY FOR DEBUG !!!
			// !!! Make sure you set to false 
			//     OR 
			//     remove the setDebugMode and setAllowDuplicates calls for Production builds !!!
			MATBinding.SetAllowDuplicates(true);
			#endif
		}
		
		else if (GUI.Button (new Rect (10, 510, Screen.width - 20, 90), "Measure Session"))
		{
			print ("Measure Session clicked");
			#if UNITY_ANDROID || UNITY_IPHONE
			MATBinding.MeasureSession();
			#endif
		}
		
		else if (GUI.Button (new Rect (10, 610, Screen.width - 20, 90), "Measure Action"))
		{
			print ("Measure Action clicked");
			#if UNITY_ANDROID || UNITY_IPHONE
			MATBinding.MeasureAction("evt11");
			MATBinding.MeasureActionWithRefId("evt12", "ref111");
			MATBinding.MeasureActionWithRevenue("evt13", 0.35, "CAD", "ref111");
			#endif
		}
		
		else if (GUI.Button (new Rect (10, 710, Screen.width - 20, 90), "Measure Action With Event Items"))
		{
			print ("Measure Action With Event Items clicked");
			
			#if UNITY_ANDROID || UNITY_IPHONE
			MATItem item1 = new MATItem();
			item1.name = "subitem1";
			item1.unitPrice = 5;
			item1.quantity = 5;
			item1.revenue = 3;
			item1.attribute2 = "attrValue2";
			item1.attribute3 = "attrValue3";
			item1.attribute4 = "attrValue4";
			item1.attribute5 = "attrValue5";
			
			MATItem item2 = new MATItem();
			item2.name = "subitem2";
			item2.unitPrice = 1;
			item2.quantity = 3;
			item2.revenue = 1.5;
			item2.attribute1 = "attrValue1";
			item2.attribute3 = "attrValue3";
			
			MATItem[] arr = { item1, item2 };
			
			// transaction state may be set to the value received from the iOS/Android app store.
			int transactionState = 1;
			
			string receiptData = null;
			string receiptSignature = null;
			
			#if UNITY_IPHONE
			receiptData = getSampleiTunesIAPReceipt();
			#endif
			
			MATBinding.MeasureActionWithEventItems("event7WithReceipt", arr, arr.Length, "ref222", 10, null, transactionState, receiptData, receiptSignature);
			#endif
		}
		
		else if (GUI.Button (new Rect (10, 810, Screen.width - 20, 90), "Test Setter Methods"))
		{
			print ("Test Setter Methods clicked");
			#if UNITY_ANDROID || UNITY_IPHONE
			MATBinding.SetAge(34);
			MATBinding.SetAllowDuplicates(true);
			MATBinding.SetAppAdTracking(true);
			#if UNITY_IPHONE
			MATBinding.SetAppleAdvertisingIdentifier(iPhone.advertisingIdentifier, iPhone.advertisingTrackingEnabled);
			MATBinding.SetAppleVendorIdentifier("87654321-4321-4321-4321-210987654321");
			#endif
			MATBinding.SetCurrencyCode("CAD");
			MATBinding.SetDebugMode(true);
			MATBinding.SetDelegate(true);
			MATBinding.SetEventAttribute1("test_attribute1");
			MATBinding.SetEventAttribute2("test_attribute2");
			MATBinding.SetEventAttribute3("test_attribute3");
			MATBinding.SetEventAttribute4("test_attribute4");
			MATBinding.SetEventAttribute5("test_attribute5");
			MATBinding.SetEventContentType("testContentType");
			MATBinding.SetEventContentId("testContentId");
			MATBinding.SetEventDate1("" + (DateTime.UtcNow - new DateTime (1970, 1, 1)).TotalMilliseconds);
			MATBinding.SetEventDate2("" + ((DateTime.UtcNow - new DateTime (1970, 1, 1)).TotalMilliseconds + 60 * 1000));
			MATBinding.SetEventLevel(3);
			MATBinding.SetEventQuantity(2);
			MATBinding.SetEventRating(4.5f);
			MATBinding.SetEventSearchString("testSearchString");
			MATBinding.SetExistingUser(false);
			MATBinding.SetFacebookUserId("temp_facebook_user_id");
			MATBinding.SetGender(0);
			#if UNITY_ANDROID
			MATBinding.SetGoogleAdvertisingId("12345678-1234-1234-1234-123456789012", true);
			#endif
			MATBinding.SetGoogleUserId("temp_google_user_id");
			MATBinding.SetJailbroken(false);
			MATBinding.SetLocation(111,222,333);
			//MATBinding.SetPackageName(MAT_PACKAGE_NAME); 
			MATBinding.SetPayingUser(false);
			MATBinding.SetRedirectUrl("http://www.example.com");
			MATBinding.SetShouldAutoDetectJailbroken(true);
			MATBinding.SetShouldAutoGenerateVendorIdentifier(true);
			MATBinding.SetSiteId(MAT_SITE_ID);
			MATBinding.SetTRUSTeId("1234567890");
			MATBinding.SetTwitterUserId("temp_twitter_user_id");
			MATBinding.SetUseCookieTracking(false);
			MATBinding.SetUserEmail("tempuser@tempcompany.com");
			MATBinding.SetUserId("temp_user_id");
			MATBinding.SetUserName("temp_user_name");
			#endif
		}
		
		else if (GUI.Button (new Rect (10, 910, Screen.width - 20, 90), "Test Getter Methods"))
		{
			print ("Test Getter Methods clicked");
			#if UNITY_ANDROID || UNITY_IPHONE
			print ("isPayingUser = " + MATBinding.GetIsPayingUser());
			print ("matId     = " + MATBinding.GetMATId());
			print ("openLogId = " + MATBinding.GetOpenLogId());
			#endif
		}
	}
	
	public static string getSampleiTunesIAPReceipt ()
	{
		return "{\"signature\" = \"AiuR/oePW4lQq2qAFerYcL/lU7HB7rqPKoddrjnqLUqvLywbSukO3OUwWwiRGE8iFiNvaqVF2CaG8siBkfkP5KYaeiTHT2RNLCIKyCfhOIr4q0wYCKwxNp2vdo3S8b+4boeSAXzgzBHCR1S1hTN5LuoeRzDeIWHoYT66CBweqDetAAADVzCCA1MwggI7oAMCAQICCGUUkU3ZWAS1MA0GCSqGSIb3DQEBBQUAMH8xCzAJBgNVBAYTAlVTMRMwEQYDVQQKDApBcHBsZSBJbmMuMSYwJAYDVQQLDB1BcHBsZSBDZXJ0aWZpY2F0aW9uIEF1dGhvcml0eTEzMDEGA1UEAwwqQXBwbGUgaVR1bmVzIFN0b3JlIENlcnRpZmljYXRpb24gQXV0aG9yaXR5MB4XDTA5MDYxNTIyMDU1NloXDTE0MDYxNDIyMDU1NlowZDEjMCEGA1UEAwwaUHVyY2hhc2VSZWNlaXB0Q2VydGlmaWNhdGUxGzAZBgNVBAsMEkFwcGxlIGlUdW5lcyBTdG9yZTETMBEGA1UECgwKQXBwbGUgSW5jLjELMAkGA1UEBhMCVVMwgZ8wDQYJKoZIhvcNAQEBBQADgY0AMIGJAoGBAMrRjF2ct4IrSdiTChaI0g8pwv/cmHs8p/RwV/rt/91XKVhNl4XIBimKjQQNfgHsDs6yju++DrKJE7uKsphMddKYfFE5rGXsAdBEjBwRIxexTevx3HLEFGAt1moKx509dhxtiIdDgJv2YaVs49B0uJvNdy6SMqNNLHsDLzDS9oZHAgMBAAGjcjBwMAwGA1UdEwEB/wQCMAAwHwYDVR0jBBgwFoAUNh3o4p2C0gEYtTJrDtdDC5FYQzowDgYDVR0PAQH/BAQDAgeAMB0GA1UdDgQWBBSpg4PyGUjFPhJXCBTMzaN+mV8k9TAQBgoqhkiG92NkBgUBBAIFADANBgkqhkiG9w0BAQUFAAOCAQEAEaSbPjtmN4C/IB3QEpK32RxacCDXdVXAeVReS5FaZxc+t88pQP93BiAxvdW/3eTSMGY5FbeAYL3etqP5gm8wrFojX0ikyVRStQ+/AQ0KEjtqB07kLs9QUe8czR8UGfdM1EumV/UgvDd4NwNYxLQMg4WTQfgkQQVy8GXZwVHgbE/UC6Y7053pGXBk51NPM3woxhd3gSRLvXj+loHsStcTEqe9pBDpmG5+sk4tw+GK3GMeEN5/+e1QT9np/Kl1nj+aBw7C0xsy0bFnaAd1cSS6xdory/CUvM6gtKsmnOOdqTesbp0bs8sn6Wqs0C9dgcxRHuOMZ2tm8npLUm7argOSzQ==\";\"purchase-info\" = \"ewoJIm9yaWdpbmFsLXB1cmNoYXNlLWRhdGUtcHN0IiA9ICIyMDEzLTA2LTE5IDEzOjMyOjE5IEFtZXJpY2EvTG9zX0FuZ2VsZXMiOwoJInVuaXF1ZS1pZGVudGlmaWVyIiA9ICJjODU0OGI1YWExZjM5NDA2NmY1ZWEwM2Q3ZGU0YTBiYzdjMTEyZDk5IjsKCSJvcmlnaW5hbC10cmFuc2FjdGlvbi1pZCIgPSAiMTAwMDAwMDA3Nzk2MDgzNSI7CgkiYnZycyIgPSAiMS4xIjsKCSJ0cmFuc2FjdGlvbi1pZCIgPSAiMTAwMDAwMDA4MzE1MjA1NCI7CgkicXVhbnRpdHkiID0gIjEiOwoJIm9yaWdpbmFsLXB1cmNoYXNlLWRhdGUtbXMiID0gIjEzNzE2NzM5MzkwMDAiOwoJInVuaXF1ZS12ZW5kb3ItaWRlbnRpZmllciIgPSAiQTM3MjFCQzctNDA3Qi00QzcyLTg4RDAtMTdGNjIwRUMzNzEzIjsKCSJwcm9kdWN0LWlkIiA9ICJjb20uaGFzb2ZmZXJzLmluYXBwcHVyY2hhc2V0cmFja2VyMS5iYWxsIjsKCSJpdGVtLWlkIiA9ICI2NTMyMzA4MjkiOwoJImJpZCIgPSAiY29tLkhhc09mZmVycy5JbkFwcFB1cmNoYXNlVHJhY2tlcjEiOwoJInB1cmNoYXNlLWRhdGUtbXMiID0gIjEzNzU4MTM2NDcxMDIiOwoJInB1cmNoYXNlLWRhdGUiID0gIjIwMTMtMDgtMDYgMTg6Mjc6MjcgRXRjL0dNVCI7CgkicHVyY2hhc2UtZGF0ZS1wc3QiID0gIjIwMTMtMDgtMDYgMTE6Mjc6MjcgQW1lcmljYS9Mb3NfQW5nZWxlcyI7Cgkib3JpZ2luYWwtcHVyY2hhc2UtZGF0ZSIgPSAiMjAxMy0wNi0xOSAyMDozMjoxOSBFdGMvR01UIjsKfQ==\";\"environment\" = \"Sandbox\";\"pod\" = \"100\";\"signing-status\" = \"0\";}";
	}
}
