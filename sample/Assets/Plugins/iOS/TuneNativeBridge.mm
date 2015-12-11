#import "TuneNativeBridge.h"
#import "TuneNativeBanner.h"
#import "TuneNativeInterstitial.h"
#import "TuneObjectCache.h"
#import "TuneNativeMetadata.h"

extern UIViewController *UnityGetGLViewController(); // Root view controller of Unity screen.

// corresponds to GameObject named "MobileAppTracker" in the Unity Project
const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_RECEIVER = "MobileAppTracker";

// corresponds to the Tune callback methods defined in the script attached to the above GameObject
const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_SUCCEEDED  = "trackerDidSucceed";
const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_FAILED  = "trackerDidFail";
const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_ENQUEUED = "trackerDidEnqueueRequest";
const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_DEEPLINK_RECEIVED = "trackerDidReceiveDeeplink";
const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_DEEPLINK_FAILED = "trackerDidFailDeeplink";

const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_ON_AD_LOAD = "onAdLoad";
const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_ON_AD_LOAD_FAILED = "onAdLoadFailed";
//const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_ON_AD_CLICK = "onAdClick";
const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_ON_AD_CLOSED = "onAdClosed";
const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_ON_AD_ACTION_START = "onAdActionStart";
const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_ON_AD_ACTION_END = "onAdActionEnd";
const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_ON_AD_REQUEST_FIRED = "onAdRequestFired";


TuneNativeInterstitial *interstitial;
TuneNativeBanner *banner;


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


@interface TuneAdSdkDelegate : NSObject <TuneAdDelegate>
// empty
@end

@implementation TuneAdSdkDelegate


#pragma mark - TuneAdSdkDelegate Methods

- (void)tuneAdDidFetchAdForView:(TuneAdView *)adView placement:(NSString *)placement
{
    NSLog(@"Native: tuneAdDidFetchAdForView: placement = %@", placement);
    
    UnitySendMessage(UNITY_SENDMESSAGE_CALLBACK_TUNE_RECEIVER, UNITY_SENDMESSAGE_CALLBACK_TUNE_ON_AD_LOAD, nil != placement ? [placement UTF8String] : "");
}

- (void)tuneAdDidStartActionForView:(TuneAdView *)adView willLeaveApplication:(BOOL)willLeave
{
    NSLog(@"Native: tuneAdDidStartActionForView: willLeave = %d", willLeave);
    
    UnitySendMessage(UNITY_SENDMESSAGE_CALLBACK_TUNE_RECEIVER, UNITY_SENDMESSAGE_CALLBACK_TUNE_ON_AD_ACTION_START, [[@(willLeave) stringValue] UTF8String]);
}

- (void)tuneAdDidEndActionForView:(TuneAdView *)adView
{
    NSLog(@"Native: tuneAdDidEndActionForView");
    
    UnitySendMessage(UNITY_SENDMESSAGE_CALLBACK_TUNE_RECEIVER, UNITY_SENDMESSAGE_CALLBACK_TUNE_ON_AD_ACTION_END, "");
}

- (void)tuneAdDidCloseForView:(TuneAdView *)adView
{
    NSLog(@"Native: tuneAdDidCloseForView");
    
    UnitySendMessage(UNITY_SENDMESSAGE_CALLBACK_TUNE_RECEIVER, UNITY_SENDMESSAGE_CALLBACK_TUNE_ON_AD_CLOSED, "");
}

- (void)tuneAdDidFailWithError:(NSError *)error forView:(TuneAdView *)adView
{
    NSLog(@"Native: tuneAdDidFailWithError: error = %@", error);
    
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
    
    UnitySendMessage(UNITY_SENDMESSAGE_CALLBACK_TUNE_RECEIVER, UNITY_SENDMESSAGE_CALLBACK_TUNE_ON_AD_LOAD_FAILED, [strError UTF8String]);
}

- (void)tuneAdDidFireRequestWithUrl:(NSString *)url data:(NSString *)data forView:(TuneAdView *)adView
{
    NSLog(@"Native: tuneAdDidFireRequestWithUrl");
    
    NSString *strOutput = [NSString stringWithFormat:@"%@%@", url, data];
    
    UnitySendMessage(UNITY_SENDMESSAGE_CALLBACK_TUNE_RECEIVER, UNITY_SENDMESSAGE_CALLBACK_TUNE_ON_AD_REQUEST_FIRED, [strOutput UTF8String]);
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

TuneSdkDelegate *matDelegate;
TuneSdkDelegate *matDeeplinkDelegate;

TuneAdSdkDelegate *interstitialAdDelegate;
TuneAdSdkDelegate *bannerAdDelegate;


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
        
        matDeeplinkDelegate = matDeeplinkDelegate ?: [TuneSdkDelegate new];
        
        [Tune checkForDeferredDeeplink:matDeeplinkDelegate];
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
        matDelegate = enable ? (matDelegate ? nil : [TuneSdkDelegate new]) : nil;
        
        [Tune setDelegate:matDelegate];
    }
    
    void TuneSetAllowDuplicates(bool allowDuplicateRequests)
    {
        NSLog(@"Native: setAllowDuplicates = %d", allowDuplicateRequests);
        
        [Tune setAllowDuplicateRequests:allowDuplicateRequests];
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
    
    void TuneSetSiteId(const char* siteId)
    {
        NSLog(@"Native: setSiteId: %s", siteId);
        
        [Tune setSiteId:TuneCreateNSString(siteId)];
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
        
        [Tune setAppleAdvertisingIdentifier:[[[NSUUID alloc] initWithUUIDString:TuneCreateNSString(appleAdvertisingId)] autorelease] advertisingTrackingEnabled:trackingEnabled];
    }
    
    void TuneSetAppleVendorIdentifier(const char* appleVendorId)
    {
        NSLog(@"Native: setAppleVendorIdentifier: %s", appleVendorId);
        
        [Tune setAppleVendorIdentifier:[[[NSUUID alloc] initWithUUIDString:TuneCreateNSString(appleVendorId)] autorelease]];
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
    
    
#pragma mark - Ad Methods
    
    /// Creates an empty TuneAdMetadata and returns its reference.
    TuneTypeMetadataRef TuneCreateMetadata() {
        TuneNativeMetadata *metadata = [[TuneNativeMetadata new] autorelease];
        TuneObjectCache *cache = [TuneObjectCache sharedInstance];
        [cache.references setObject:metadata forKey:[metadata Tune_referenceKey]];
        return metadata;
    }
    
    /// Adds a keyword to the TuneAdMetadata.
    void TuneAddKeyword(TuneTypeMetadataRef metadata, const char *keyword) {
        TuneNativeMetadata *internalMetadata = (TuneNativeMetadata *)metadata;
        [internalMetadata addKeyword:TuneCreateNSString(keyword)];
    }
    
    /// Sets the user's birthDate on the TuneAdMetadata.
    void TuneSetBirthDate(TuneTypeMetadataRef metadata, NSInteger year, NSInteger month, NSInteger day) {
        TuneNativeMetadata *internalMetadata = (TuneNativeMetadata *)metadata;
        [internalMetadata setBirthDateWithMonth:month day:day year:year];
    }
    
    /// Sets the user's gender on the TuneAdMetadata.
    void TuneAdSetGender(TuneTypeMetadataRef metadata, NSInteger genderCode) {
        TuneNativeMetadata *internalMetadata = (TuneNativeMetadata *)metadata;
        [internalMetadata setGenderWithCode:(TuneAdGender)genderCode];
    }
    
    /// Sets the debugMode on the TuneAdMetadata.
    void TuneAdSetDebugMode(TuneTypeMetadataRef metadata, BOOL debugMode) {
        TuneNativeMetadata *internalMetadata = (TuneNativeMetadata *)metadata;
        [internalMetadata setDebugMode:debugMode];
    }
    
    void TuneSetLatitude(TuneTypeMetadataRef metadata, double latitude) {
        TuneNativeMetadata *internalMetadata = (TuneNativeMetadata *)metadata;
        [internalMetadata setLatitude:latitude];
    }
    
    void TuneSetLongitude(TuneTypeMetadataRef metadata, double longitude) {
        TuneNativeMetadata *internalMetadata = (TuneNativeMetadata *)metadata;
        [internalMetadata setLongitude:longitude];
    }
    
    void TuneSetAltitude(TuneTypeMetadataRef metadata, double altitude) {
        TuneNativeMetadata *internalMetadata = (TuneNativeMetadata *)metadata;
        [internalMetadata setAltitude:altitude];
    }
    
    /// Sets an extra parameter to be included in the ad metadata.
    void TuneSetCustomTargets(TuneTypeMetadataRef metadata, const char *key, const char *value) {
        TuneNativeMetadata *internalMetadata = (TuneNativeMetadata *)metadata;
        [internalMetadata setCustomTargetWithKey:TuneCreateNSString(key)
                                           value:TuneCreateNSString(value)];
    }
    
    /// Removes an object from the cache.
    void TuneRelease(TuneTypeRef ref) {
        if (ref) {
            TuneObjectCache *cache = [TuneObjectCache sharedInstance];
            [cache.references removeObjectForKey:[(NSObject *)ref Tune_referenceKey]];
        }
    }
    
    
#pragma mark - Interstitial Ad Methods
    
    void TuneCreateInterstitial()
    {
        if(!interstitial)
        {
            interstitialAdDelegate = interstitialAdDelegate ?: [TuneAdSdkDelegate new];
            
            interstitial = [[[TuneNativeInterstitial alloc] initWithAdDelegate:interstitialAdDelegate] autorelease];
            TuneObjectCache *cache = [TuneObjectCache sharedInstance];
            [cache.references setObject:interstitial forKey:[interstitial Tune_referenceKey]];
        }
    }
    
    void TuneCacheInterstitialWithMetadata(const char *placement, TuneTypeMetadataRef metadata)
    {
        NSLog(@"Native: cacheInterstitialMetadata");
        
        TuneNativeMetadata *internalMetadata = (TuneNativeMetadata *)metadata;
        TuneAdMetadata *adMetadata = [internalMetadata metadata];
        
        TuneCreateInterstitial();
        
        [interstitial cacheForPlacement:[NSString stringWithUTF8String:placement] adMetadata:adMetadata];
    }
    
    void TuneCacheInterstitial(const char *placement)
    {
        NSLog(@"Native: cacheInterstitial");
        
        TuneCacheInterstitialWithMetadata(placement, NULL);
    }
    
    void TuneShowInterstitialWithMetadata(const char *placement, TuneTypeMetadataRef metadata)
    {
        NSLog(@"Native: showInterstitialWithMetadata");
        
        TuneNativeMetadata *internalMetadata = (TuneNativeMetadata *)metadata;
        TuneAdMetadata *adMetadata = [internalMetadata metadata];
        
        TuneCreateInterstitial();
        
        [interstitial showForPlacement:[NSString stringWithUTF8String:placement] adMetadata:adMetadata];
    }
    
    void TuneShowInterstitial(const char *placement)
    {
        NSLog(@"Native: showInterstitial");
        
        TuneShowInterstitialWithMetadata(placement, NULL);
    }
    
    void TuneDestroyInterstitial()
    {
        NSLog(@"Native: destroyInterstitial");
        
        [interstitial destroy];
        
        TuneRelease(interstitial);
        interstitial = nil;
        interstitialAdDelegate = nil;
    }
    
    void TuneLayoutInterstitial()
    {
        //NSLog(@"Native: layoutInterstitial");
        
        [interstitial layout];
    }
    
    
#pragma mark - Banner Ad Methods
    
    void TuneCreateBanner(TuneAdPosition adPosition)
    {
        if(!banner)
        {
            bannerAdDelegate = bannerAdDelegate ?: [TuneAdSdkDelegate new];
            
            banner = [[[TuneNativeBanner alloc] initWithAdDelegate:bannerAdDelegate adPosition:adPosition] autorelease];
            TuneObjectCache *cache = [TuneObjectCache sharedInstance];
            [cache.references setObject:banner forKey:[banner Tune_referenceKey]];
        }
    }
    
    void TuneShowBannerWithPosition(const char *placement, TuneTypeMetadataRef metadata, TuneAdPosition position)
    {
        NSLog(@"Native: showBannerWithPosition");
        
        TuneNativeMetadata *internalMetadata = (TuneNativeMetadata *)metadata;
        TuneAdMetadata *adMetadata = [internalMetadata metadata];
        
        TuneCreateBanner(position);
        banner.adPosition = position;
        
        [banner showForPlacement:[NSString stringWithUTF8String:placement] adMetadata:adMetadata];
    }
    
    void TuneShowBannerWithMetadata(const char *placement, TuneTypeMetadataRef metadata)
    {
        NSLog(@"Native: showBannerWithMetadata");
        
        TuneShowBannerWithPosition(placement, metadata, kTuneAdPositionBottomOfScreen);
    }
    
    void TuneShowBanner(const char *placement)
    {
        NSLog(@"Native: showBanner");
        
        TuneShowBannerWithMetadata(placement, NULL);
    }
    
    void TuneHideBanner()
    {
        NSLog(@"Native: hideBanner");
        
        [banner hide];
    }
    
    void TuneDestroyBanner()
    {
        NSLog(@"Native: destroyBanner");
        
        [banner destroy];
        
        TuneRelease(banner);
        banner = nil;
        bannerAdDelegate = nil;
    }
    
    void TuneLayoutBanner()
    {
        //NSLog(@"Native: layoutBanner");
        
        [banner layout];
    }
}
