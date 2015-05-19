#import <Foundation/Foundation.h>
#import <string>
#import <map>

#import "MobileAppTracker.h"
#import "AppDelegateListener.h"

typedef struct MATItemIos
{
    const char* name;
    double      unitPrice;
    int         quantity;
    double      revenue;
    const char* attribute1;
    const char* attribute2;
    const char* attribute3;
    const char* attribute4;
    const char* attribute5;
} MATItemIos;

typedef struct MATEventIos
{
    const char* name;
    const char* eventId;
    const char* revenue;
    const char* currencyCode;
    const char* advertiserRefId;
    const char* transactionState;
    const char* contentType;
    const char* contentId;
    const char* level;
    const char* quantity;
    const char* searchString;
    const char* rating;
    const char* date1;
    const char* date2;
    const char* attribute1;
    const char* attribute2;
    const char* attribute3;
    const char* attribute4;
    const char* attribute5;
} MATEventIos;

typedef struct MATPreloadDataIos
{
    const char* advertiserSubAd;
    const char* advertiserSubAdgroup;
    const char* advertiserSubCampaign;
    const char* advertiserSubKeyword;
    const char* advertiserSubPublisher;
    const char* advertiserSubSite;
    const char* agencyId;
    const char* offerId;
    const char* publisherId;
    const char* publisherReferenceId;
    const char* publisherSub1;
    const char* publisherSub2;
    const char* publisherSub3;
    const char* publisherSub4;
    const char* publisherSub5;
    const char* publisherSubAd;
    const char* publisherSubAdgroup;
    const char* publisherSubCampaign;
    const char* publisherSubKeyword;
    const char* publisherSubPublisher;
    const char* publisherSubSite;
} MATPreloadDataIos;