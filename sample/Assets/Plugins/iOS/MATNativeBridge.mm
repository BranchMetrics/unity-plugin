#import "MATNativeBridge.h"

// corresponds to GameObject named MobileAppTracker in the Unity Project
const char * UNITY_SENDMESSAGE_CALLBACK_RECEIVER = "MobileAppTracker";

// corresponds to the MAT callback methods defined in the script attached to the above GameObject
const char * UNITY_SENDMESSAGE_CALLBACK_SUCCESS  = "trackerDidSucceed";
const char * UNITY_SENDMESSAGE_CALLBACK_FAILURE  = "trackerDidFail";
const char * UNITY_SENDMESSAGE_CALLBACK_ENQUEUED = "trackerDidEnqueueRequest";
const char * UNITY_SENDMESSAGE_CALLBACK_DEEPLINK = "trackerDidReceiveDeepLink";


#pragma mark - MobileAppTracker Plugin Helper Category

@interface MobileAppTracker (MATUnityPlugin)

+ (void)setPluginName:(NSString *)pluginName;

@end


#pragma mark - MATSDKDelegate

@interface MATSDKDelegate : NSObject<MobileAppTrackerDelegate>

// empty

@end

@implementation MATSDKDelegate

- (void)mobileAppTrackerDidSucceedWithData:(NSData *)data
{
    //NSString *str = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
    //NSLog(@"Native: MATSDKDelegate: success = %@", str);
    
    NSLog(@"Native: MATSDKDelegate: success");
    
    UnitySendMessage(UNITY_SENDMESSAGE_CALLBACK_RECEIVER, UNITY_SENDMESSAGE_CALLBACK_SUCCESS, [[self base64String:data] UTF8String]);
}

- (void)mobileAppTrackerDidFailWithError:(NSError *)error
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

- (void)mobileAppTrackerEnqueuedActionWithReferenceId:(NSString *)referenceId
{
    NSLog(@"Native: MATSDKDelegate: enqueued request");
    
    UnitySendMessage(UNITY_SENDMESSAGE_CALLBACK_RECEIVER, UNITY_SENDMESSAGE_CALLBACK_ENQUEUED, nil != referenceId ? [referenceId UTF8String] : "");
}

- (NSString *)base64String:(NSData *)data
{
    // Get NSString from NSData object in Base64
    NSString *encodedString = nil;
    
    // if iOS 7+
    if([NSData instancesRespondToSelector:@selector(base64EncodedStringWithOptions:)])
    {
        encodedString = [data base64EncodedStringWithOptions:NSDataBase64Encoding64CharacterLineLength];
    }
    else
    {
        encodedString = [data base64Encoding];
    }
    
    return encodedString;
}

@end


#pragma mark - MATAppDelegateListener

@interface MATAppDelegateListener : NSObject<AppDelegateListener>

+(MATAppDelegateListener *)sharedInstance;

@end


static MATAppDelegateListener *_instance = [MATAppDelegateListener sharedInstance];

@implementation MATAppDelegateListener

+(MATAppDelegateListener *)sharedInstance {
    return _instance;
}

+ (void)initialize {
    if(!_instance) {
        _instance = [[MATAppDelegateListener alloc] init];
    }
}

- (id)init {
    if(_instance != nil) {
        return _instance;
    }
    
    self = [super init];
    if(!self)
        return nil;
    
    _instance = self;
    
    UnityRegisterAppDelegateListener(self);
    
    return self;
}


#pragma mark - Unity AppDelegateListener Callback Methods

- (void)onOpenURL:(NSNotification*)notification {
    
    NSURL *url = [notification.userInfo objectForKey:@"url"];
    NSString *strSource = [notification.userInfo objectForKey:@"sourceApplication"];
    NSString *appBundleId = [[[NSBundle mainBundle] infoDictionary] objectForKey:(__bridge NSString*)kCFBundleIdentifierKey];
    
    if([strSource isEqualToString:appBundleId])
    {
        UnitySendMessage(UNITY_SENDMESSAGE_CALLBACK_RECEIVER, UNITY_SENDMESSAGE_CALLBACK_DEEPLINK, [[url absoluteString] UTF8String]);
    }
}

@end


#pragma mark - Helper Methods

// Converts C style string to NSString
NSString* MATCreateNSString (const char* string)
{
    return [NSString stringWithUTF8String:string ? string : ""];
}

NSData* MATCreateNSData (Byte bytes[], NSUInteger length)
{
    if(bytes)
    {
        return [NSData dataWithBytes:bytes length:length];
    }
    else
    {
        return nil;
    }
}

// Ref: http://answers.unity3d.com/questions/364779/passing-nsdictionary-from-obj-c-to-c.html
char* MATAutonomousStringCopy (const char* string)
{
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

NSArray *arrayFromItems(MATItemIos eventItems[], int eventItemCount)
{
    // reformat the items array as an nsarray of dictionary
    NSMutableArray *arrEventItems = [NSMutableArray array];
    
    if(eventItemCount > 0)
    {
        for (uint i = 0; i < eventItemCount; i++)
        {
            MATItemIos item = eventItems[i];
            
            NSString *name = MATCreateNSString(item.name);
            float unitPrice = (float)item.unitPrice;
            int quantity = item.quantity;
            float revenue = (float)item.revenue;
            
            NSString *attr1 = MATCreateNSString(item.attribute1);
            NSString *attr2 = MATCreateNSString(item.attribute2);
            NSString *attr3 = MATCreateNSString(item.attribute3);
            NSString *attr4 = MATCreateNSString(item.attribute4);
            NSString *attr5 = MATCreateNSString(item.attribute5);
            
            // convert from MATItem struct to MATEventItem object
            MATEventItem *eventItem = [MATEventItem eventItemWithName:name unitPrice:unitPrice quantity:quantity revenue:revenue
                                                           attribute1:attr1
                                                           attribute2:attr2
                                                           attribute3:attr3
                                                           attribute4:attr4
                                                           attribute5:attr5];
            
            [arrEventItems addObject:eventItem];
        }
    }
    
    return arrEventItems;
}

MATEvent *convertIosEvent(MATEventIos event, MATItemIos eventItems[], int eventItemCount, Byte receipt[], int receiptByteCount)
{
    MATEvent *evt = nil;
    
    if(event.name || event.eventId)
    {
        evt = event.name ? [MATEvent eventWithName:MATCreateNSString(event.name)] : [MATEvent eventWithId:[MATCreateNSString(event.eventId) integerValue]];
        
        if(event.revenue)
        {
            evt.revenue = [MATCreateNSString(event.revenue) floatValue];
        }
        
        if(event.currencyCode)
        {
            evt.currencyCode = MATCreateNSString(event.currencyCode);
        }
        
        if(event.advertiserRefId)
        {
            evt.refId = MATCreateNSString(event.advertiserRefId);
        }
        
        if(eventItemCount > 0)
        {
            evt.eventItems = arrayFromItems(eventItems, eventItemCount);
        }
        
        if(receiptByteCount > 0)
        {
            evt.receipt = MATCreateNSData(receipt, receiptByteCount);
        }
        
        if(event.transactionState)
        {
            evt.transactionState = [MATCreateNSString(event.transactionState) integerValue];
        }
        
        if(event.contentType)
        {
            evt.contentType = MATCreateNSString(event.contentType);
        }
        
        if(event.contentId)
        {
            evt.contentId = MATCreateNSString(event.contentId);
        }
        
        if(event.level)
        {
            evt.level = [MATCreateNSString(event.level) integerValue];
        }
        
        if(event.quantity)
        {
            evt.quantity = [MATCreateNSString(event.quantity) integerValue];
        }
        
        if(event.searchString)
        {
            evt.searchString = MATCreateNSString(event.searchString);
        }
        
        if(event.rating)
        {
            evt.rating = [MATCreateNSString(event.rating) floatValue];
        }
        
        if(event.date1)
        {
            // convert millis string to NSTimeInterval
            NSTimeInterval ti = [MATCreateNSString(event.date1) doubleValue] / 1000.0f;
            
            // convert NSTimeInterval to NSDate
            evt.date1 = [NSDate dateWithTimeIntervalSince1970:ti];
        }
        
        if(event.date2)
        {
            // convert millis string to NSTimeInterval
            NSTimeInterval ti = [MATCreateNSString(event.date2) doubleValue] / 1000.0f;
            
            // convert NSTimeInterval to NSDate
            evt.date2 = [NSDate dateWithTimeIntervalSince1970:ti];
        }
        
        if(event.attribute1)
        {
            evt.attribute1 = MATCreateNSString(event.attribute1);
        }
        if(event.attribute2)
        {
            evt.attribute2 = MATCreateNSString(event.attribute2);
        }
        if(event.attribute3)
        {
            evt.attribute3 = MATCreateNSString(event.attribute3);
        }
        if(event.attribute4)
        {
            evt.attribute4 = MATCreateNSString(event.attribute4);
        }
        if(event.attribute5)
        {
            evt.attribute5 = MATCreateNSString(event.attribute5);
        }
    }
    
    return evt;
}

MATPreloadData *convertIosPreloadData(MATPreloadDataIos preloadData)
{
    MATPreloadData *matPreloadData = nil;
    
    if(preloadData.publisherId)
    {
        matPreloadData = [MATPreloadData preloadDataWithPublisherId:MATCreateNSString(preloadData.publisherId)];
        
        if(preloadData.advertiserSubAd)
        {
            matPreloadData.advertiserSubAd = MATCreateNSString(preloadData.advertiserSubAd);
        }
        
        if(preloadData.advertiserSubAdgroup)
        {
            matPreloadData.advertiserSubAdgroup = MATCreateNSString(preloadData.advertiserSubAdgroup);
        }
        
        if(preloadData.advertiserSubCampaign)
        {
            matPreloadData.advertiserSubCampaign = MATCreateNSString(preloadData.advertiserSubCampaign);
        }
        
        if(preloadData.advertiserSubKeyword)
        {
            matPreloadData.advertiserSubKeyword = MATCreateNSString(preloadData.advertiserSubKeyword);
        }
        
        if(preloadData.advertiserSubPublisher)
        {
            matPreloadData.advertiserSubPublisher = MATCreateNSString(preloadData.advertiserSubPublisher);
        }
        
        if(preloadData.advertiserSubSite)
        {
            matPreloadData.advertiserSubSite = MATCreateNSString(preloadData.advertiserSubSite);
        }
        
        if(preloadData.agencyId)
        {
            matPreloadData.agencyId = MATCreateNSString(preloadData.agencyId);
        }
        
        if(preloadData.offerId)
        {
            matPreloadData.offerId = MATCreateNSString(preloadData.offerId);
        }
        
        if(preloadData.publisherReferenceId)
        {
            matPreloadData.publisherReferenceId = MATCreateNSString(preloadData.publisherReferenceId);
        }
        
        if(preloadData.publisherSub1)
        {
            matPreloadData.publisherSub1 = MATCreateNSString(preloadData.publisherSub1);
        }
        
        if(preloadData.publisherSub1)
        {
            matPreloadData.publisherSub1 = MATCreateNSString(preloadData.publisherSub1);
        }
        
        if(preloadData.publisherSub2)
        {
            matPreloadData.publisherSub2 = MATCreateNSString(preloadData.publisherSub2);
        }
        
        if(preloadData.publisherSub3)
        {
            matPreloadData.publisherSub3 = MATCreateNSString(preloadData.publisherSub3);
        }
        
        if(preloadData.publisherSub4)
        {
            matPreloadData.publisherSub4 = MATCreateNSString(preloadData.publisherSub4);
        }
        
        if(preloadData.publisherSub5)
        {
            matPreloadData.publisherSub5 = MATCreateNSString(preloadData.publisherSub5);
        }
        
        if(preloadData.publisherSubAd)
        {
            matPreloadData.publisherSubAd = MATCreateNSString(preloadData.publisherSubAd);
        }
        
        if(preloadData.publisherSubAdgroup)
        {
            matPreloadData.publisherSubAdgroup = MATCreateNSString(preloadData.publisherSubAdgroup);
        }
        
        if(preloadData.publisherSubCampaign)
        {
            matPreloadData.publisherSubCampaign = MATCreateNSString(preloadData.publisherSubCampaign);
        }
        
        if(preloadData.publisherSubKeyword)
        {
            matPreloadData.publisherSubKeyword = MATCreateNSString(preloadData.publisherSubKeyword);
        }
        
        if(preloadData.publisherSubPublisher)
        {
            matPreloadData.publisherSubPublisher = MATCreateNSString(preloadData.publisherSubPublisher);
        }
        
        if(preloadData.publisherSubSite)
        {
            matPreloadData.publisherSubSite = MATCreateNSString(preloadData.publisherSubSite);
        }
    }
    
    return matPreloadData;
}


MATSDKDelegate *matDelegate;

// When native code plugin is implemented in .mm / .cpp file, then functions
// should be surrounded with extern "C" block to conform to C function naming rules.
extern "C" {
    
    
#pragma mark - Init Method
    
    void MATInit (const char* advertiserId, const char* conversionKey)
    {
        NSLog(@"Native: initNativeCode = %s, %s", advertiserId, conversionKey);
        
        [MobileAppTracker initializeWithMATAdvertiserId:MATCreateNSString(advertiserId)
                                       MATConversionKey:MATCreateNSString(conversionKey)];
        [MobileAppTracker setPluginName:@"unity"];
    }
    
    
#pragma mark - Behavior Flags
    
    void MATCheckForDeferredDeeplinkWithTimeout(double timeoutMillis)
    {
        NSTimeInterval timeoutSeconds = timeoutMillis / 1000.0f;
        
        NSLog(@"Native: checkForDeferredDeeplinkWithTimeout: millis = %f, seconds = %f", timeoutMillis, timeoutSeconds);
        
        [MobileAppTracker checkForDeferredDeeplinkWithTimeout:timeoutSeconds];
    }
    
    void MATAutomateIapEventMeasurement(bool automate)
    {
        NSLog(@"Native: automateIapEventMeasurement = %d", automate);
        
        [MobileAppTracker automateIapEventMeasurement:automate];
    }
    
    void MATSetFacebookEventLogging(bool enable, bool limitEventAndDataUsage)
    {
        NSLog(@"Native: setFacebookEventLogging: enable = %d, limit = %d", enable, limitEventAndDataUsage);
        
        [MobileAppTracker setFacebookEventLogging:enable limitEventAndDataUsage:limitEventAndDataUsage];
    }
    
    
#pragma mark - Getter Methods
    
    const char* MATGetMatId()
    {
        NSLog(@"Native: getMatId");
        
        NSString *matId = [MobileAppTracker matId];
        char *strMatId = MATAutonomousStringCopy([matId UTF8String]);
        
        return strMatId;
    }
    
    bool MATGetIsPayingUser()
    {
        NSLog(@"Native: getIsPayingUser");
        
        return [MobileAppTracker isPayingUser];
    }
    
    const char* MATGetOpenLogId()
    {
        NSLog(@"Native: getOpenLogId");
        
        NSString *openLogId = [MobileAppTracker openLogId];
        char *strOpenLogId = MATAutonomousStringCopy([openLogId UTF8String]);
        
        return strOpenLogId;
    }
    
    
#pragma mark - Setter Methods
    
    void MATSetDelegate(bool enable)
    {
        NSLog(@"Native: setDelegate = %d", enable);
        
        // When enabled, create/set MATSDKDelegate object as the delegate for MobileAppTracker.
        matDelegate = enable ? (matDelegate ? nil : [[MATSDKDelegate alloc] init]) : nil;
        
        [MobileAppTracker setDelegate:matDelegate];
    }
    
    void MATSetAllowDuplicates(bool allowDuplicateRequests)
    {
        NSLog(@"Native: setAllowDuplicates = %d", allowDuplicateRequests);
        
        [MobileAppTracker setAllowDuplicateRequests:allowDuplicateRequests];
    }
    
    void MATSetShouldAutoDetectJailbroken(bool shouldAutoDetect)
    {
        NSLog(@"Native: setShouldAutoDetectJailbroken = %d", shouldAutoDetect);
        
        [MobileAppTracker setShouldAutoDetectJailbroken:shouldAutoDetect];
    }
    
    void MATSetShouldAutoGenerateAppleVendorIdentifier(bool shouldAutoGenerate)
    {
        NSLog(@"Native: setShouldAutoGenerateAppleVendorIdentifier = %d", shouldAutoGenerate);
        
        [MobileAppTracker setShouldAutoGenerateAppleVendorIdentifier:shouldAutoGenerate];
    }
    
    void MATSetUseCookieTracking(bool useCookieTracking)
    {
        NSLog(@"Native: setUseCookieTracking = %d", useCookieTracking);
        
        [MobileAppTracker setUseCookieTracking:useCookieTracking];
    }
    
    void MATSetExistingUser(bool isExisting)
    {
        NSLog(@"Native: setExistingUser = %d", isExisting);
        
        [MobileAppTracker setExistingUser:isExisting];
    }
    
    void MATSetPayingUser(bool isPaying)
    {
        NSLog(@"Native: setPayingUser = %d", isPaying);
        
        [MobileAppTracker setPayingUser:isPaying];
    }
    
    void MATSetPreloadData(MATPreloadDataIos preloadData)
    {
        NSLog(@"Native: setPreloadData");
        
        MATPreloadData *matPreloadData = convertIosPreloadData(preloadData);
        [MobileAppTracker setPreloadData:matPreloadData];
    }
    
    void MATSetJailbroken(bool isJailbroken)
    {
        NSLog(@"Native: setJailbroken = %d", isJailbroken);
        
        [MobileAppTracker setJailbroken:isJailbroken];
    }
    
    void MATSetAppAdTracking(bool enable)
    {
        NSLog(@"Native: setAppAdTracking = %d", enable);
        
        [MobileAppTracker setAppAdTracking:enable];
    }
    
    void MATSetCurrencyCode(const char* currencyCode)
    {
        NSLog(@"Native: setCurrencyCode = %s", currencyCode);
        
        [MobileAppTracker setCurrencyCode:MATCreateNSString(currencyCode)];
    }
    
    void MATSetDebugMode(bool enable)
    {
        NSLog(@"Native: setDebugMode = %d", enable);
        
        [MobileAppTracker setDebugMode:enable];
    }
    
    void MATSetPackageName(const char* packageName)
    {
        NSLog(@"Native: setPackageName = %s", packageName);
        
        [MobileAppTracker setPackageName:MATCreateNSString(packageName)];
    }
    
    void MATSetPhoneNumber(const char* phoneNumber)
    {
        NSLog(@"Native: setPhoneNumber = %s", phoneNumber);
        
        [MobileAppTracker setPhoneNumber:MATCreateNSString(phoneNumber)];
    }
    
    void MATSetSiteId(const char* siteId)
    {
        NSLog(@"Native: setSiteId: %s", siteId);
        
        [MobileAppTracker setSiteId:MATCreateNSString(siteId)];
    }
    
    void MATSetTRUSTeId(const char* tpid)
    {
        NSLog(@"Native: setTRUSTeId: %s", tpid);
        
        [MobileAppTracker setTRUSTeId:MATCreateNSString(tpid)];
    }
    
    void MATSetUserEmail(const char* userEmail)
    {
        NSLog(@"Native: setUserEmail: %s", userEmail);
        
        [MobileAppTracker setUserEmail:MATCreateNSString(userEmail)];
    }
    
    void MATSetUserId(const char* userId)
    {
        NSLog(@"Native: setUserId: %s", userId);
        
        [MobileAppTracker setUserId:MATCreateNSString(userId)];
    }
    
    void MATSetUserName(const char* userName)
    {
        NSLog(@"Native: setUserName: %s", userName);
        
        [MobileAppTracker setUserName:MATCreateNSString(userName)];
    }
    
    void MATSetFacebookUserId(const char* userId)
    {
        NSLog(@"Native: setFacebookUserId: %s", userId);
        
        [MobileAppTracker setFacebookUserId:MATCreateNSString(userId)];
    }
    
    void MATSetTwitterUserId(const char* userId)
    {
        NSLog(@"Native: setTwitterUserId: %s", userId);
        
        [MobileAppTracker setTwitterUserId:MATCreateNSString(userId)];
    }
    
    void MATSetGoogleUserId(const char* userId)
    {
        NSLog(@"Native: setGoogleUserId: %s", userId);
        
        [MobileAppTracker setGoogleUserId:MATCreateNSString(userId)];
    }
    
    void MATSetAppleAdvertisingIdentifier(const char* appleAdvertisingId, bool trackingEnabled)
    {
        NSLog(@"Native: setAppleAdvertisingIdentifier: %s advertisingTrackingEnabled:%d", appleAdvertisingId, trackingEnabled);
        
        [MobileAppTracker setAppleAdvertisingIdentifier:[[NSUUID alloc] initWithUUIDString:MATCreateNSString(appleAdvertisingId)] advertisingTrackingEnabled:trackingEnabled];
    }
    
    void MATSetAppleVendorIdentifier(const char* appleVendorId)
    {
        NSLog(@"Native: setAppleVendorIdentifier: %s", appleVendorId);
        
        [MobileAppTracker setAppleVendorIdentifier:[[NSUUID alloc] initWithUUIDString:MATCreateNSString(appleVendorId)] ];
    }
    
    void MATSetAge(int age)
    {
        NSLog(@"Native: setAge = %d", age);
        
        [MobileAppTracker setAge:age];
    }
    
    void MATSetGender(MATGender gender)
    {
        NSLog(@"Native: setGender = %d", gender);
        
        [MobileAppTracker setGender:gender];
    }
    
    void MATSetLocation(double latitude, double longitude, double altitude)
    {
        NSLog(@"Native: setLocation: %f, %f, %f", latitude, longitude, altitude);
        
        [MobileAppTracker setLatitude:latitude longitude:longitude altitude:altitude];
    }
    
    
#pragma mark - Measure Session
    
    void MATMeasureSession()
    {
        NSLog(@"Native: measureSession");
        
        [MobileAppTracker measureSession];
    }
    
    
#pragma mark - Measure Event Methods
    
    void MATMeasureEventName(const char* eventName)
    {
        NSLog(@"Native: measureEventName");
        
        [MobileAppTracker measureEventName:MATCreateNSString(eventName)];
    }
    
    void MATMeasureEventId(int eventId)
    {
        NSLog(@"Native: measureEventId");
        
        [MobileAppTracker measureEventId:eventId];
    }
    
    void MATMeasureEvent(MATEventIos event, MATItemIos eventItems[], int eventItemCount, Byte receipt[], int receiptByteCount)
    {
        NSLog(@"Native: measureEvent");
        
        MATEvent *matEvent = convertIosEvent(event, eventItems, eventItemCount, receipt, receiptByteCount);
        [MobileAppTracker measureEvent:matEvent];
    }
}
