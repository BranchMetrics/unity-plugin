// Copyright 2014 Tune Inc. All Rights Reserved.

#import <Foundation/Foundation.h>

#import "TuneAdMetadata.h"
#import "TuneAdView.h"

/// Genders to help deliver more relevant ads.
typedef NS_ENUM(NSInteger, TuneAdGender) {
    kTuneAdGenderUnknown = 0, /// Unknown
    kTuneAdGenderMale    = 1, /// Male
    kTuneAdGenderFemale  = 2, /// Female
};

// Specifies optional parameters for ad requests.
@interface TuneNativeMetadata : NSObject

/// Returns an initialized TuneMetadata object.
- (id)init;

/// Words or phrase describing the current activity of the user.
@property(nonatomic, retain) NSMutableArray *keywords;

/// The user's birthday may be used to deliver more relevant ads.
@property(nonatomic, retain) NSDate *birthDate;

/// The user's gender may be used to deliver more relevant ads.
@property(nonatomic, assign) TuneGender gender;

/// Location (latitude, logitude, altitude) to be targeted by the ads.
@property(nonatomic, assign) double latitude, longitude, altitude;

/// Ad view debug mode status.
@property(nonatomic, assign) BOOL debugMode;

/// Extra parameters to be sent up in the ad request.
@property(nonatomic, retain) NSMutableDictionary *customTargets;

/// Convenience method for adding a single keyword.
- (void)addKeyword:(NSString *)keyword;

/// Convenience method for setting the user's birthday.
- (void)setBirthDateWithMonth:(NSInteger)month day:(NSInteger)day year:(NSInteger)year;

/// Convenience method for setting the user's birthday with a TuneGender.
- (void)setGenderWithCode:(TuneAdGender)gender;

/// Convenience method for setting an extra parameters.
- (void)setCustomTargetWithKey:(NSString *)key value:(NSString *)value;

/// Constructs a TuneAdMetadata with the defined targeting values.
- (TuneAdMetadata *)metadata;

@end
