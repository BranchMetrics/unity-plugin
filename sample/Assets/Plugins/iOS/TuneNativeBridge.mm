#import "TuneNativeBridge.h"

extern UIViewController *UnityGetGLViewController(); // Root view controller of Unity screen.

// corresponds to GameObject named "TuneListener" in the Unity Project
const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_RECEIVER = "TuneListener";

// corresponds to the Tune callback methods defined in the script attached to the above GameObject
const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_SUCCEEDED  = "trackerDidSucceed";
const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_FAILED  = "trackerDidFail";
const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_ENQUEUED = "trackerDidEnqueueRequest";
const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_DEEPLINK_RECEIVED = "trackerDidReceiveDeeplink";
const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_DEEPLINK_FAILED = "trackerDidFailDeeplink";

const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_POWERHOOKS_CHANGED = "onPowerHooksChanged";
const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_FIRST_PLAYLIST_DOWNLOAD = "onFirstPlaylistDownloaded";


#pragma mark - Tune Plugin Helper Category

@interface Tune (TuneUnityPlugin)

+ (void)setPluginName:(NSString *)pluginName;

@end


@interface TuneSdkDelegate : NSObject<TuneDelegate>

// empty

@end


@implementation TuneSdkDelegate


#pragma mark - TuneSdkDelegate Methods

- (void)tuneDidSucceedWithData:(NSData *)data
{
    //NSString *str = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
    //NSLog(@"Native: TuneSdkDelegate: success = %@", str);
    
    NSLog(@"Native: TuneSdkDelegate: success");
    
    UnitySendMessage(UNITY_SENDMESSAGE_CALLBACK_TUNE_RECEIVER, UNITY_SENDMESSAGE_CALLBACK_TUNE_SUCCEEDED, [[[self class] base64String:data] UTF8String]);
}

- (void)tuneDidFailWithError:(NSError *)error
{
    //NSLog(@"Native: TuneSdkDelegate: error = %@", error);
    NSLog(@"Native: TuneSdkDelegate: error");
    
    NSInteger errorCode = [error code];
    NSString *errorDescr = [error localizedDescription];
    
    NSString *errorURLString = nil;
    NSDictionary *dictError = [error userInfo];
    
    if(dictError)
    {
        errorURLString = [dictError objectForKey:NSURLErrorFailingURLStringErrorKey];
    }
    
    errorURLString = nil == error ? @"" : errorURLString;
    
    NSString *strError = [NSString stringWithFormat:@"{\"code\":\"%zd\",\"localizedDescription\":\"%@\",\"failedURL\":\"%@\"}", errorCode, errorDescr, errorURLString];
    
    UnitySendMessage(UNITY_SENDMESSAGE_CALLBACK_TUNE_RECEIVER, UNITY_SENDMESSAGE_CALLBACK_TUNE_FAILED, [strError UTF8String]);
}

- (void)tuneEnqueuedActionWithReferenceId:(NSString *)referenceId
{
    NSLog(@"Native: TuneSdkDelegate: enqueued request");
    
    UnitySendMessage(UNITY_SENDMESSAGE_CALLBACK_TUNE_RECEIVER, UNITY_SENDMESSAGE_CALLBACK_TUNE_ENQUEUED, nil != referenceId ? [referenceId UTF8String] : "");
}

- (void)tuneDidReceiveDeeplink:(NSString *)deeplink
{
    NSLog(@"Native: TuneSdkDelegate: deeplinkReceived: %@", deeplink);
    
    UnitySendMessage(UNITY_SENDMESSAGE_CALLBACK_TUNE_RECEIVER, UNITY_SENDMESSAGE_CALLBACK_TUNE_DEEPLINK_RECEIVED, nil != deeplink ? [deeplink UTF8String] : "");
}

- (void)tuneDidFailDeeplinkWithError:(NSError *)error
{
    NSLog(@"Native: TuneSdkDelegate: deeplinkError");
    
    NSInteger errorCode = [error code];
    NSString *errorDescr = [error localizedDescription];
    
    NSString *errorURLString = nil;
    NSDictionary *dictError = [error userInfo];
    
    if(dictError)
    {
        errorURLString = [dictError objectForKey:NSURLErrorFailingURLStringErrorKey];
    }
    
    errorURLString = nil == error ? @"" : errorURLString;
    
    NSString *strError = [NSString stringWithFormat:@"{\"code\":\"%zd\",\"localizedDescription\":\"%@\",\"failedURL\":\"%@\"}", errorCode, errorDescr, errorURLString];
    
    UnitySendMessage(UNITY_SENDMESSAGE_CALLBACK_TUNE_RECEIVER, UNITY_SENDMESSAGE_CALLBACK_TUNE_DEEPLINK_FAILED, [strError UTF8String]);
}

+ (NSString *)base64String:(NSData *)data
{
    // Get NSString from NSData object in Base64
    NSString *encodedString = nil;
    
    // if iOS 7+
    if([data respondsToSelector:@selector(base64EncodedStringWithOptions:)])
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


@interface TuneAppDelegateListener : NSObject<AppDelegateListener>

+(TuneAppDelegateListener *)sharedInstance;

@end


static TuneAppDelegateListener *_instance = [TuneAppDelegateListener sharedInstance];


@implementation TuneAppDelegateListener


#pragma mark - TuneAppDelegateListener Methods

+(TuneAppDelegateListener *)sharedInstance {
    return _instance;
}

+ (void)initialize {
    if(!_instance) {
        _instance = [TuneAppDelegateListener new];
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
    
    [Tune applicationDidOpenURL:url.absoluteString sourceApplication:strSource];
}

@end


#pragma mark - Helper Methods

// Converts C style string to NSString
NSString* TuneCreateNSString (const char* string)
{
    return [NSString stringWithUTF8String:string ?: ""];
}

NSData* TuneCreateNSData (Byte bytes[], NSUInteger length)
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
char* TuneAutonomousStringCopy (const char* string)
{
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

char* TuneSerializePowerHookOrExperimentDetails(NSDictionary *dict) {
    NSString *jsonString = nil;
    if (dict) {
        NSMutableDictionary *detailsDictionary = [NSMutableDictionary new];
        NSMutableArray *keys = [NSMutableArray new];
        NSMutableArray *values = [NSMutableArray new];
        
        // Convert TunePowerHookExperimentDetails/TuneInAppMessageExperimentDetails values to NSDictionary in order to convert to string
        [dict enumerateKeysAndObjectsUsingBlock:^(id  _Nonnull key, id  _Nonnull obj, BOOL * _Nonnull stop) {
            id innerDict = [obj toDictionary];
            if ([NSJSONSerialization isValidJSONObject:innerDict]) {
                [keys addObject:key];
                [values addObject:innerDict];
            }
        }];
        
        detailsDictionary[@"keys"] = keys;
        detailsDictionary[@"values"] = values;
        
        NSError *err;
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:detailsDictionary options:0 error:&err]; 
        jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    } else {
        jsonString = @"";
    }
    
    return TuneAutonomousStringCopy([jsonString UTF8String]);
}

NSArray *arrayFromItems(TuneItemIos eventItems[], int eventItemCount)
{
    // reformat the items array as an nsarray of dictionary
    NSMutableArray *arrEventItems = [NSMutableArray array];
    
    if(eventItemCount > 0)
    {
        for (uint i = 0; i < eventItemCount; i++)
        {
            TuneItemIos item = eventItems[i];
            
            NSString *name = TuneCreateNSString(item.name);
            float unitPrice = (float)item.unitPrice;
            int quantity = item.quantity;
            float revenue = (float)item.revenue;
            
            NSString *attr1 = TuneCreateNSString(item.attribute1);
            NSString *attr2 = TuneCreateNSString(item.attribute2);
            NSString *attr3 = TuneCreateNSString(item.attribute3);
            NSString *attr4 = TuneCreateNSString(item.attribute4);
            NSString *attr5 = TuneCreateNSString(item.attribute5);
            
            // convert from TuneItemIos struct to TuneEventItem object
            TuneEventItem *eventItem = [TuneEventItem eventItemWithName:name unitPrice:unitPrice quantity:quantity revenue:revenue
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

TuneEvent *convertIosEvent(TuneEventIos event, TuneItemIos eventItems[], int eventItemCount, Byte receipt[], int receiptByteCount)
{
    
    TuneEvent *evt = nil;
    
    if(event.name || event.eventId)
    {
        evt = event.name ? [TuneEvent eventWithName:TuneCreateNSString(event.name)] : [TuneEvent eventWithId:[TuneCreateNSString(event.eventId) integerValue]];
        
        if(event.revenue)
        {
            evt.revenue = [TuneCreateNSString(event.revenue) floatValue];
        }
        
        if(event.currencyCode)
        {
            evt.currencyCode = TuneCreateNSString(event.currencyCode);
        }
        
        if(event.advertiserRefId)
        {
            evt.refId = TuneCreateNSString(event.advertiserRefId);
        }
        
        if(eventItemCount > 0)
        {
            evt.eventItems = arrayFromItems(eventItems, eventItemCount);
        }
        
        if(receiptByteCount > 0)
        {
            evt.receipt = TuneCreateNSData(receipt, receiptByteCount);
        }
        
        if(event.transactionState)
        {
            evt.transactionState = [TuneCreateNSString(event.transactionState) integerValue];
        }
        
        if(event.contentType)
        {
            evt.contentType = TuneCreateNSString(event.contentType);
        }
        
        if(event.contentId)
        {
            evt.contentId = TuneCreateNSString(event.contentId);
        }
        
        if(event.level)
        {
            evt.level = [TuneCreateNSString(event.level) integerValue];
        }
        
        if(event.quantity)
        {
            evt.quantity = [TuneCreateNSString(event.quantity) integerValue];
        }
        
        if(event.searchString)
        {
            evt.searchString = TuneCreateNSString(event.searchString);
        }
        
        if(event.rating)
        {
            evt.rating = [TuneCreateNSString(event.rating) floatValue];
        }
        
        if(event.date1)
        {
            // convert millis string to NSTimeInterval
            NSTimeInterval ti = [TuneCreateNSString(event.date1) doubleValue] / 1000.0f;
            
            // convert NSTimeInterval to NSDate
            evt.date1 = [NSDate dateWithTimeIntervalSince1970:ti];
        }
        
        if(event.date2)
        {
            // convert millis string to NSTimeInterval
            NSTimeInterval ti = [TuneCreateNSString(event.date2) doubleValue] / 1000.0f;
            
            // convert NSTimeInterval to NSDate
            evt.date2 = [NSDate dateWithTimeIntervalSince1970:ti];
        }
        
        if(event.attribute1)
        {
            evt.attribute1 = TuneCreateNSString(event.attribute1);
        }
        if(event.attribute2)
        {
            evt.attribute2 = TuneCreateNSString(event.attribute2);
        }
        if(event.attribute3)
        {
            evt.attribute3 = TuneCreateNSString(event.attribute3);
        }
        if(event.attribute4)
        {
            evt.attribute4 = TuneCreateNSString(event.attribute4);
        }
        if(event.attribute5)
        {
            evt.attribute5 = TuneCreateNSString(event.attribute5);
        }
    }
    
    return evt;
}

TunePreloadData *convertIosPreloadData(TunePreloadDataIos preloadData)
{
    TunePreloadData *tunePreloadData = nil;
    
    if(preloadData.publisherId)
    {
        tunePreloadData = [TunePreloadData preloadDataWithPublisherId:TuneCreateNSString(preloadData.publisherId)];
        
        if(preloadData.advertiserSubAd)
        {
            tunePreloadData.advertiserSubAd = TuneCreateNSString(preloadData.advertiserSubAd);
        }
        
        if(preloadData.advertiserSubAdgroup)
        {
            tunePreloadData.advertiserSubAdgroup = TuneCreateNSString(preloadData.advertiserSubAdgroup);
        }
        
        if(preloadData.advertiserSubCampaign)
        {
            tunePreloadData.advertiserSubCampaign = TuneCreateNSString(preloadData.advertiserSubCampaign);
        }
        
        if(preloadData.advertiserSubKeyword)
        {
            tunePreloadData.advertiserSubKeyword = TuneCreateNSString(preloadData.advertiserSubKeyword);
        }
        
        if(preloadData.advertiserSubPublisher)
        {
            tunePreloadData.advertiserSubPublisher = TuneCreateNSString(preloadData.advertiserSubPublisher);
        }
        
        if(preloadData.advertiserSubSite)
        {
            tunePreloadData.advertiserSubSite = TuneCreateNSString(preloadData.advertiserSubSite);
        }
        
        if(preloadData.agencyId)
        {
            tunePreloadData.agencyId = TuneCreateNSString(preloadData.agencyId);
        }
        
        if(preloadData.offerId)
        {
            tunePreloadData.offerId = TuneCreateNSString(preloadData.offerId);
        }
        
        if(preloadData.publisherReferenceId)
        {
            tunePreloadData.publisherReferenceId = TuneCreateNSString(preloadData.publisherReferenceId);
        }
        
        if(preloadData.publisherSub1)
        {
            tunePreloadData.publisherSub1 = TuneCreateNSString(preloadData.publisherSub1);
        }
        
        if(preloadData.publisherSub1)
        {
            tunePreloadData.publisherSub1 = TuneCreateNSString(preloadData.publisherSub1);
        }
        
        if(preloadData.publisherSub2)
        {
            tunePreloadData.publisherSub2 = TuneCreateNSString(preloadData.publisherSub2);
        }
        
        if(preloadData.publisherSub3)
        {
            tunePreloadData.publisherSub3 = TuneCreateNSString(preloadData.publisherSub3);
        }
        
        if(preloadData.publisherSub4)
        {
            tunePreloadData.publisherSub4 = TuneCreateNSString(preloadData.publisherSub4);
        }
        
        if(preloadData.publisherSub5)
        {
            tunePreloadData.publisherSub5 = TuneCreateNSString(preloadData.publisherSub5);
        }
        
        if(preloadData.publisherSubAd)
        {
            tunePreloadData.publisherSubAd = TuneCreateNSString(preloadData.publisherSubAd);
        }
        
        if(preloadData.publisherSubAdgroup)
        {
            tunePreloadData.publisherSubAdgroup = TuneCreateNSString(preloadData.publisherSubAdgroup);
        }
        
        if(preloadData.publisherSubCampaign)
        {
            tunePreloadData.publisherSubCampaign = TuneCreateNSString(preloadData.publisherSubCampaign);
        }
        
        if(preloadData.publisherSubKeyword)
        {
            tunePreloadData.publisherSubKeyword = TuneCreateNSString(preloadData.publisherSubKeyword);
        }
        
        if(preloadData.publisherSubPublisher)
        {
            tunePreloadData.publisherSubPublisher = TuneCreateNSString(preloadData.publisherSubPublisher);
        }
        
        if(preloadData.publisherSubSite)
        {
            tunePreloadData.publisherSubSite = TuneCreateNSString(preloadData.publisherSubSite);
        }
    }
    
    return tunePreloadData;
}

void (^firstPlaylistBlock)(void) = ^{
    NSLog(@"Entered first playlist download block");
    UnitySendMessage(UNITY_SENDMESSAGE_CALLBACK_TUNE_RECEIVER, UNITY_SENDMESSAGE_CALLBACK_TUNE_FIRST_PLAYLIST_DOWNLOAD, "");
};

void (^powerhooksChangedBlock)(void) = ^{
    NSLog(@"Entered power hooks changed block");
    UnitySendMessage(UNITY_SENDMESSAGE_CALLBACK_TUNE_RECEIVER, UNITY_SENDMESSAGE_CALLBACK_TUNE_POWERHOOKS_CHANGED, "");
};


TuneSdkDelegate *tuneDelegate;
TuneSdkDelegate *tuneDeeplinkDelegate;


// When native code plugin is implemented in .mm / .cpp file, then functions
// should be surrounded with extern "C" block to conform to C function naming rules.
extern "C" {
    
    
#pragma mark - Init Method
    
    void TuneInit (const char* advertiserId, const char* conversionKey)
    {
        NSLog(@"Native: initNativeCode = %s, %s", advertiserId, conversionKey);

        [Tune initializeWithTuneAdvertiserId:TuneCreateNSString(advertiserId)
                           tuneConversionKey:TuneCreateNSString(conversionKey)];
        [Tune setPluginName:@"unity"];
    }
    
    void TuneInitForWearable (const char* advertiserId, const char* conversionKey, const char* packageName, BOOL wearable)
    {
        NSLog(@"Native: initNativeCode = %s, %s", advertiserId, conversionKey);
        
        [Tune initializeWithTuneAdvertiserId:TuneCreateNSString(advertiserId)
                           tuneConversionKey:TuneCreateNSString(conversionKey)
                             tunePackageName:TuneCreateNSString(packageName)
                                    wearable:wearable];
        [Tune setPluginName:@"unity"];
    }
    
    
#pragma mark - Behavior Flags
    
    void TuneCheckForDeferredDeeplink()
    {
        NSLog(@"Native: checkForDeferredDeeplink");
        
        tuneDeeplinkDelegate = tuneDeeplinkDelegate ?: [TuneSdkDelegate new];
        
        [Tune checkForDeferredDeeplink:tuneDeeplinkDelegate];
    }
    
    void TuneAutomateIapEventMeasurement(bool automate)
    {
        NSLog(@"Native: automateIapEventMeasurement = %d", automate);
        
        [Tune automateIapEventMeasurement:automate];
    }
    
    void TuneSetFacebookEventLogging(bool enable, bool limitEventAndDataUsage)
    {
        NSLog(@"Native: setFacebookEventLogging: enable = %d, limit = %d", enable, limitEventAndDataUsage);
        
        [Tune setFacebookEventLogging:enable limitEventAndDataUsage:limitEventAndDataUsage];
    }
    
    
#pragma mark - Getter Methods
    
    const char* TuneGetTuneId()
    {
        NSLog(@"Native: getTuneId");
        
        NSString *tuneId = [Tune tuneId];
        char *strTuneId = TuneAutonomousStringCopy([tuneId UTF8String]);
        
        return strTuneId;
    }
    
    bool TuneGetIsPayingUser()
    {
        NSLog(@"Native: getIsPayingUser");
        
        return [Tune isPayingUser];
    }
    
    const char* TuneGetOpenLogId()
    {
        NSLog(@"Native: getOpenLogId");
        
        NSString *openLogId = [Tune openLogId];
        char *strOpenLogId = TuneAutonomousStringCopy([openLogId UTF8String]);
        
        return strOpenLogId;
    }
    
    
#pragma mark - Setter Methods
    
    void TuneSetDelegate(bool enable)
    {
        NSLog(@"Native: setDelegate = %d", enable);
        
        // When enabled, create/set TuneSdkDelegate object as the delegate for Tune.
        tuneDelegate = enable ? (tuneDelegate ? nil : [TuneSdkDelegate new]) : nil;
        
        [Tune setDelegate:tuneDelegate];
    }
    
    void TuneSetShouldAutoCollectAppleAdvertisingIdentifier(bool shouldAutoCollect)
    {
        NSLog(@"Native: setShouldAutoCollectAppleAdvertisingIdentifier = %d", shouldAutoCollect);
        
        [Tune setShouldAutoCollectAppleAdvertisingIdentifier:shouldAutoCollect];
    }
    
    void TuneSetShouldAutoCollectDeviceLocation(bool shouldAutoCollect)
    {
        NSLog(@"Native: setShouldAutoCollectDeviceLocation = %d", shouldAutoCollect);
        
        [Tune setShouldAutoCollectDeviceLocation:shouldAutoCollect];
    }
    
    void TuneSetShouldAutoDetectJailbroken(bool shouldAutoDetect)
    {
        NSLog(@"Native: setShouldAutoDetectJailbroken = %d", shouldAutoDetect);
        
        [Tune setShouldAutoDetectJailbroken:shouldAutoDetect];
    }
    
    void TuneSetShouldAutoGenerateAppleVendorIdentifier(bool shouldAutoGenerate)
    {
        NSLog(@"Native: setShouldAutoGenerateAppleVendorIdentifier = %d", shouldAutoGenerate);
        
        [Tune setShouldAutoGenerateAppleVendorIdentifier:shouldAutoGenerate];
    }
    
    void TuneSetUseCookieTracking(bool useCookieTracking)
    {
        NSLog(@"Native: setUseCookieTracking = %d", useCookieTracking);
        
        [Tune setUseCookieMeasurement:useCookieTracking];
    }
    
    void TuneSetExistingUser(bool isExisting)
    {
        NSLog(@"Native: setExistingUser = %d", isExisting);
        
        [Tune setExistingUser:isExisting];
    }
    
    void TuneSetPayingUser(bool isPaying)
    {
        NSLog(@"Native: setPayingUser = %d", isPaying);
        
        [Tune setPayingUser:isPaying];
    }
    
    void TuneSetPreloadData(TunePreloadDataIos preloadData)
    {
        NSLog(@"Native: setPreloadData");
        
        TunePreloadData *matPreloadData = convertIosPreloadData(preloadData);
        [Tune setPreloadData:matPreloadData];
    }
    
    void TuneSetJailbroken(bool isJailbroken)
    {
        NSLog(@"Native: setJailbroken = %d", isJailbroken);
        
        [Tune setJailbroken:isJailbroken];
    }
    
    void TuneSetAppAdTracking(bool enable)
    {
        NSLog(@"Native: setAppAdTracking = %d", enable);
        
        [Tune setAppAdMeasurement:enable];
    }
    
    void TuneSetDebugMode(bool enable)
    {
        NSLog(@"Native: setDebugMode = %d", enable);
        
        [Tune setDebugMode:enable];
    }
    
    void TuneSetPackageName(const char* packageName)
    {
        NSLog(@"Native: setPackageName = %s", packageName);
        
        [Tune setPackageName:TuneCreateNSString(packageName)];
    }
    
    void TuneSetPhoneNumber(const char* phoneNumber)
    {
        NSLog(@"Native: setPhoneNumber = %s", phoneNumber);
        
        [Tune setPhoneNumber:TuneCreateNSString(phoneNumber)];
    }
    
    void TuneSetTRUSTeId(const char* tpid)
    {
        NSLog(@"Native: setTRUSTeId: %s", tpid);
        
        [Tune setTRUSTeId:TuneCreateNSString(tpid)];
    }
    
    void TuneSetUserEmail(const char* userEmail)
    {
        NSLog(@"Native: setUserEmail: %s", userEmail);
        
        [Tune setUserEmail:TuneCreateNSString(userEmail)];
    }
    
    void TuneSetUserId(const char* userId)
    {
        NSLog(@"Native: setUserId: %s", userId);
        
        [Tune setUserId:TuneCreateNSString(userId)];
    }
    
    void TuneSetUserName(const char* userName)
    {
        NSLog(@"Native: setUserName: %s", userName);
        
        [Tune setUserName:TuneCreateNSString(userName)];
    }
    
    void TuneSetFacebookUserId(const char* userId)
    {
        NSLog(@"Native: setFacebookUserId: %s", userId);
        
        [Tune setFacebookUserId:TuneCreateNSString(userId)];
    }
    
    void TuneSetTwitterUserId(const char* userId)
    {
        NSLog(@"Native: setTwitterUserId: %s", userId);
        
        [Tune setTwitterUserId:TuneCreateNSString(userId)];
    }
    
    void TuneSetGoogleUserId(const char* userId)
    {
        NSLog(@"Native: setGoogleUserId: %s", userId);
        
        [Tune setGoogleUserId:TuneCreateNSString(userId)];
    }
    
    void TuneSetAppleAdvertisingIdentifier(const char* appleAdvertisingId, bool trackingEnabled)
    {
        NSLog(@"Native: setAppleAdvertisingIdentifier: %s advertisingTrackingEnabled:%d", appleAdvertisingId, trackingEnabled);
        
        [Tune setAppleAdvertisingIdentifier:[[NSUUID alloc] initWithUUIDString:TuneCreateNSString(appleAdvertisingId)] advertisingTrackingEnabled:trackingEnabled];
    }
    
    void TuneSetAppleVendorIdentifier(const char* appleVendorId)
    {
        NSLog(@"Native: setAppleVendorIdentifier: %s", appleVendorId);
        
        [Tune setAppleVendorIdentifier:[[NSUUID alloc] initWithUUIDString:TuneCreateNSString(appleVendorId)]];
    }
    
    void TuneSetAge(int age)
    {
        NSLog(@"Native: setAge = %d", age);
        
        [Tune setAge:age];
    }
    
    void TuneSetCurrencyCode(const char* currencyCode)
    {
        NSLog(@"Native: setCurrencyCode = %s", currencyCode);
        
        [Tune setCurrencyCode:TuneCreateNSString(currencyCode)];
    }
    
    void TuneSetGender(TuneGender gender)
    {
        NSLog(@"Native: setGender = %zd", gender);
        
        [Tune setGender:gender];
    }
    
    void TuneSetLocation(double latitude, double longitude, double altitude)
    {
        NSLog(@"Native: setLocation: %f, %f, %f", latitude, longitude, altitude);
        
        TuneLocation *loc = [TuneLocation new];
        loc.latitude = @(latitude);
        loc.longitude = @(longitude);
        loc.altitude = @(altitude);
        [Tune setLocation:loc];
    }
    
    void TuneSetDeepLink(const char* deepLinkUrl)
    {
        NSLog(@"Native: setDeepLink: %s", deepLinkUrl);
        
        [Tune applicationDidOpenURL:TuneCreateNSString(deepLinkUrl) sourceApplication:nil];
    }
    
    
#pragma mark - Measure Session
    
    void TuneMeasureSession()
    {
        NSLog(@"Native: measureSession");
        
        [Tune measureSession];
    }
    
    
#pragma mark - Measure Event Methods
    
    void TuneMeasureEventName(const char* eventName)
    {
        NSLog(@"Native: measureEventName");
        
        [Tune measureEventName:TuneCreateNSString(eventName)];
    }
    
    void TuneMeasureEventId(int eventId)
    {
        NSLog(@"Native: measureEventId");
        
        [Tune measureEventId:eventId];
    }
    
    void TuneMeasureEvent(TuneEventIos event, TuneItemIos eventItems[], int eventItemCount, Byte receipt[], int receiptByteCount)
    {
        NSLog(@"Native: measureEvent");
        
        TuneEvent *tuneEvent = convertIosEvent(event, eventItems, eventItemCount, receipt, receiptByteCount);
        [Tune measureEvent:tuneEvent];
    }

#pragma mark - In-App Marketing Methods

    // Custom Profile API
    
    void TuneRegisterCustomProfileString(const char* variableName)
    {
        NSLog(@"Native: registerCustomProfileString");
        [Tune registerCustomProfileString:TuneCreateNSString(variableName)];
    }

    void TuneRegisterCustomProfileStringWithDefault(const char* variableName, const char* defaultValue)
    {
        NSLog(@"Native: registerCustomProfileString");
        [Tune registerCustomProfileString:TuneCreateNSString(variableName) withDefault:TuneCreateNSString(defaultValue)];
    }

    void TuneRegisterCustomProfileDate(const char* variableName)
    {
        NSLog(@"Native: registerCustomProfileDate");
        [Tune registerCustomProfileDateTime:TuneCreateNSString(variableName)];
    }

    void TuneRegisterCustomProfileDateWithDefault(const char* variableName, const char* defaultValue)
    {
        NSLog(@"Native: registerCustomProfileDate");
        // convert millis string to NSTimeInterval
        NSTimeInterval ti = [TuneCreateNSString(defaultValue) doubleValue] / 1000.0f;
        // convert NSTimeInterval to NSDate
        NSDate *defaultDate = [NSDate dateWithTimeIntervalSince1970:ti];
        [Tune registerCustomProfileDateTime:TuneCreateNSString(variableName) withDefault:defaultDate];
    }

    void TuneRegisterCustomProfileNumber(const char* variableName)
    {
        NSLog(@"Native: registerCustomProfileNumber");
        [Tune registerCustomProfileNumber:TuneCreateNSString(variableName)];
    }

    void TuneRegisterCustomProfileNumberWithDefaultInt(const char* variableName, int defaultValue)
    {
        NSLog(@"Native: registerCustomProfileNumber");
        [Tune registerCustomProfileNumber:TuneCreateNSString(variableName) withDefault:[NSNumber numberWithInteger:defaultValue]];
    }

    void TuneRegisterCustomProfileNumberWithDefaultDouble(const char* variableName, double defaultValue)
    {
        NSLog(@"Native: registerCustomProfileNumber");
        [Tune registerCustomProfileNumber:TuneCreateNSString(variableName) withDefault:[NSNumber numberWithDouble:defaultValue]];
    }

    void TuneRegisterCustomProfileNumberWithDefaultFloat(const char* variableName, float defaultValue)
    {
        NSLog(@"Native: registerCustomProfileNumber");
        [Tune registerCustomProfileNumber:TuneCreateNSString(variableName) withDefault:[NSNumber numberWithFloat:defaultValue]];
    }

    void TuneRegisterCustomProfileGeolocation(const char* variableName)
    {
        NSLog(@"Native: registerCustomProfileGeolocation");
        [Tune registerCustomProfileGeolocation:TuneCreateNSString(variableName)];
    }

    void TuneRegisterCustomProfileGeolocationWithDefault(const char* variableName, double defaultLongitude, double defaultLatitude)
    {
        NSLog(@"Native: registerCustomProfileGeolocation");
        TuneLocation *loc = [TuneLocation new];
        loc.longitude = [NSNumber numberWithDouble:defaultLongitude];
        loc.latitude = [NSNumber numberWithDouble:defaultLatitude];
        [Tune registerCustomProfileGeolocation:TuneCreateNSString(variableName) withDefault:loc];
    }

    void TuneSetCustomProfileString(const char* variableName, const char* value)
    {
        NSLog(@"Native: setCustomProfileStringValue");
        [Tune setCustomProfileStringValue:TuneCreateNSString(value) forVariable:TuneCreateNSString(variableName)];
    }

    void TuneSetCustomProfileDate(const char* variableName, const char* value)
    {
        NSLog(@"Native: setCustomProfileDateTimeValue");
        // convert millis string to NSTimeInterval
        NSTimeInterval ti = [TuneCreateNSString(value) doubleValue] / 1000.0f;
        // convert NSTimeInterval to NSDate
        NSDate *date = [NSDate dateWithTimeIntervalSince1970:ti];
        [Tune setCustomProfileDateTimeValue:date forVariable:TuneCreateNSString(variableName)];
    }

    void TuneSetCustomProfileNumberWithInt(const char* variableName, int value)
    {
        NSLog(@"Native: setCustomProfileNumberValue");
        [Tune setCustomProfileNumberValue:[NSNumber numberWithInt:value] forVariable:TuneCreateNSString(variableName)];
    }

    void TuneSetCustomProfileNumberWithDouble(const char* variableName, double value)
    {
        NSLog(@"Native: setCustomProfileNumberValue");
        [Tune setCustomProfileNumberValue:[NSNumber numberWithDouble:value] forVariable:TuneCreateNSString(variableName)];
    }

    void TuneSetCustomProfileNumberWithFloat(const char* variableName, float value)
    {
        NSLog(@"Native: setCustomProfileNumberValue");
        [Tune setCustomProfileNumberValue:[NSNumber numberWithFloat:value] forVariable:TuneCreateNSString(variableName)];
    }

    void TuneSetCustomProfileGeolocation(const char* variableName, double longitude, double latitude)
    {
        NSLog(@"Native: setCustomProfileGeolocationValue");
        TuneLocation *loc = [TuneLocation new];
        loc.longitude = [NSNumber numberWithDouble:longitude];
        loc.latitude = [NSNumber numberWithDouble:latitude];
        [Tune setCustomProfileGeolocationValue:loc forVariable:TuneCreateNSString(variableName)];
    }

    const char* TuneGetCustomProfileString(const char* variableName)
    {
        NSLog(@"Native: getCustomProfileString");
        return TuneAutonomousStringCopy([[Tune getCustomProfileString:TuneCreateNSString(variableName)] UTF8String]);
    }

    const char* TuneGetCustomProfileDate(const char* variableName)
    {
        NSLog(@"Native: getCustomProfileDateTime");
        NSDate *customDate = [Tune getCustomProfileDateTime:TuneCreateNSString(variableName)];
        NSTimeInterval ti = [customDate timeIntervalSince1970];
        double milliseconds = ti * 1000;
        return TuneAutonomousStringCopy([[[NSNumber numberWithDouble:milliseconds] stringValue] UTF8String]);
    }

    const char* TuneGetCustomProfileNumber(const char* variableName)
    {
        NSLog(@"Native: getCustomProfileNumber");
        NSNumber *customNumber = [Tune getCustomProfileNumber:TuneCreateNSString(variableName)];
        return TuneAutonomousStringCopy([[customNumber stringValue] UTF8String]);
    }

    const char* TuneGetCustomProfileGeolocation(const char* variableName)
    {
        NSLog(@"Native: getCustomProfileGeolocation");
        TuneLocation *customGeo = [Tune getCustomProfileGeolocation:TuneCreateNSString(variableName)];
        NSMutableString *customGeoStr = [[customGeo.latitude stringValue] mutableCopy];
        [customGeoStr appendString:@","];
        [customGeoStr appendString:[customGeo.longitude stringValue]];
        return TuneAutonomousStringCopy([customGeoStr UTF8String]);
    }

    void TuneClearCustomProfileVariable(const char* variableName)
    {
        NSLog(@"Native: clearCustomProfileVariable");
        [Tune clearCustomProfileVariable:TuneCreateNSString(variableName)];
    }

    void TuneClearAllCustomProfileVariables()
    {
        NSLog(@"Native: clearAllCustomProfileVariables");
        [Tune clearAllCustomProfileVariables];
    }

    // Power Hook API

    void TuneRegisterHookWithId(const char* hookId, const char* friendlyName, const char* defaultValue)
    {
        NSLog(@"Native: registerHookWithId");
        [Tune registerHookWithId:TuneCreateNSString(hookId) friendlyName:TuneCreateNSString(friendlyName) defaultValue:TuneCreateNSString(defaultValue)];
    }

    const char* TuneGetValueForHookById(const char* hookId)
    {
        NSLog(@"Native: getValueForHookById");
        NSString *hookValue = [Tune getValueForHookById:TuneCreateNSString(hookId)];
        char *strHookValue = TuneAutonomousStringCopy([hookValue UTF8String]);
        return strHookValue;
    }

    void TuneSetValueForHookById(const char* hookId, const char* value)
    {
        NSLog(@"Native: setValueForHookById");
        [Tune setValueForHookById:TuneCreateNSString(hookId) value:TuneCreateNSString(value)];
    }

    void TuneOnPowerHooksChanged(bool listenForPowerHooksChanged)
    {
        NSLog(@"Native: onPowerhooksChanged");
        if (listenForPowerHooksChanged)
        {
            // Send message to TuneListener Unity GameObject in block
            [Tune onPowerHooksChanged:[powerhooksChangedBlock copy]];
        }
    }

    // Experiment Details API

    const char* TuneGetPowerHookVariableExperimentDetails()
    {
        NSLog(@"Native: getPowerHookVariableExperimentDetails");
        return TuneSerializePowerHookOrExperimentDetails([Tune getPowerHookVariableExperimentDetails]);
    }

    const char* TuneGetInAppMessageExperimentDetails()
    {
        NSLog(@"Native: getInAppMessageExperimentDetails");
        return TuneSerializePowerHookOrExperimentDetails([Tune getInAppMessageExperimentDetails]);
    }

    // Playlist API

    void TuneOnFirstPlaylistDownloaded(bool listenForFirstPlaylist)
    {
        if (listenForFirstPlaylist)
        {
            [Tune onFirstPlaylistDownloaded:[firstPlaylistBlock copy]];
        }
    }

    void TuneOnFirstPlaylistDownloadedWithTimeout(bool listenForFirstPlaylist, long timeout)
    {
        if (listenForFirstPlaylist)
        {
            [Tune onFirstPlaylistDownloaded:[firstPlaylistBlock copy] withTimeout:timeout];
        }
    }
}
