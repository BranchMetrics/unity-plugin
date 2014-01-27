#include <cstddef>
#include <stdlib.h>
#include <jni.h>
#include <android/log.h>

extern "C"
{
	typedef struct
	{
		char * item;
		double unitPrice;
		int    quantity;
		double revenue;
		char * attribute1;
		char * attribute2;
		char * attribute3;
		char * attribute4;
		char * attribute5;
	} MATItem;

	JavaVM*		java_vm;
	JNIEnv*		jni_env;

	jobject		MobileAppTracker;
	jmethodID	trackActionMethod;
	jmethodID	trackActionWithEventItemMethod;
	jmethodID	trackActionWithEventItemAndReceiptMethod;
	jmethodID	trackInstallMethod;
	jmethodID	trackUpdateMethod;
	jmethodID	setAgeMethod;
	jmethodID	setAllowDuplicatesMethod;
	jmethodID	setAltitudeMethod;
	jmethodID	setCurrencyCodeMethod;
	jmethodID	setDebugModeMethod;
	jmethodID	setGenderMethod;
	jmethodID	setLatitudeMethod;
	jmethodID	setLimitAdTrackingEnabledMethod;
	jmethodID	setLongitudeMethod;
	jmethodID	setPackageNameMethod;
	jmethodID	setRefIdMethod;
	jmethodID	setRevenueMethod;
	jmethodID	setSiteIdMethod;
	jmethodID	setTRUSTeIdMethod;
	jmethodID	setUserIdMethod;
	jmethodID	setFacebookUserIdMethod;
	jmethodID	setTwitterUserIdMethod;
	jmethodID	setGoogleUserIdMethod;
	jmethodID	startAppToAppTrackingMethod;

	jint JNI_OnLoad(JavaVM* vm, void* reserved)
	{
		// Use __android_log_print for logcat debugging
		__android_log_print(ANDROID_LOG_INFO, "MATJavaBridge", "[%s] Creating java link vm = %08x\n", __FUNCTION__, vm);
		java_vm = vm;
		jni_env = 0;
		// Attach our thread to the Java VM to get the JNI env
		java_vm->AttachCurrentThread(&jni_env, 0);
		__android_log_print(ANDROID_LOG_INFO, "MATJavaBridge", "[%s] JNI Environment is = %08x\n", __FUNCTION__, jni_env);
	
		return JNI_VERSION_1_6;		// minimum JNI version
	}

	const void initNativeCode (char* advertiserId, char* conversionKey)
	{
		// Convert char arrays to Java Strings for passing in to MobileAppTracker constructor
		jstring advertiserIdUTF		= jni_env->NewStringUTF(advertiserId);
		jstring conversionKeyUTF	= jni_env->NewStringUTF(conversionKey); 

		// Find our main activity..
		jclass cls_Activity		= jni_env->FindClass("com/unity3d/player/UnityPlayer");
		jfieldID fid_Activity	= jni_env->GetStaticFieldID(cls_Activity, "currentActivity", "Landroid/app/Activity;");
		jobject obj_Activity	= jni_env->GetStaticObjectField(cls_Activity, fid_Activity);
		__android_log_print(ANDROID_LOG_INFO, "MATJavaBridge", "[%s] Current activity = %08x\n", __FUNCTION__, obj_Activity);

		// Create a MobileAppTracker object
		jclass cls_MobileAppTracker		= jni_env->FindClass("com/mobileapptracker/MobileAppTracker");
		jmethodID mid_MobileAppTracker	= jni_env->GetMethodID(cls_MobileAppTracker, "<init>", "(Landroid/content/Context;Ljava/lang/String;Ljava/lang/String;)V");
		jobject obj_MobileAppTracker	= jni_env->NewObject(cls_MobileAppTracker, mid_MobileAppTracker, obj_Activity, advertiserIdUTF, conversionKeyUTF);
		__android_log_print(ANDROID_LOG_INFO, "MATJavaBridge", "[%s] MobileAppTracker object = %08x\n", __FUNCTION__, obj_MobileAppTracker);

		// Set SDK plugin name
		jmethodID setPluginMethod = jni_env->GetMethodID(cls_MobileAppTracker, "setPluginName", "(Ljava/lang/String;)V");
		jstring pluginUTF = jni_env->NewStringUTF("unity");
		jni_env->CallVoidMethod(obj_MobileAppTracker, setPluginMethod, pluginUTF);

		// Create a global reference to the MobileAppTracker object and fetch method ids
		MobileAppTracker		= jni_env->NewGlobalRef(obj_MobileAppTracker);
		trackInstallMethod		= jni_env->GetMethodID(cls_MobileAppTracker, "trackInstall", "()I");
		trackActionMethod		= jni_env->GetMethodID(cls_MobileAppTracker, "trackAction", "(Ljava/lang/String;DLjava/lang/String;)I");
		trackActionWithEventItemMethod = jni_env->GetMethodID(cls_MobileAppTracker, "trackAction", "(Ljava/lang/String;Ljava/util/List;)I");
		trackActionWithEventItemAndReceiptMethod = jni_env->GetMethodID(cls_MobileAppTracker, "trackAction", "(Ljava/lang/String;Ljava/util/List;Ljava/lang/String;Ljava/lang/String;)I");
		trackUpdateMethod		= jni_env->GetMethodID(cls_MobileAppTracker, "trackUpdate", "()I");

		setAgeMethod			= jni_env->GetMethodID(cls_MobileAppTracker, "setAge", "(I)V");
		setAllowDuplicatesMethod 	= jni_env->GetMethodID(cls_MobileAppTracker, "setAllowDuplicates", "(Z)V");
		setAltitudeMethod		= jni_env->GetMethodID(cls_MobileAppTracker, "setAltitude", "(D)V");
		setCurrencyCodeMethod	= jni_env->GetMethodID(cls_MobileAppTracker, "setCurrencyCode", "(Ljava/lang/String;)V");
		setDebugModeMethod		= jni_env->GetMethodID(cls_MobileAppTracker, "setDebugMode", "(Z)V");
		setGenderMethod			= jni_env->GetMethodID(cls_MobileAppTracker, "setGender", "(I)V");
		setLatitudeMethod		= jni_env->GetMethodID(cls_MobileAppTracker, "setLatitude", "(D)V");
		setLimitAdTrackingEnabledMethod	= jni_env->GetMethodID(cls_MobileAppTracker, "setLimitAdTrackingEnabled", "(Z)V");
		setLongitudeMethod		= jni_env->GetMethodID(cls_MobileAppTracker, "setLongitude", "(D)V");
		setPackageNameMethod	= jni_env->GetMethodID(cls_MobileAppTracker, "setPackageName", "(Ljava/lang/String;)V");
		setRefIdMethod			= jni_env->GetMethodID(cls_MobileAppTracker, "setRefId", "(Ljava/lang/String;)V");
		setRevenueMethod		= jni_env->GetMethodID(cls_MobileAppTracker, "setRevenue", "(D)V");
		setSiteIdMethod			= jni_env->GetMethodID(cls_MobileAppTracker, "setSiteId", "(Ljava/lang/String;)V");
		setTRUSTeIdMethod		= jni_env->GetMethodID(cls_MobileAppTracker, "setTRUSTeId", "(Ljava/lang/String;)V");
		setUserIdMethod			= jni_env->GetMethodID(cls_MobileAppTracker, "setUserId", "(Ljava/lang/String;)V");
		setFacebookUserIdMethod	= jni_env->GetMethodID(cls_MobileAppTracker, "setFacebookUserId", "(Ljava/lang/String;)V");
		setTwitterUserIdMethod	= jni_env->GetMethodID(cls_MobileAppTracker, "setTwitterUserId", "(Ljava/lang/String;)V");
		setGoogleUserIdMethod	= jni_env->GetMethodID(cls_MobileAppTracker, "setGoogleUserId", "(Ljava/lang/String;)V");

		startAppToAppTrackingMethod = jni_env->GetMethodID(cls_MobileAppTracker, "setTracking", "(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;Z)Ljava/lang/String;");

		// Explicitly remove the local variables to prevent leaks
		jni_env->DeleteLocalRef(advertiserIdUTF);
		jni_env->DeleteLocalRef(conversionKeyUTF);
		jni_env->DeleteLocalRef(pluginUTF);

		return;
	}

	const void startAppToAppTracking(char* targetAppId, char* advertiserId, char* offerId, char* publisherId, bool shouldRedirect)
	{
		jstring targetAppIdUTF = jni_env->NewStringUTF(targetAppId);
		jstring advertiserIdUTF = jni_env->NewStringUTF(advertiserId);
		jstring offerIdUTF = jni_env->NewStringUTF(offerId);
		jstring publisherIdUTF = jni_env->NewStringUTF(publisherId);

		// Call setTracking from Android SDK
		jni_env->CallObjectMethod(MobileAppTracker, startAppToAppTrackingMethod, advertiserIdUTF, targetAppIdUTF, publisherIdUTF, offerIdUTF, shouldRedirect);

		jni_env->DeleteLocalRef(targetAppIdUTF);
		jni_env->DeleteLocalRef(advertiserIdUTF);
		jni_env->DeleteLocalRef(offerIdUTF);
		jni_env->DeleteLocalRef(publisherIdUTF);

		return;
	}

	const void trackAction(char* eventName, bool isId, double revenue, char* currencyCode)
	{
		jstring eventNameUTF = jni_env->NewStringUTF(eventName);
		jstring currencyCodeUTF = jni_env->NewStringUTF(currencyCode);
		jint trackStatus = jni_env->CallIntMethod(MobileAppTracker, trackActionMethod, eventNameUTF, revenue, currencyCodeUTF);
		__android_log_print(ANDROID_LOG_INFO, "MATJavaBridge", "[%s] trackAction status = %d\n", __FUNCTION__, trackStatus);

		jni_env->DeleteLocalRef(eventNameUTF);
		jni_env->DeleteLocalRef(currencyCodeUTF);

		return;
	}

	const void trackActionWithEventItem(char* eventName, bool isId, MATItem items[], int eventItemCount, char* refId, double revenue, char* currency, int transactionState, char* receiptData, char* receiptSignature)
	{
		// Set the ref ID
		jstring refIdUTF = jni_env->NewStringUTF(refId);
		jni_env->CallVoidMethod(MobileAppTracker, setRefIdMethod, refIdUTF);
		jni_env->DeleteLocalRef(refIdUTF);

		// Set the revenue
		jni_env->CallVoidMethod(MobileAppTracker, setRevenueMethod, revenue);

		// Set the currency code
		jstring currencyCodeUTF = jni_env->NewStringUTF(currency);
		jni_env->CallVoidMethod(MobileAppTracker, setCurrencyCodeMethod, currencyCodeUTF);
		jni_env->DeleteLocalRef(currencyCodeUTF);

		// Get Java ArrayList class and add method
		jclass clsArrayList = jni_env->FindClass("java/util/ArrayList");
		jmethodID listConstructorID = jni_env->GetMethodID(clsArrayList, "<init>", "()V");
		jmethodID list_add_mid = 0;
		list_add_mid = jni_env->GetMethodID(clsArrayList, "add", "(Ljava/lang/Object;)Z");

		// Get MATEventItem class constructor
		const char* mateventitem_class_name = "com/mobileapptracker/MATEventItem";
		jclass clsMATEventItem = jni_env->FindClass(mateventitem_class_name);
		jmethodID constructorID = jni_env->GetMethodID(clsMATEventItem, "<init>", "(Ljava/lang/String;IDDLjava/lang/String;Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;)V");

		// Create a jobject of ArrayList for storing MATEventItems
		jobject jlistobj = jni_env->NewObject(clsArrayList, listConstructorID);

		// Add a MATEventItem for each item to a List
		for (uint i = 0; i < eventItemCount; i++) {
			jstring nameVal = jni_env->NewStringUTF(items[i].item);
			jstring att1Val = jni_env->NewStringUTF(items[i].attribute1);
			jstring att2Val = jni_env->NewStringUTF(items[i].attribute2);
			jstring att3Val = jni_env->NewStringUTF(items[i].attribute3);
			jstring att4Val = jni_env->NewStringUTF(items[i].attribute4);
			jstring att5Val = jni_env->NewStringUTF(items[i].attribute5);

			// Create a MATEventItem
			jobject jeventitemobj = jni_env->NewObject(clsMATEventItem, constructorID,
				nameVal,
				items[i].quantity,
				items[i].unitPrice,
				items[i].revenue,
				att1Val,
				att2Val,
				att3Val,
				att4Val,
				att5Val);

			// Add the MATEventItem to the List
			jboolean jbool = jni_env->CallBooleanMethod(jlistobj, list_add_mid, jeventitemobj);

			jni_env->DeleteLocalRef(jeventitemobj);
			jni_env->DeleteLocalRef(nameVal);
			jni_env->DeleteLocalRef(att1Val);
			jni_env->DeleteLocalRef(att2Val);
			jni_env->DeleteLocalRef(att3Val);
			jni_env->DeleteLocalRef(att4Val);
			jni_env->DeleteLocalRef(att5Val);
		}

		// Convert event name to UTF
		jstring eventNameUTF = jni_env->NewStringUTF(eventName);
		
		jint trackStatus;
		if (receiptData && strlen(receiptData) > 0 && receiptSignature && strlen(receiptSignature) > 0) {
			jstring receiptDataUTF = jni_env->NewStringUTF(receiptData);
			jstring receiptSignatureUTF = jni_env->NewStringUTF(receiptSignature);
			// Call trackActionWithEventItem with receipt data
			trackStatus = jni_env->CallIntMethod(MobileAppTracker, trackActionWithEventItemAndReceiptMethod, eventNameUTF, jlistobj, receiptDataUTF, receiptSignatureUTF);
			jni_env->DeleteLocalRef(receiptDataUTF);
			jni_env->DeleteLocalRef(receiptSignatureUTF);
		} else {
			// Call trackActionWithEventItem
			trackStatus = jni_env->CallIntMethod(MobileAppTracker, trackActionWithEventItemMethod, eventNameUTF, jlistobj);
		}
		__android_log_print(ANDROID_LOG_INFO, "MATJavaBridge", "[%s] trackActionWithEventItem status = %d\n", __FUNCTION__, trackStatus);

		// Delete local variables used
		jni_env->DeleteLocalRef(eventNameUTF);
		jni_env->DeleteLocalRef(jlistobj);

		return;
	}

	const void trackInstall() 
	{
		jint trackStatus = jni_env->CallIntMethod(MobileAppTracker, trackInstallMethod);
		__android_log_print(ANDROID_LOG_INFO, "MATJavaBridge", "[%s] trackInstall status = %d\n", __FUNCTION__, trackStatus);
		return;
	}

	const void trackInstallWithReferenceId(char* refId)
	{
		jstring refIdUTF = jni_env->NewStringUTF(refId);
		jni_env->CallVoidMethod(MobileAppTracker, setRefIdMethod, refIdUTF);
		jni_env->DeleteLocalRef(refIdUTF);
		
		jint trackStatus = jni_env->CallIntMethod(MobileAppTracker, trackInstallMethod);
		__android_log_print(ANDROID_LOG_INFO, "MATJavaBridge", "[%s] trackInstallWithReferenceId status = %d\n", __FUNCTION__, trackStatus);
		
		return;
	}

	const void trackUpdate()
	{
		jint trackStatus = jni_env->CallIntMethod(MobileAppTracker, trackUpdateMethod);
		__android_log_print(ANDROID_LOG_INFO, "MATJavaBridge", "[%s] trackUpdate status = %d\n", __FUNCTION__, trackStatus);
		return;
	}

	const void trackUpdateWithReferenceId(char* refId)
	{
		jstring refIdUTF = jni_env->NewStringUTF(refId);
		jni_env->CallVoidMethod(MobileAppTracker, setRefIdMethod, refIdUTF);
		jni_env->DeleteLocalRef(refIdUTF);
		
		jint trackStatus = jni_env->CallIntMethod(MobileAppTracker, trackUpdateMethod);
		__android_log_print(ANDROID_LOG_INFO, "MATJavaBridge", "[%s] trackUpdateWithReferenceId status = %d\n", __FUNCTION__, trackStatus);
		
		return;	
	}

	const void setAge(int age)
	{
		jni_env->CallVoidMethod(MobileAppTracker, setAgeMethod, age);
		return;
	}

	const void setAllowDuplicateRequests(bool allowDuplicateRequests)
	{
		jni_env->CallVoidMethod(MobileAppTracker, setAllowDuplicatesMethod, allowDuplicateRequests);
		return;
	}

	const void setAppAdTracking(bool appAdTracking) {
		jni_env->CallVoidMethod(MobileAppTracker, setLimitAdTrackingEnabledMethod, !appAdTracking);
		return;
	}
	
	const void setCurrencyCode(char* currencyCode) 
	{
		jstring currencyCodeUTF = jni_env->NewStringUTF(currencyCode);
		jni_env->CallVoidMethod(MobileAppTracker, setCurrencyCodeMethod, currencyCodeUTF);
		jni_env->DeleteLocalRef(currencyCodeUTF);
		return;
	}
	
	const void setDebugMode(bool debugMode)
	{
		jni_env->CallVoidMethod(MobileAppTracker, setDebugModeMethod, debugMode);
		return;
	}

	const void setGender(int gender)
	{
		jni_env->CallVoidMethod(MobileAppTracker, setGenderMethod, gender);
		return;
	}

	const void setLocation(double latitude, double longitude, double altitude)
	{
		jni_env->CallVoidMethod(MobileAppTracker, setLatitudeMethod, latitude);
		jni_env->CallVoidMethod(MobileAppTracker, setLongitudeMethod, longitude);
		jni_env->CallVoidMethod(MobileAppTracker, setAltitudeMethod, altitude);
		return;
	}

	const void setPackageName(char* packageName)
	{
		jstring packageNameUTF = jni_env->NewStringUTF(packageName);
		jni_env->CallVoidMethod(MobileAppTracker, setPackageNameMethod, packageNameUTF);
		jni_env->DeleteLocalRef(packageNameUTF);
		return;
	}

	const void setSiteId(char* siteId)
	{
		jstring siteIdUTF = jni_env->NewStringUTF(siteId);
		jni_env->CallVoidMethod(MobileAppTracker, setSiteIdMethod, siteIdUTF);
		jni_env->DeleteLocalRef(siteIdUTF);
		return;
	}

	const void setTRUSTeId(char* trusteId)
	{
		jstring trusteIdUTF = jni_env->NewStringUTF(trusteId);
		jni_env->CallVoidMethod(MobileAppTracker, setTRUSTeIdMethod, trusteIdUTF);
		jni_env->DeleteLocalRef(trusteIdUTF);
		return;
	}

	const void setUserId(char* userId)
	{
		jstring userIdUTF = jni_env->NewStringUTF(userId);
		jni_env->CallVoidMethod(MobileAppTracker, setUserIdMethod, userIdUTF);
		jni_env->DeleteLocalRef(userIdUTF);
		return;
	}

	const void setFacebookUserId(char* facebookUserId)
	{
		jstring userIdUTF = jni_env->NewStringUTF(facebookUserId);
		jni_env->CallVoidMethod(MobileAppTracker, setFacebookUserIdMethod, userIdUTF);
		jni_env->DeleteLocalRef(userIdUTF);
		return;
	}

	const void setTwitterUserId(char* twitterUserId)
	{
		jstring userIdUTF = jni_env->NewStringUTF(twitterUserId);
		jni_env->CallVoidMethod(MobileAppTracker, setTwitterUserIdMethod, userIdUTF);
		jni_env->DeleteLocalRef(userIdUTF);
		return;
	}

	const void setGoogleUserId(char* googleUserId)
	{
		jstring userIdUTF = jni_env->NewStringUTF(googleUserId);
		jni_env->CallVoidMethod(MobileAppTracker, setGoogleUserIdMethod, userIdUTF);
		jni_env->DeleteLocalRef(userIdUTF);
		return;
	}

	// iOS only functions that do nothing on Android
	
    const void setAppLevelOptOut(bool optout)
	{
		return;
	}
    
	const void setAppleAdvertisingIdentifier(char* appleAdvertisingIdentifier)
	{
		return;
	}
    
	const void setAppleVendorIdentifier(char* appleVendorIdentifier)
	{
		return;
	}

	const void setDelegate(bool enable)
	{
		return;
	}

	const void setOpenUDID(char* openUdid)
	{
		return;
	}

	const void setRedirectUrl(char* redirectUrl)
	{
		return;
	}

	const void setShouldAutoGenerateAppleAdvertisingIdentifier(bool shouldAutoGenerate)
	{
		return;
	}
	
	const void setMACAddress(char* mac)
	{
		return;
	}

	const void setODIN1(char* odin1)
	{
		return;
	}

	const void setShouldAutoGenerateAppleVendorIdentifier(bool shouldAutoGenerate)
	{
		return;
	}

	const void setUseCookieTracking(bool useCookieTracking)
	{
		return;
	}

	const void setUIID(char* uiid)
	{
		return;
	}

	const char * getSDKDataParameters()
	{
		return '\0';
	}

} // extern "C"
