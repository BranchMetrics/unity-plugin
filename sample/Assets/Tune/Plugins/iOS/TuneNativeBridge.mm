#import "TuneNativeBridge.h"

extern UIViewController *UnityGetGLViewController(); // Root view controller of Unity screen.

// corresponds to GameObject named "TuneListener" in the Unity Project
const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_RECEIVER = "TuneListener";

// corresponds to the Tune callback methods defined in the script attached to the above GameObject
const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_DEEPLINK_RECEIVED = "DidReceiveDeeplink";
const char * UNITY_SENDMESSAGE_CALLBACK_TUNE_DEEPLINK_FAILED = "DidFailDeeplink";

#pragma mark - Tune Plugin Helper Category

@interface Tune (TuneUnityPlugin)

+ (void)setPluginName:(NSString *)pluginName;

@end

@interface TuneSdkDelegate : NSObject<TuneDelegate>

@end

@implementation TuneSdkDelegate

#pragma mark - TuneSdkDelegate Methods

- (void)tuneDidReceiveDeeplink:(NSString *)deeplink {
    UnitySendMessage(UNITY_SENDMESSAGE_CALLBACK_TUNE_RECEIVER, UNITY_SENDMESSAGE_CALLBACK_TUNE_DEEPLINK_RECEIVED, nil != deeplink ? [deeplink UTF8String] : "");
}

- (void)tuneDidFailDeeplinkWithError:(NSError *)error {
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

@end


@interface TuneAppDelegateListener : NSObject<AppDelegateListener>

@end

@implementation TuneAppDelegateListener

#pragma mark - TuneAppDelegateListener Methods

+ (TuneAppDelegateListener *)sharedInstance {
    static TuneAppDelegateListener *listener;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        listener = [TuneAppDelegateListener new];
    });
    return listener;
}

- (instancetype)init {
    self = [super init];
    if (!self) {
        UnityRegisterAppDelegateListener(self);
    }

    return self;
}


#pragma mark - Unity AppDelegateListener Callback Methods

- (void)onOpenURL:(NSNotification*)notification {

    NSURL *url = [notification.userInfo objectForKey:@"url"];
    NSString *strSource = [notification.userInfo objectForKey:@"sourceApplication"];

    [Tune handleOpenURL:url sourceApplication:strSource];
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

    if(event.name)
    {
        evt = [TuneEvent eventWithName:TuneCreateNSString(event.name)];

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

TuneSdkDelegate *tuneDeeplinkDelegate;


// When native code plugin is implemented in .mm / .cpp file, then functions
// should be surrounded with extern "C" block to conform to C function naming rules.
extern "C" {


#pragma mark - Init Method

    void TuneInit (const char* advertiserId, const char* conversionKey)
    {
        [Tune initializeWithTuneAdvertiserId:TuneCreateNSString(advertiserId)
                           tuneConversionKey:TuneCreateNSString(conversionKey)];
        [Tune setPluginName:@"unity"];
    }

    void TuneInitWithPackageName (const char* advertiserId, const char* conversionKey, const char* packageName)
    {
        [Tune initializeWithTuneAdvertiserId:TuneCreateNSString(advertiserId)
                           tuneConversionKey:TuneCreateNSString(conversionKey)
                             tunePackageName:TuneCreateNSString(packageName)];
        [Tune setPluginName:@"unity"];
    }


#pragma mark - Behavior Flags

    void TuneAutomateIapEventMeasurement(bool automate)
    {
        [Tune automateInAppPurchaseEventMeasurement:automate];
    }

    void TuneSetFacebookEventLogging(bool enable, bool limitEventAndDataUsage)
    {
        [Tune setFacebookEventLogging:enable limitEventAndDataUsage:limitEventAndDataUsage];
    }


#pragma mark - Getter Methods

    const char* TuneGetTuneId()
    {
        NSString *tuneId = [Tune tuneId];
        char *strTuneId = TuneAutonomousStringCopy([tuneId UTF8String]);

        return strTuneId;
    }

    bool TuneIsPayingUser()
    {
        return [Tune isPayingUser];
    }

    const char* TuneGetOpenLogId()
    {
        NSString *openLogId = [Tune openLogId];
        char *strOpenLogId = TuneAutonomousStringCopy([openLogId UTF8String]);

        return strOpenLogId;
    }


#pragma mark - Setter Methods

    void TuneSetExistingUser(bool isExisting)
    {
        [Tune setExistingUser:isExisting];
    }

    void TuneSetPayingUser(bool isPaying)
    {
        [Tune setPayingUser:isPaying];
    }

    void TuneSetPreloadData(TunePreloadDataIos preloadData)
    {
        TunePreloadData *matPreloadData = convertIosPreloadData(preloadData);
        [Tune setPreloadedAppData:matPreloadData];
    }

    void TuneSetJailbroken(bool isJailbroken)
    {
        [Tune setJailbroken:isJailbroken];
    }

    void TuneSetDebugMode(bool enable)
    {
        if (enable) {
            [Tune setDebugLogVerbose:YES];
            [Tune setDebugLogCallback:^(NSString * _Nonnull logMessage) {
                NSLog(@"Tune Debug: %@", logMessage);
            }];
        } else {
            [Tune setDebugLogVerbose:NO];
            [Tune setDebugLogCallback:nil];
        }
    }
    
    void TuneSetAppAdTracking(bool enable)
    {
        [Tune setAppAdTrackingEnabled:enable];
    }

#pragma mark - Measure Session

    void TuneMeasureSession()
    {
        [Tune measureSession];
    }


#pragma mark - Measure Event Methods

    void TuneMeasureEventName(const char* eventName)
    {
        [Tune measureEventName:TuneCreateNSString(eventName)];
    }

    void TuneMeasureEvent(TuneEventIos tuneEvent, TuneItemIos eventItems[], int eventItemCount, Byte receipt[], int receiptByteCount)
    {
        TuneEvent *event = convertIosEvent(tuneEvent, eventItems, eventItemCount, receipt, receiptByteCount);
        [Tune measureEvent:event];
    }

#pragma mark - Deeplinking

    void TuneRegisterDeeplinkListener()
    {
        tuneDeeplinkDelegate = tuneDeeplinkDelegate ?: [TuneSdkDelegate new];

        [Tune registerDeeplinkListener:tuneDeeplinkDelegate];
    }

    void TuneUnregisterDeeplinkListener()
    {
        [Tune unregisterDeeplinkListener];
    }

    bool TuneIsTuneLink(const char* linkUrl)
    {
        return [Tune isTuneLink:TuneCreateNSString(linkUrl)];
    }

    void TuneRegisterCustomTuneLinkDomain(const char* domain)
    {
        [Tune registerCustomTuneLinkDomain:TuneCreateNSString(domain)];
    }

#pragma mark - COPPA and GDPR

    void TuneSetPrivacyProtectedDueToAge(bool isPrivacyProtected)
    {
        [Tune setPrivacyProtectedDueToAge:isPrivacyProtected];
    }

    bool TuneIsPrivacyProtectedDueToAge()
    {
        return [Tune isPrivacyProtectedDueToAge];
    }
}
