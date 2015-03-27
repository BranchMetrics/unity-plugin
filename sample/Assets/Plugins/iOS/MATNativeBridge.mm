#import "MATNativeBridge.h"

// corresponds to GameObject named MobileAppTracker in the Unity Project
const char * UNITY_SENDMESSAGE_CALLBACK_RECEIVER = "MobileAppTracker";

// corresponds to the MAT callback methods defined in the script attached to the above GameObject
const char * UNITY_SENDMESSAGE_CALLBACK_SUCCESS  = "trackerDidSucceed";
const char * UNITY_SENDMESSAGE_CALLBACK_FAILURE  = "trackerDidFail";
const char * UNITY_SENDMESSAGE_CALLBACK_ENQUEUED = "trackerDidEnqueueRequest";
const char * UNITY_SENDMESSAGE_CALLBACK_DEEPLINK = "trackerDidReceiveDeepLink";

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
        return [NSData data];
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

NSArray *arrayFromItems(MATItem eventItems[], int eventItemCount)
{
    // reformat the items array as an nsarray of dictionary
    NSMutableArray *arrEventItems = [NSMutableArray array];
    
    if(eventItemCount > 0)
    {
        for (uint i = 0; i < eventItemCount; i++)
        {
            MATItem item = eventItems[i];
            
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

void measureActionInternal(const char* eventName, MATItem eventItems[], int eventItemCount, const char* refId, double revenue, const char* currency)
{
    // reformat the items array as an nsarray of dictionary
    NSArray *arrEventItems = arrayFromItems(eventItems, eventItemCount);
    
    [MobileAppTracker measureAction:MATCreateNSString(eventName)
                         eventItems:arrEventItems
                        referenceId:MATCreateNSString(refId)
                      revenueAmount:revenue
                       currencyCode:MATCreateNSString(currency)];
}

void measureActionInternalWithReceiptData(const char* eventName, MATItem eventItems[], int eventItemCount, const char* refId, double revenue, const char* currency, int transactionState, Byte receiptData[], int byteCount)
{
    // reformat the items array as an nsarray of dictionary
    NSArray *arrEventItems = arrayFromItems(eventItems, eventItemCount);
    
    NSData *receipt = MATCreateNSData(receiptData, byteCount);
    
    [MobileAppTracker measureAction:MATCreateNSString(eventName)
                         eventItems:arrEventItems
                        referenceId:MATCreateNSString(refId)
                      revenueAmount:revenue
                       currencyCode:MATCreateNSString(currency)
                   transactionState:(NSInteger)transactionState
                            receipt:receipt];
}

MATSDKDelegate *matDelegate;

// When native code plugin is implemented in .mm / .cpp file, then functions
// should be surrounded with extern "C" block to conform to C function naming rules.
extern "C" {
    
    void initNativeCode (const char* advertiserId, const char* conversionKey)
    {
        NSLog(@"Native: initNativeCode = %s, %s", advertiserId, conversionKey);
        
        [MobileAppTracker initializeWithMATAdvertiserId:MATCreateNSString(advertiserId)
                                       MATConversionKey:MATCreateNSString(conversionKey)];
        [MobileAppTracker setPluginName:@"unity"];
    }
    
    void checkForDeferredDeeplinkWithTimeout(double timeoutMillis)
    {
        NSTimeInterval timeoutSeconds = timeoutMillis / 1000.0f;
        
        NSLog(@"Native: checkForDeferredDeeplinkWithTimeout: millis = %f, seconds = %f", timeoutMillis, timeoutSeconds);
        
        [MobileAppTracker checkForDeferredDeeplinkWithTimeout:timeoutSeconds];
    }
    
    const char* getMatId()
    {
        NSLog(@"Native: getMatId");
        
        NSString *matId = [MobileAppTracker matId];
        char *strMatId = MATAutonomousStringCopy([matId UTF8String]);
        
        return strMatId;
    }
    
    bool getIsPayingUser()
    {
        NSLog(@"Native: getIsPayingUser");
        
        return [MobileAppTracker isPayingUser];
    }
    
    const char* getOpenLogId()
    {
        NSLog(@"Native: getOpenLogId");
        
        NSString *openLogId = [MobileAppTracker openLogId];
        char *strOpenLogId = MATAutonomousStringCopy([openLogId UTF8String]);
        
        return strOpenLogId;
    }
    
    void setDelegate(bool enable)
    {
        NSLog(@"Native: setDelegate = %d", enable);
        
        // When enabled, create/set MATSDKDelegate object as the delegate for MobileAppTracker.
        matDelegate = enable ? (matDelegate ? nil : [[MATSDKDelegate alloc] init]) : nil;
        
        [MobileAppTracker setDelegate:matDelegate];
    }
    
    void setAllowDuplicates(bool allowDuplicateRequests)
    {
        NSLog(@"Native: setAllowDuplicates = %d", allowDuplicateRequests);
        
        [MobileAppTracker setAllowDuplicateRequests:allowDuplicateRequests];
    }
    
    void setShouldAutoDetectJailbroken(bool shouldAutoDetect)
    {
        NSLog(@"Native: setShouldAutoDetectJailbroken = %d", shouldAutoDetect);
        
        [MobileAppTracker setShouldAutoDetectJailbroken:shouldAutoDetect];
    }
    
    void setShouldAutoGenerateAppleVendorIdentifier(bool shouldAutoGenerate)
    {
        NSLog(@"Native: setShouldAutoGenerateAppleVendorIdentifier = %d", shouldAutoGenerate);
        
        [MobileAppTracker setShouldAutoGenerateAppleVendorIdentifier:shouldAutoGenerate];
    }
    
    void setUseCookieTracking(bool useCookieTracking)
    {
        NSLog(@"Native: setUseCookieTracking = %d", useCookieTracking);
        
        [MobileAppTracker setUseCookieTracking:useCookieTracking];
    }
    
    void setExistingUser(bool isExisting)
    {
        NSLog(@"Native: setExistingUser = %d", isExisting);
        
        [MobileAppTracker setExistingUser:isExisting];
    }
    
    void setPayingUser(bool isPaying)
    {
        NSLog(@"Native: setPayingUser = %d", isPaying);
        
        [MobileAppTracker setPayingUser:isPaying];
    }
    
    void setJailbroken(bool isJailbroken)
    {
        NSLog(@"Native: setJailbroken = %d", isJailbroken);
        
        [MobileAppTracker setJailbroken:isJailbroken];
    }
    
    void setAppAdTracking(bool enable)
    {
        NSLog(@"Native: setAppAdTracking = %d", enable);
        
        [MobileAppTracker setAppAdTracking:enable];
    }
    
    void setCurrencyCode(const char* currencyCode)
    {
        NSLog(@"Native: setCurrencyCode = %s", currencyCode);
        
        [MobileAppTracker setCurrencyCode:MATCreateNSString(currencyCode)];
    }
    
    void tuneSetDebugMode(bool enable)
    {
        NSLog(@"Native: setDebugMode = %d", enable);
        
        [MobileAppTracker setDebugMode:enable];
    }
    
    void setPackageName(const char* packageName)
    {
        NSLog(@"Native: setPackageName = %s", packageName);
        
        [MobileAppTracker setPackageName:MATCreateNSString(packageName)];
    }
    
    void setSiteId(const char* siteId)
    {
        NSLog(@"Native: setSiteId: %s", siteId);
        
        [MobileAppTracker setSiteId:MATCreateNSString(siteId)];
    }
    
    void setTRUSTeId(const char* tpid)
    {
        NSLog(@"Native: setTRUSTeId: %s", tpid);
        
        [MobileAppTracker setTRUSTeId:MATCreateNSString(tpid)];
    }
    
    void setUserEmail(const char* userEmail)
    {
        NSLog(@"Native: setUserEmail: %s", userEmail);
        
        [MobileAppTracker setUserEmail:MATCreateNSString(userEmail)];
    }
    
    void setUserId(const char* userId)
    {
        NSLog(@"Native: setUserId: %s", userId);
        
        [MobileAppTracker setUserId:MATCreateNSString(userId)];
    }
    
    void setUserName(const char* userName)
    {
        NSLog(@"Native: setUserName: %s", userName);
        
        [MobileAppTracker setUserName:MATCreateNSString(userName)];
    }
    
    void setFacebookEventLogging(bool enable, bool limitEventAndDataUsage)
    {
        NSLog(@"Native: setFacebookEventLogging: enable = %d, limit = %d", enable, limitEventAndDataUsage);
        
        [MobileAppTracker setFacebookEventLogging:enable limitEventAndDataUsage:limitEventAndDataUsage];
    }
    
    void setFacebookUserId(const char* userId)
    {
        NSLog(@"Native: setFacebookUserId: %s", userId);
        
        [MobileAppTracker setFacebookUserId:MATCreateNSString(userId)];
    }
    
    void setTwitterUserId(const char* userId)
    {
        NSLog(@"Native: setTwitterUserId: %s", userId);
        
        [MobileAppTracker setTwitterUserId:MATCreateNSString(userId)];
    }
    
    void setGoogleUserId(const char* userId)
    {
        NSLog(@"Native: setGoogleUserId: %s", userId);
        
        [MobileAppTracker setGoogleUserId:MATCreateNSString(userId)];
    }
    
    void setAppleAdvertisingIdentifier(const char* appleAdvertisingId, bool trackingEnabled)
    {
        NSLog(@"Native: setAppleAdvertisingIdentifier: %s advertisingTrackingEnabled:%d", appleAdvertisingId, trackingEnabled);
        
        [MobileAppTracker setAppleAdvertisingIdentifier:[[NSUUID alloc] initWithUUIDString:MATCreateNSString(appleAdvertisingId)] advertisingTrackingEnabled:trackingEnabled];
    }
    
    void setAppleVendorIdentifier(const char* appleVendorId)
    {
        NSLog(@"Native: setAppleVendorIdentifier: %s", appleVendorId);
        
        [MobileAppTracker setAppleVendorIdentifier:[[NSUUID alloc] initWithUUIDString:MATCreateNSString(appleVendorId)] ];
    }
    
    void setAge(int age)
    {
        NSLog(@"Native: setAge = %d", age);
        
        [MobileAppTracker setAge:age];
    }
    
    void setEventAttribute1(const char* value)
    {
        NSLog(@"Native: setEventAttribute1: %s", value);
        
        [MobileAppTracker setEventAttribute1:MATCreateNSString(value)];
    }
    
    void setEventAttribute2(const char* value)
    {
        NSLog(@"Native: setEventAttribute2: %s", value);
        
        [MobileAppTracker setEventAttribute2:MATCreateNSString(value)];
    }
    
    void setEventAttribute3(const char* value)
    {
        NSLog(@"Native: setEventAttribute3: %s", value);
        
        [MobileAppTracker setEventAttribute3:MATCreateNSString(value)];
    }
    
    void setEventAttribute4(const char* value)
    {
        NSLog(@"Native: setEventAttribute4: %s", value);
        
        [MobileAppTracker setEventAttribute4:MATCreateNSString(value)];
    }
    
    void setEventAttribute5(const char* value)
    {
        NSLog(@"Native: setEventAttribute5: %s", value);
        
        [MobileAppTracker setEventAttribute5:MATCreateNSString(value)];
    }
    
    void setEventContentType(const char* value)
    {
        NSLog(@"Native: setEventContentType: %s", value);
        
        [MobileAppTracker setEventContentType:MATCreateNSString(value)];
    }
    
    void setEventContentId(const char* value)
    {
        NSLog(@"Native: setEventContentId: %s", value);
        
        [MobileAppTracker setEventContentId:MATCreateNSString(value)];
    }
    
    void setEventDate1(const char* value)
    {
        NSLog(@"Native: setEventDate1: %s", value);
        
        NSString *strMillis = MATCreateNSString(value);
        
        double millis = [strMillis doubleValue];
        
        NSDate *date = [NSDate dateWithTimeIntervalSince1970:millis / 1000];
        
        [MobileAppTracker setEventDate1:date];
    }
    
    void setEventDate2(const char* value)
    {
        NSLog(@"Native: setEventDate2: %s", value);
        
        NSString *strMillis = MATCreateNSString(value);
        
        double millis = [strMillis doubleValue];
        
        NSDate *date = [NSDate dateWithTimeIntervalSince1970:millis / 1000];
        
        [MobileAppTracker setEventDate2:date];
    }
    
    void setEventLevel(int level)
    {
        NSLog(@"Native: setEventLevel: %d", level);
        
        [MobileAppTracker setEventLevel:level];
    }
    
    void setEventQuantity(int quantity)
    {
        NSLog(@"Native: setEventQuantity: %d", quantity);
        
        [MobileAppTracker setEventQuantity:quantity];
    }
    
    void setEventRating(float rating)
    {
        NSLog(@"Native: setEventRating: %f", rating);
        
        [MobileAppTracker setEventRating:rating];
    }
    
    void setEventSearchString(const char* value)
    {
        NSLog(@"Native: setEventSearchString: %s", value);
        
        [MobileAppTracker setEventSearchString:MATCreateNSString(value)];
    }
    
    void setGender(MATGender gender)
    {
        NSLog(@"Native: setGender = %d", gender);
        
        [MobileAppTracker setGender:gender];
    }
    
    void setLocation(double latitude, double longitude, double altitude)
    {
        NSLog(@"Native: setLocation: %f, %f, %f", latitude, longitude, altitude);
        
        [MobileAppTracker setLatitude:latitude longitude:longitude altitude:altitude];
    }
    
    void measureSession()
    {
        NSLog(@"Native: measureSession");
        
        [MobileAppTracker measureSession];
    }
    
    void measureAction(const char* eventName)
    {
        NSLog(@"Native: measureAction");
        
        measureActionInternal(eventName, NULL, -1, NULL, 0, NULL);
    }
    
    void measureActionWithRefId(const char* eventName, const char* refId)
    {
        NSLog(@"Native: measureActionWithRefId");
        
        measureActionInternal(eventName, NULL, -1, refId, 0, NULL);
    }
    
    void measureActionWithRevenue(const char* eventName, double revenue, const char*  currency, const char* refId)
    {
        NSLog(@"Native: measureActionWithRevenue");
        
        measureActionInternal(eventName, NULL, -1, refId, revenue, currency);
    }
    
    void measureActionWithEventItems(const char* eventName, MATItem eventItems[], int eventItemCount, const char* refId, double revenue, const char* currency, int transactionState, Byte receiptData[], int byteCount)
    {
        NSLog(@"Native: measureActionWithEventItems");
        
        measureActionInternalWithReceiptData(eventName, eventItems, eventItemCount, refId, revenue, currency, transactionState, receiptData, byteCount);
    }
    
    const char* setGoogleAdvertisingId(const char* advertisingId, bool limitAdTracking)
    {
        // Android only method
        
        NSLog(@"Native: setGoogleAdvertisingId: only supported on Android");
        
        return NULL;
    }
}
