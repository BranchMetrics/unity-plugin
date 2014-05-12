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

    JavaVM*    java_vm;
    JNIEnv*    jni_env;

    jobject     MobileAppTracker;
    
    jmethodID   getIsPayingUserMethod;
    jmethodID   getMatIdMethod;
    jmethodID   getOpenLogIdMethod;
    
    jmethodID   measureActionMethod;
    jmethodID   measureActionWithEventItemsMethod;
    jmethodID   measureActionWithEventItemsAndReceiptMethod;
    jmethodID   measureSessionMethod;
    jmethodID   setAgeMethod;
    jmethodID   setAllowDuplicatesMethod;
    jmethodID   setAltitudeMethod;
    jmethodID   setAppAdTrackingMethod;
    jmethodID   setCurrencyCodeMethod;
    jmethodID   setDebugModeMethod;
    jmethodID   setEventAttribute1Method;
    jmethodID   setEventAttribute2Method;
    jmethodID   setEventAttribute3Method;
    jmethodID   setEventAttribute4Method;
    jmethodID   setEventAttribute5Method;
    jmethodID   setEventContentIdMethod;
    jmethodID   setEventContentTypeMethod;
    jmethodID   setEventDate1Method;
    jmethodID   setEventDate2Method;
    jmethodID   setEventLevelMethod;
    jmethodID   setEventQuantityMethod;
    jmethodID   setEventRatingMethod;
    jmethodID   setEventSearchStringMethod;
    jmethodID   setExistingUserMethod;
    jmethodID   setGenderMethod;
    jmethodID   setGoogleAdvertisingIdMethod;
    jmethodID   setLatitudeMethod;
    jmethodID   setLongitudeMethod;
    jmethodID   setPackageNameMethod;
    jmethodID   setPayingUserMethod;
    jmethodID   setRefIdMethod;
    jmethodID   setRevenueMethod;
    jmethodID   setSiteIdMethod;
    jmethodID   setTRUSTeIdMethod;
    jmethodID   setUserEmailMethod;
    jmethodID   setUserIdMethod;
    jmethodID   setUserNameMethod;
    jmethodID   setFacebookUserIdMethod;
    jmethodID   setTwitterUserIdMethod;
    jmethodID   setGoogleUserIdMethod;
    jmethodID   startAppToAppTrackingMethod;
    
    const void measureAction(char* eventName);
    const void measureActionWithRefId(char* eventName, char* refId);
    const void measureActionWithRevenue(char* eventName, double revenue, char* currencyCode, char* refId);
    const void measureActionWithEventItems(char* eventName, MATItem items[], int eventItemCount, char* refId, double revenue, char* currency, int transactionState, char* receiptData, char* receiptSignature);
    
    jint JNI_OnLoad(JavaVM* vm, void* reserved)
    {
        // Use __android_log_print for logcat debugging
        __android_log_print(ANDROID_LOG_INFO, "MATJavaBridge", "[%s] Creating java link vm = %08x\n", __FUNCTION__, vm);
        java_vm = vm;
        jni_env = 0;
        // Attach our thread to the Java VM to get the JNI env
        java_vm->AttachCurrentThread(&jni_env, 0);
        __android_log_print(ANDROID_LOG_INFO, "MATJavaBridge", "[%s] JNI Environment is = %08x\n", __FUNCTION__, jni_env);

        return JNI_VERSION_1_6;     // minimum JNI version
    }

    const void initNativeCode (char* advertiserId, char* conversionKey)
    {
        // Convert char arrays to Java Strings for passing in to MobileAppTracker constructor
        jstring advertiserIdUTF         = jni_env->NewStringUTF(advertiserId);
        jstring conversionKeyUTF        = jni_env->NewStringUTF(conversionKey); 

        // Find our main activity..
        jclass cls_Activity             = jni_env->FindClass("com/unity3d/player/UnityPlayer");
        jfieldID fid_Activity           = jni_env->GetStaticFieldID(cls_Activity, "currentActivity", "Landroid/app/Activity;");
        jobject obj_Activity            = jni_env->GetStaticObjectField(cls_Activity, fid_Activity);
        __android_log_print(ANDROID_LOG_INFO, "MATJavaBridge", "[%s] Current activity = %08x\n", __FUNCTION__, obj_Activity);

        // Create a MobileAppTracker object
        jclass cls_MobileAppTracker     = jni_env->FindClass("com/mobileapptracker/MobileAppTracker");

        jmethodID initMethod = jni_env->GetStaticMethodID(cls_MobileAppTracker, "init", "(Landroid/content/Context;Ljava/lang/String;Ljava/lang/String;)V");
        jni_env->CallStaticVoidMethod(cls_MobileAppTracker, initMethod, obj_Activity, advertiserIdUTF, conversionKeyUTF);

        jmethodID getInstanceMethod = jni_env->GetStaticMethodID(cls_MobileAppTracker, "getInstance", "()Lcom/mobileapptracker/MobileAppTracker;");
        jobject obj_MobileAppTracker = jni_env->CallStaticObjectMethod(cls_MobileAppTracker, getInstanceMethod);
        __android_log_print(ANDROID_LOG_INFO, "MATJavaBridge", "[%s] MobileAppTracker object = %08x\n", __FUNCTION__, obj_MobileAppTracker);

        // Set SDK plugin name
        jmethodID setPluginMethod = jni_env->GetMethodID(cls_MobileAppTracker, "setPluginName", "(Ljava/lang/String;)V");
        jstring pluginUTF = jni_env->NewStringUTF("unity");
        jni_env->CallVoidMethod(obj_MobileAppTracker, setPluginMethod, pluginUTF);

        // Set referral sources
        jmethodID setReferralSourcesMethod = jni_env->GetMethodID(cls_MobileAppTracker, "setReferralSources", "(Landroid/app/Activity;)V");
        jni_env->CallVoidMethod(obj_MobileAppTracker, setReferralSourcesMethod, obj_Activity);

        // Start GAID fetch
        jclass cls_GAIDFetcher = jni_env->FindClass("com/mobileapptracker/gaidwrapper/GAIDFetcher");
        jmethodID mid_GAIDFetcher = jni_env->GetMethodID(cls_GAIDFetcher, "<init>", "()V");
        jobject obj_GAIDFetcher = jni_env->NewObject(cls_GAIDFetcher, mid_GAIDFetcher);

        jmethodID useUnityInterfaceMethod = jni_env->GetMethodID(cls_GAIDFetcher, "useUnityFetcherInterface", "()V");
        jni_env->CallVoidMethod(obj_GAIDFetcher, useUnityInterfaceMethod);

        jmethodID fetchGAIDMethod = jni_env->GetMethodID(cls_GAIDFetcher, "fetchGAID", "(Landroid/content/Context;)V");
        jni_env->CallVoidMethod(obj_GAIDFetcher, fetchGAIDMethod, obj_Activity);

        // Create a global reference to the MobileAppTracker object and fetch method ids
        MobileAppTracker        = jni_env->NewGlobalRef(obj_MobileAppTracker);
        
        getIsPayingUserMethod   = jni_env->GetMethodID(cls_MobileAppTracker, "getIsPayingUser", "()Z");
        getMatIdMethod          = jni_env->GetMethodID(cls_MobileAppTracker, "getMatId", "()Ljava/lang/String;");
        getOpenLogIdMethod      = jni_env->GetMethodID(cls_MobileAppTracker, "getOpenLogId", "()Ljava/lang/String;");
        
        measureSessionMethod                        = jni_env->GetMethodID(cls_MobileAppTracker, "measureSession", "()V");
        measureActionMethod                         = jni_env->GetMethodID(cls_MobileAppTracker, "measureAction", "(Ljava/lang/String;DLjava/lang/String;Ljava/lang/String;)V");
        measureActionWithEventItemsMethod           = jni_env->GetMethodID(cls_MobileAppTracker, "measureAction", "(Ljava/lang/String;Ljava/util/List;DLjava/lang/String;Ljava/lang/String;)V");
        measureActionWithEventItemsAndReceiptMethod = jni_env->GetMethodID(cls_MobileAppTracker, "measureAction", "(Ljava/lang/String;Ljava/util/List;DLjava/lang/String;Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;)V");

        setAgeMethod                 = jni_env->GetMethodID(cls_MobileAppTracker, "setAge", "(I)V");
        setAllowDuplicatesMethod     = jni_env->GetMethodID(cls_MobileAppTracker, "setAllowDuplicates", "(Z)V");
        setAltitudeMethod            = jni_env->GetMethodID(cls_MobileAppTracker, "setAltitude", "(D)V");
        setAppAdTrackingMethod       = jni_env->GetMethodID(cls_MobileAppTracker, "setAppAdTrackingEnabled", "(Z)V");
        setCurrencyCodeMethod        = jni_env->GetMethodID(cls_MobileAppTracker, "setCurrencyCode", "(Ljava/lang/String;)V");
        setDebugModeMethod           = jni_env->GetMethodID(cls_MobileAppTracker, "setDebugMode", "(Z)V");
        setEventAttribute1Method     = jni_env->GetMethodID(cls_MobileAppTracker, "setEventAttribute1", "(Ljava/lang/String;)V");
        setEventAttribute2Method     = jni_env->GetMethodID(cls_MobileAppTracker, "setEventAttribute2", "(Ljava/lang/String;)V");
        setEventAttribute3Method     = jni_env->GetMethodID(cls_MobileAppTracker, "setEventAttribute3", "(Ljava/lang/String;)V");
        setEventAttribute4Method     = jni_env->GetMethodID(cls_MobileAppTracker, "setEventAttribute4", "(Ljava/lang/String;)V");
        setEventAttribute5Method     = jni_env->GetMethodID(cls_MobileAppTracker, "setEventAttribute5", "(Ljava/lang/String;)V");
        setEventContentIdMethod      = jni_env->GetMethodID(cls_MobileAppTracker, "setEventContentId", "(Ljava/lang/String;)V");
        setEventContentTypeMethod    = jni_env->GetMethodID(cls_MobileAppTracker, "setEventContentType", "(Ljava/lang/String;)V");
        setEventDate1Method          = jni_env->GetMethodID(cls_MobileAppTracker, "setEventDate1", "(Ljava/util/Date;)V");
        setEventDate2Method          = jni_env->GetMethodID(cls_MobileAppTracker, "setEventDate2", "(Ljava/util/Date;)V");
        setEventLevelMethod          = jni_env->GetMethodID(cls_MobileAppTracker, "setEventLevel", "(I)V");
        setEventQuantityMethod       = jni_env->GetMethodID(cls_MobileAppTracker, "setEventQuantity", "(I)V");
        setEventRatingMethod         = jni_env->GetMethodID(cls_MobileAppTracker, "setEventRating", "(F)V");
        setEventSearchStringMethod   = jni_env->GetMethodID(cls_MobileAppTracker, "setEventSearchString", "(Ljava/lang/String;)V");
        setExistingUserMethod        = jni_env->GetMethodID(cls_MobileAppTracker, "setExistingUser", "(Z)V");
        setFacebookUserIdMethod      = jni_env->GetMethodID(cls_MobileAppTracker, "setFacebookUserId", "(Ljava/lang/String;)V");
        setGenderMethod              = jni_env->GetMethodID(cls_MobileAppTracker, "setGender", "(I)V");
        setGoogleAdvertisingIdMethod = jni_env->GetMethodID(cls_MobileAppTracker, "setGoogleAdvertisingId", "(Ljava/lang/String;Z)V");
        setGoogleUserIdMethod        = jni_env->GetMethodID(cls_MobileAppTracker, "setGoogleUserId", "(Ljava/lang/String;)V");
        setLatitudeMethod            = jni_env->GetMethodID(cls_MobileAppTracker, "setLatitude", "(D)V");
        setLongitudeMethod           = jni_env->GetMethodID(cls_MobileAppTracker, "setLongitude", "(D)V");
        setPackageNameMethod         = jni_env->GetMethodID(cls_MobileAppTracker, "setPackageName", "(Ljava/lang/String;)V");
        setPayingUserMethod          = jni_env->GetMethodID(cls_MobileAppTracker, "setIsPayingUser", "(Z)V");
        setSiteIdMethod              = jni_env->GetMethodID(cls_MobileAppTracker, "setSiteId", "(Ljava/lang/String;)V");
        setTRUSTeIdMethod            = jni_env->GetMethodID(cls_MobileAppTracker, "setTRUSTeId", "(Ljava/lang/String;)V");
        setTwitterUserIdMethod       = jni_env->GetMethodID(cls_MobileAppTracker, "setTwitterUserId", "(Ljava/lang/String;)V");
        setUserEmailMethod           = jni_env->GetMethodID(cls_MobileAppTracker, "setUserEmail", "(Ljava/lang/String;)V");
        setUserIdMethod              = jni_env->GetMethodID(cls_MobileAppTracker, "setUserId", "(Ljava/lang/String;)V");
        setUserNameMethod            = jni_env->GetMethodID(cls_MobileAppTracker, "setUserName", "(Ljava/lang/String;)V");

        startAppToAppTrackingMethod  = jni_env->GetMethodID(cls_MobileAppTracker, "startAppToAppTracking", "(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;Z)Ljava/lang/String;");

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

    const void measureAction(char* eventName)
    {
        measureActionWithRefId(eventName, NULL);
    }
    
    const void measureActionWithRefId(char* eventName, char* refId)
    {
        measureActionWithRevenue(eventName, 0, NULL, refId);
    }

    const void measureActionWithRevenue(char* eventName, double revenue, char* currencyCode, char* refId)
    {
        jstring eventNameUTF = jni_env->NewStringUTF(eventName);
        jstring currencyCodeUTF = jni_env->NewStringUTF(currencyCode);
        jstring refIdUTF = jni_env->NewStringUTF(refId);
        jni_env->CallVoidMethod(MobileAppTracker, measureActionMethod, eventNameUTF, revenue, currencyCodeUTF, refIdUTF);
        __android_log_print(ANDROID_LOG_INFO, "MATJavaBridge", "[%s]\n", __FUNCTION__);
        
        jni_env->DeleteLocalRef(eventNameUTF);
        jni_env->DeleteLocalRef(currencyCodeUTF);
        jni_env->DeleteLocalRef(refIdUTF);
        
        return;
    }
    
    const void measureActionWithEventItems(char* eventName, MATItem items[], int eventItemCount, char* refId, double revenue, char* currency, int transactionState, char* receiptData, char* receiptSignature)
    {
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
        
        // Convert event values to UTF
        jstring eventNameUTF = jni_env->NewStringUTF(eventName);
        jstring refIdUTF = jni_env->NewStringUTF(refId);
        jstring currencyCodeUTF = jni_env->NewStringUTF(currency);
        
        if (receiptData && strlen(receiptData) > 0 && receiptSignature && strlen(receiptSignature) > 0) {
            jstring receiptDataUTF = jni_env->NewStringUTF(receiptData);
            jstring receiptSignatureUTF = jni_env->NewStringUTF(receiptSignature);
            // Call measureActionWithEventItems with receipt data
            jni_env->CallVoidMethod(MobileAppTracker, measureActionWithEventItemsAndReceiptMethod, eventNameUTF, jlistobj, revenue, currencyCodeUTF, refIdUTF, receiptDataUTF, receiptSignatureUTF);
            jni_env->DeleteLocalRef(receiptDataUTF);
            jni_env->DeleteLocalRef(receiptSignatureUTF);
        } else {
            // Call measureActionWithEventItems
            jni_env->CallVoidMethod(MobileAppTracker, measureActionWithEventItemsMethod, eventNameUTF, jlistobj, revenue, currencyCodeUTF, refIdUTF);
        }
        __android_log_print(ANDROID_LOG_INFO, "MATJavaBridge", "[%s]\n", __FUNCTION__);
        
        // Delete local variables used
        jni_env->DeleteLocalRef(eventNameUTF);
        jni_env->DeleteLocalRef(currencyCodeUTF);
        jni_env->DeleteLocalRef(refIdUTF);
        jni_env->DeleteLocalRef(jlistobj);
        
        return;
    }

    const void measureSession()
    {
        jni_env->CallVoidMethod(MobileAppTracker, measureSessionMethod);
        __android_log_print(ANDROID_LOG_INFO, "MATJavaBridge", "[%s]\n", __FUNCTION__);
        return;
    }

    const void setAge(int age)
    {
        jni_env->CallVoidMethod(MobileAppTracker, setAgeMethod, age);
        return;
    }

    const void setAllowDuplicates(bool allowDuplicates)
    {
        jni_env->CallVoidMethod(MobileAppTracker, setAllowDuplicatesMethod, allowDuplicates);
        return;
    }

    const void setAppAdTracking(bool appAdTracking)
    {
        jni_env->CallVoidMethod(MobileAppTracker, setAppAdTrackingMethod, appAdTracking);
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

    const void setEventAttribute1(char* value)
    {
        jstring valueUTF = jni_env->NewStringUTF(value);
        jni_env->CallVoidMethod(MobileAppTracker, setEventAttribute1Method, valueUTF);
        jni_env->DeleteLocalRef(valueUTF);
        return;
    }

    const void setEventAttribute2(char* value)
    {
        jstring valueUTF = jni_env->NewStringUTF(value);
        jni_env->CallVoidMethod(MobileAppTracker, setEventAttribute2Method, valueUTF);
        jni_env->DeleteLocalRef(valueUTF);
        return;
    }

    const void setEventAttribute3(char* value)
    {
        jstring valueUTF = jni_env->NewStringUTF(value);
        jni_env->CallVoidMethod(MobileAppTracker, setEventAttribute3Method, valueUTF);
        jni_env->DeleteLocalRef(valueUTF);
        return;
    }

    const void setEventAttribute4(char* value)
    {
        jstring valueUTF = jni_env->NewStringUTF(value);
        jni_env->CallVoidMethod(MobileAppTracker, setEventAttribute4Method, valueUTF);
        jni_env->DeleteLocalRef(valueUTF);
        return;
    }

    const void setEventAttribute5(char* value)
    {
        jstring valueUTF = jni_env->NewStringUTF(value);
        jni_env->CallVoidMethod(MobileAppTracker, setEventAttribute5Method, valueUTF);
        jni_env->DeleteLocalRef(valueUTF);
        return;
    }
    
    const void setEventContentId(char* contentId)
    {
        jstring contentIdUTF = jni_env->NewStringUTF(contentId);
        jni_env->CallVoidMethod(MobileAppTracker, setEventContentIdMethod, contentIdUTF);
        jni_env->DeleteLocalRef(contentIdUTF);
        return;
    }
    
    const void setEventContentType(char* contentType)
    {
        jstring contentTypeUTF = jni_env->NewStringUTF(contentType);
        jni_env->CallVoidMethod(MobileAppTracker, setEventContentTypeMethod, contentTypeUTF);
        jni_env->DeleteLocalRef(contentTypeUTF);
        return;
    }
    
    const void setEventDate1(char* dateMillis)
    {
        jstring dateMillisUTF = jni_env->NewStringUTF(dateMillis);
        
        jclass clsDouble = jni_env->FindClass("java/lang/Double");
        jmethodID ctrDouble = jni_env->GetMethodID(clsDouble, "<init>", "(Ljava/lang/String;)V");
        jobject objDouble = jni_env->NewObject(clsDouble, ctrDouble, dateMillisUTF);
        
        jmethodID methodLongValue = jni_env->GetMethodID(clsDouble, "longValue", "()J");
        jlong millis = jni_env->CallLongMethod(objDouble, methodLongValue);
        
        jclass clsDate = jni_env->FindClass("java/util/Date");
        jmethodID ctrDate = jni_env->GetMethodID(clsDate, "<init>", "(J)V");
        jobject objDate = jni_env->NewObject(clsDate, ctrDate, millis);
        
        jni_env->CallVoidMethod(MobileAppTracker, setEventDate1Method, objDate);
        
        jni_env->DeleteLocalRef(dateMillisUTF);
        jni_env->DeleteLocalRef(objDouble);
        jni_env->DeleteLocalRef(objDate);
        
        return;
    }
    
    const void setEventDate2(char* dateMillis)
    {
        jstring dateMillisUTF = jni_env->NewStringUTF(dateMillis);
        
        jclass clsDouble = jni_env->FindClass("java/lang/Double");
        jmethodID ctrDouble = jni_env->GetMethodID(clsDouble, "<init>", "(Ljava/lang/String;)V");
        jobject objDouble = jni_env->NewObject(clsDouble, ctrDouble, dateMillisUTF);
        
        jmethodID methodLongValue = jni_env->GetMethodID(clsDouble, "longValue", "()J");
        jlong millis = jni_env->CallLongMethod(objDouble, methodLongValue);
        
        jclass clsDate = jni_env->FindClass("java/util/Date");
        jmethodID ctrDate = jni_env->GetMethodID(clsDate, "<init>", "(J)V");
        jobject objDate = jni_env->NewObject(clsDate, ctrDate, millis);
        
        jni_env->CallVoidMethod(MobileAppTracker, setEventDate2Method, objDate);
        
        jni_env->DeleteLocalRef(dateMillisUTF);
        jni_env->DeleteLocalRef(objDouble);
        jni_env->DeleteLocalRef(objDate);
        
        return;
    }
    
    const void setEventLevel(int level)
    {
        jni_env->CallVoidMethod(MobileAppTracker, setEventLevelMethod, level);
        return;
    }
    
    const void setEventQuantity(int quantity)
    {
        jni_env->CallVoidMethod(MobileAppTracker, setEventQuantityMethod, quantity);
        return;
    }
    
    const void setEventRating(float rating)
    {
        jni_env->CallVoidMethod(MobileAppTracker, setEventRatingMethod, rating);
        return;
    }
    
    const void setEventSearchString(char* searchString)
    {
        jstring searchStringUTF = jni_env->NewStringUTF(searchString);
        jni_env->CallVoidMethod(MobileAppTracker, setEventSearchStringMethod, searchStringUTF);
        jni_env->DeleteLocalRef(searchStringUTF);
        return;
    }

    const void setExistingUser(bool isExisting)
    {
        jni_env->CallVoidMethod(MobileAppTracker, setExistingUserMethod, isExisting);
        return;
    }
    
    const void setPayingUser(bool isPaying)
    {
        jni_env->CallVoidMethod(MobileAppTracker, setPayingUserMethod, isPaying);
        return;
    }

    const void setFacebookUserId(char* facebookUserId)
    {
        jstring userIdUTF = jni_env->NewStringUTF(facebookUserId);
        jni_env->CallVoidMethod(MobileAppTracker, setFacebookUserIdMethod, userIdUTF);
        jni_env->DeleteLocalRef(userIdUTF);
        return;
    }

    const void setGender(int gender)
    {
        jni_env->CallVoidMethod(MobileAppTracker, setGenderMethod, gender);
        return;
    }
    
    const void setGoogleAdvertisingId(char* adId, bool isLATEnabled)
    {
        jstring adIdUTF = jni_env->NewStringUTF(adId);
        jni_env->CallVoidMethod(MobileAppTracker, setGoogleAdvertisingIdMethod, adIdUTF, isLATEnabled);
        jni_env->DeleteLocalRef(adIdUTF);
        return;
    }

    const void setGoogleUserId(char* googleUserId)
    {
        jstring userIdUTF = jni_env->NewStringUTF(googleUserId);
        jni_env->CallVoidMethod(MobileAppTracker, setGoogleUserIdMethod, userIdUTF);
        jni_env->DeleteLocalRef(userIdUTF);
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

    const void setTwitterUserId(char* twitterUserId)
    {
        jstring userIdUTF = jni_env->NewStringUTF(twitterUserId);
        jni_env->CallVoidMethod(MobileAppTracker, setTwitterUserIdMethod, userIdUTF);
        jni_env->DeleteLocalRef(userIdUTF);
        return;
    }

    const void setUserEmail(char* userEmail)
    {
        jstring userEmailUTF = jni_env->NewStringUTF(userEmail);
        jni_env->CallVoidMethod(MobileAppTracker, setUserEmailMethod, userEmailUTF);
        jni_env->DeleteLocalRef(userEmailUTF);
        return;
    }

    const void setUserId(char* userId)
    {
        jstring userIdUTF = jni_env->NewStringUTF(userId);
        jni_env->CallVoidMethod(MobileAppTracker, setUserIdMethod, userIdUTF);
        jni_env->DeleteLocalRef(userIdUTF);
        return;
    }

    const void setUserName(char* userName)
    {
        jstring userNameUTF = jni_env->NewStringUTF(userName);
        jni_env->CallVoidMethod(MobileAppTracker, setUserNameMethod, userNameUTF);
        jni_env->DeleteLocalRef(userNameUTF);
        return;
    }
    
    const bool getIsPayingUser()
    {
        bool payingUser = jni_env->CallBooleanMethod(MobileAppTracker, getIsPayingUserMethod);
        __android_log_print(ANDROID_LOG_INFO, "MATJavaBridge", "[%s] payingUser = %d\n", __FUNCTION__, payingUser);
        return payingUser;
    }

    const char* getMatId()
    {
        jstring matId = (jstring)jni_env->CallObjectMethod(MobileAppTracker, getMatIdMethod);
        const char* matIdChars = jni_env->GetStringUTFChars(matId, NULL);
        __android_log_print(ANDROID_LOG_INFO, "MATJavaBridge", "[%s] matId = %s\n", __FUNCTION__, matIdChars);
        return matIdChars;
    }
    
    const char* getOpenLogId()
    {
        jstring openLogId = (jstring)jni_env->CallObjectMethod(MobileAppTracker, getOpenLogIdMethod);
        const char* openLogIdChars = jni_env->GetStringUTFChars(openLogId, NULL);
        __android_log_print(ANDROID_LOG_INFO, "MATJavaBridge", "[%s] openLogId = %s\n", __FUNCTION__, openLogIdChars);
        return openLogIdChars;
    }

    // iOS only functions that do nothing on Android

    const void setAppLevelOptOut(bool optout)
    {
        return;
    }

    const void setAppleAdvertisingIdentifier(char* appleAdvertisingIdentifier, bool trackingEnabled)
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

    const void setRedirectUrl(char* redirectUrl)
    {
        return;
    }

    const void setShouldAutoDetectJailbroken(bool shouldAutoDetect)
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

    const void setJailbroken(bool isJailbroken)
    {
        return;
    }


    JNIEXPORT void JNICALL Java_com_mobileapptracker_gaidwrapper_UnityGAIDInterface_nativeRetrievedGAID(JNIEnv *env, jobject obj, jstring gaid, jboolean isLimitAdTrackingEnabled)
    {
        jstring localstr_gaid = (jstring)(env->NewLocalRef(gaid));
        env->CallVoidMethod(MobileAppTracker, setGoogleAdvertisingIdMethod, localstr_gaid, isLimitAdTrackingEnabled);
        env->DeleteLocalRef(localstr_gaid);
    }
} // extern "C"
