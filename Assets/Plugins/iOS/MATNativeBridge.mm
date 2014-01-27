#import "MATNativeBridge.h"
#import "NSData+MATBase64.h"

// corresponds to GameObject named MobileAppTracker in the Unity Project
const char * UNITY_SENDMESSAGE_CALLBACK_RECEIVER = "MobileAppTracker";
// corresponds to the method defined in the script attached to the above GameObject
const char * UNITY_SENDMESSAGE_CALLBACK_SUCCESS = "trackerDidSucceed";
// corresponds to the method defined in the script attached to the above GameObject
const char * UNITY_SENDMESSAGE_CALLBACK_FAILURE = "trackerDidFail";

@interface MATSDKDelegate : NSObject<MobileAppTrackerDelegate>

// empty

@end

@implementation MATSDKDelegate

- (void)mobileAppTracker:(MobileAppTracker *)tracker didSucceedWithData:(NSData *)data
{
    //NSString *str = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
    //NSLog(@"Native: MATSDKDelegate: success = %@", str);
    
    NSLog(@"Native: MATSDKDelegate: success");
    
    UnitySendMessage(UNITY_SENDMESSAGE_CALLBACK_RECEIVER, UNITY_SENDMESSAGE_CALLBACK_SUCCESS, [[data base64EncodedString] UTF8String]);
}

- (void)mobileAppTracker:(MobileAppTracker *)tracker didFailWithError:(NSError *)error
{
    //NSLog(@"Native: MATSDKDelegate: error = %@", error);
    NSLog(@"Native: MATSDKDelegate: error");
    
    NSInteger errorCode = [error code];
    NSString *errorDescr = [error localizedDescription];
    
    NSString *errorURLString = nil;
    NSDictionary *dictError = [error userInfo];
    
    if(dictError)
    {
        errorURLString = [dictError objectForKey:NSURLErrorFailingURLStringErrorKey];
    }
    
    errorURLString = nil == error ? @"" : errorURLString;
    
    NSString *strError = [NSString stringWithFormat:@"{\"code\":\"%d\",\"localizedDescription\":\"%@\",\"failedURL\":\"%@\"}", errorCode, errorDescr, errorURLString];
    
    UnitySendMessage(UNITY_SENDMESSAGE_CALLBACK_RECEIVER, UNITY_SENDMESSAGE_CALLBACK_FAILURE, [strError UTF8String]);
}

@end

// Converts C style string to NSString
NSString* CreateNSString (const char* string)
{
	return [NSString stringWithUTF8String:string ? string : ""];
}

// Ref: http://answers.unity3d.com/questions/364779/passing-nsdictionary-from-obj-c-to-c.html
char* AutonomousStringCopy (const char* string)
{
	if (string == NULL)
		return NULL;
	
	char* res = (char*)malloc(strlen(string) + 1);
	strcpy(res, string);
	return res;
}

MATSDKDelegate *matDelegate;

// When native code plugin is implemented in .mm / .cpp file, then functions
// should be surrounded with extern "C" block to conform to C function naming rules.
extern "C" {
    
    void initNativeCode (const char* advertiserId, const char* conversionKey)
    {
        printf("Native: initNativeCode = %s, %s", advertiserId, conversionKey);
        
        [[MobileAppTracker sharedManager] startTrackerWithMATAdvertiserId:CreateNSString(advertiserId)
                                                         MATConversionKey:CreateNSString(conversionKey)];
        [[MobileAppTracker sharedManager] setPluginName:@"unity"];
    }
    
    void setDelegate(bool enable)
    {
        NSLog(@"Native: setDelegate = %d", enable);
        
        // When enabled, create/set MATSDKDelegate object as the delegate for MobileAppTracker.
        matDelegate = enable ? (matDelegate ? nil : [[MATSDKDelegate alloc] init]) : nil;
        
        [[MobileAppTracker sharedManager] setDelegate:matDelegate];
    }
    
    void setAllowDuplicateRequests(bool allowDuplicateRequests)
    {
        NSLog(@"Native: setAllowDuplicates = %d", allowDuplicateRequests);
        
        [[MobileAppTracker sharedManager] setAllowDuplicateRequests:allowDuplicateRequests];
    }
    
    void setShouldAutoDetectJailbroken(bool shouldAutoDetect)
    {
        NSLog(@"Native: setShouldAutoDetectJailbroken = %d", shouldAutoDetect);
        
        [[MobileAppTracker sharedManager] setShouldAutoDetectJailbroken:shouldAutoDetect];
    }
    
    void setMACAddress(const char * mac)
    {
        NSLog(@"Native: setMACAddress = %s", mac);
        
        [[MobileAppTracker sharedManager] setMACAddress:CreateNSString(mac)];
    }
    
    void setODIN1(const char * odin1)
    {
        NSLog(@"Native: setODIN1 = %s", odin1);
        
        [[MobileAppTracker sharedManager] setODIN1:CreateNSString(odin1)];
    }
    
    void setOpenUDID(const char * openUDID)
    {
        NSLog(@"Native: setOpenUDID = %s", openUDID);
        
        [[MobileAppTracker sharedManager] setOpenUDID:CreateNSString(openUDID)];
    }
    
    void setUIID(const char * uiid)
    {
        NSLog(@"Native: setUIID = %s", uiid);
        
        [[MobileAppTracker sharedManager] setUIID:CreateNSString(uiid)];
    }
    
    void setShouldAutoGenerateAppleVendorIdentifier(bool shouldAutoGenerate)
    {
        NSLog(@"Native: setShouldAutoGenerateAppleVendorIdentifier = %d", shouldAutoGenerate);
        
        [[MobileAppTracker sharedManager] setShouldAutoGenerateAppleVendorIdentifier:shouldAutoGenerate];
    }
    
    void setShouldAutoGenerateAppleAdvertisingIdentifier(bool shouldAutoGenerate)
    {
        NSLog(@"Native: setShouldAutoGenerateAppleAdvertisingIdentifier = %d", shouldAutoGenerate);
        
        [[MobileAppTracker sharedManager] setShouldAutoGenerateAppleAdvertisingIdentifier:shouldAutoGenerate];
    }
    
    void setUseCookieTracking(bool useCookieTracking)
    {
        NSLog(@"Native: setUseCookieTracking = %d", useCookieTracking);
        
        [[MobileAppTracker sharedManager] setUseCookieTracking:useCookieTracking];
    }
    
    void setJailbroken(bool isJailbroken)
    {
        NSLog(@"Native: setJailbroken = %d", isJailbroken);
        
        [[MobileAppTracker sharedManager] setJailbroken:isJailbroken];
    }
    
    void setAppAdTracking(bool enable)
    {
        NSLog(@"Native: setAppAdTracking = %d", enable);
        
        [[MobileAppTracker sharedManager] setAppAdTracking:enable];
    }
    
    void setCurrencyCode(const char* currencyCode)
    {
        NSLog(@"Native: setCurrencyCode = %s", currencyCode);
        
        [[MobileAppTracker sharedManager] setCurrencyCode:CreateNSString(currencyCode)];
    }
    
    void setRedirectUrl(const char* redirectUrl)
    {
        NSLog(@"Native: setRedirectUrl = %s", redirectUrl);
        
        [[MobileAppTracker sharedManager] setRedirectUrl:CreateNSString(redirectUrl)];
    }
    
    void setDebugMode(bool enable)
    {
        NSLog(@"Native: setDebugMode = %d", enable);
        
        [[MobileAppTracker sharedManager] setDebugMode:enable];
    }
    
    void setPackageName(const char* packageName)
    {
        NSLog(@"Native: setPackageName = %s", packageName);
        
        [[MobileAppTracker sharedManager] setPackageName:CreateNSString(packageName)];
    }
    
    void setSiteId(const char* siteId)
    {
        NSLog(@"Native: setSiteId: %s", siteId);
        
        [[MobileAppTracker sharedManager] setSiteId:CreateNSString(siteId)];
    }
    
    void setTRUSTeId(const char* trusteTPID)
    {
        NSLog(@"Native: setTRUSTeId: %s", trusteTPID);
        
        [[MobileAppTracker sharedManager] setTrusteTPID:CreateNSString(trusteTPID)];
    }
    
    void setUserId(const char* userId)
    {
        NSLog(@"Native: setUserId: %s", userId);
        
        [[MobileAppTracker sharedManager] setUserId:CreateNSString(userId)];
    }

    void setFacebookUserId(const char* userId)
    {
        NSLog(@"Native: setFacebookUserId: %s", userId);
        
        [[MobileAppTracker sharedManager] setFacebookUserId:CreateNSString(userId)];
    }

    void setTwitterUserId(const char* userId)
    {
        NSLog(@"Native: setTwitterUserId: %s", userId);
        
        [[MobileAppTracker sharedManager] setTwitterUserId:CreateNSString(userId)];
    }

    void setGoogleUserId(const char* userId)
    {
        NSLog(@"Native: setGoogleUserId: %s", userId);
        
        [[MobileAppTracker sharedManager] setGoogleUserId:CreateNSString(userId)];
    }
    
    void setAppleAdvertisingIdentifier(const char* appleAdvertisingId)
    {
        NSLog(@"Native: setAppleAdvertisingIdentifier: %s", appleAdvertisingId);
        
        [[MobileAppTracker sharedManager] setAppleAdvertisingIdentifier:[[NSUUID alloc] initWithUUIDString:CreateNSString(appleAdvertisingId)] ];
    }
    
    void setAppleVendorIdentifier(const char* appleVendorId)
    {
        NSLog(@"Native: setAppleVendorIdentifier: %s", appleVendorId);
        
        [[MobileAppTracker sharedManager] setAppleVendorIdentifier:[[NSUUID alloc] initWithUUIDString:CreateNSString(appleVendorId)] ];
    }
    
    void startAppToAppTracking(const char *targetAppId, const char *advertiserId, const char *offerId, const char *publisherId, bool shouldRedirect)
    {
        NSLog(@"Native: startAppToAppTracking: %s, %s, %s, %s, %d", targetAppId, advertiserId, offerId, publisherId, shouldRedirect);
        
        [[MobileAppTracker sharedManager] setTracking:CreateNSString(targetAppId) advertiserId:CreateNSString(advertiserId) offerId:CreateNSString(offerId) publisherId:CreateNSString(publisherId) redirect:shouldRedirect];
    }
    
    void setAge(int age)
    {
        NSLog(@"Native: setAge = %d", age);
        
        [[MobileAppTracker sharedManager] setAge:age];
    }
    
    void setGender(MATGender gender)
    {
        NSLog(@"Native: setGender = %d", gender);
        
        [[MobileAppTracker sharedManager] setGender:gender];
    }
    
    void setLocation(double latitude, double longitude, double altitude)
    {
        NSLog(@"Native: setLocation: %f, %f, %f", latitude, longitude, altitude);
        
        [[MobileAppTracker sharedManager] setLatitude:latitude longitude:longitude altitude:altitude];
    }
    
    const char * getSDKDataParameters()
    {
        // return the default NSString representation of the NSDictionary -- non-json string
        //return [[[[MobileAppTracker sharedManager] sdkDataParameters] description] UTF8String];
        
        NSLog(@"Native: getSDKDataParameters");
        
        // create an equivalent json representation of the NSDictionary
        NSMutableString *strParams = [NSMutableString stringWithFormat:@"{\"sdkDataParams\":{"];
        
        NSDictionary *dictParams = [[MobileAppTracker sharedManager] sdkDataParameters];
        NSArray *arrKeys = [dictParams allKeys];
        
        int count = [dictParams count];
        
        for(int i = 0; i < count; ++i)
        {
            NSString *key = [arrKeys objectAtIndex:i];
            [strParams appendFormat:@"\"%@\":\"%@\"%@", key, [dictParams objectForKey:key], i < (count - 1) ? @"," : @""];
        }
        
        [strParams appendFormat:@"}}"];
        
        char *strDataParams = AutonomousStringCopy([strParams UTF8String]);
        
        return strDataParams;
    }
    
    void trackInstall()
    {
        NSLog(@"Native: trackInstall");
        
        [[MobileAppTracker sharedManager] trackInstall];
    }
    
    void trackUpdate()
    {
        NSLog(@"Native: trackUpdate");
        
        [[MobileAppTracker sharedManager] trackUpdate];
    }
    
	void trackInstallWithReferenceId(const char *refId)
    {
        NSLog(@"Native: trackInstallWithReferenceId: refId = %s", refId);
        
        [[MobileAppTracker sharedManager] trackInstallWithReferenceId:refId ? CreateNSString(refId) : nil];
    }
	
	void trackUpdateWithReferenceId(const char *refId)
    {
        NSLog(@"Native: trackUpdateWithReferenceId: refId = %s", refId);
        
        [[MobileAppTracker sharedManager] trackUpdateWithReferenceId:refId ? CreateNSString(refId) : nil];
    }
    
    void trackAction(const char* eventName, bool isId, double revenue, const char*  currency)
    {
        NSLog(@"Native: trackAction");
        
        [[MobileAppTracker sharedManager] trackActionForEventIdOrName:CreateNSString(eventName)
                                                            eventIsId:isId
                                                        revenueAmount:revenue
                                                         currencyCode:CreateNSString(currency)];
    }
    
    void trackActionWithEventItem(const char* eventName, bool isId, MATItem eventItems[], int eventItemCount, const char* refId, double revenue, const char* currency, int transactionState, const char* receiptData, const char* receiptSignature)
    {
        NSLog(@"Native: trackActionWithEventItem");
        
        // reformat the items array as an nsarray of dictionary
        NSMutableArray *arrEventItems = [NSMutableArray array];
        for (uint i = 0; i < eventItemCount; i++)
        {
            MATItem item = eventItems[i];
            
            NSString *name = CreateNSString(item.name);
            float unitPrice = (float)item.unitPrice;
            int quantity = item.quantity;
            float revenue = (float)item.revenue;
            
            NSString *attr1 = CreateNSString(item.attribute1);
            NSString *attr2 = CreateNSString(item.attribute2);
            NSString *attr3 = CreateNSString(item.attribute3);
            NSString *attr4 = CreateNSString(item.attribute4);
            NSString *attr5 = CreateNSString(item.attribute5);
            
            // convert from MATItem struct to MATEventItem object
            MATEventItem *eventItem = [MATEventItem eventItemWithName:name unitPrice:unitPrice quantity:quantity revenue:revenue
                                                           attribute1:attr1
                                                           attribute2:attr2
                                                           attribute3:attr3
                                                           attribute4:attr4
                                                           attribute5:attr5];
            
            [arrEventItems addObject:eventItem];
        }
        
        NSString *strReceipt = CreateNSString(receiptData);
        NSData *receipt = [strReceipt dataUsingEncoding:NSUTF8StringEncoding];
        
        [[MobileAppTracker sharedManager] trackActionForEventIdOrName:CreateNSString(eventName)
                                                            eventIsId:isId
                                                           eventItems:arrEventItems
                                                          referenceId:CreateNSString(refId)
                                                        revenueAmount:revenue
                                                         currencyCode:CreateNSString(currency)
                                                     transactionState:(NSInteger)transactionState
                                                              receipt:receipt];
    }
}
