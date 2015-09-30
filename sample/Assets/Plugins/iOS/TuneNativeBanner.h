// Copyright 2014 Tune Inc. All Rights Reserved.

#import <CoreGraphics/CoreGraphics.h>
#import <Foundation/Foundation.h>

#import "TuneTypes.h"

@class TuneAdView;
@class TuneBanner;
@class TuneAdMetadata;

@protocol TuneAdDelegate;

/// Positions to place a banner.
typedef NS_ENUM(NSUInteger, TuneAdPosition) {
    kTuneAdPositionBottomOfScreen = 0,      ///< Ad positioned at top of screen.
    kTuneAdPositionTopOfScreen = 1  ///< Ad positioned at bottom of screen.
};


/// A wrapper around TuneAdView. Includes the ability to create TuneAdView objects, load them
/// with ads, and listen for ad events.
@interface TuneNativeBanner : NSObject

/// A TuneBanner which contains the ad.
@property (nonatomic, retain) TuneBanner *bannerView;

/// Defines where the ad should be positioned on the screen.
@property (nonatomic, assign) TuneAdPosition adPosition;


/// Initializes a TuneBanner
- (id)initWithAdDelegate:(id<TuneAdDelegate>)adDelegate adPosition:(TuneAdPosition)position;

/// Shows an ad banner for the given placement using the additional targeting options supplied through metadata.
- (void)showForPlacement:(NSString *)placement
              adMetadata:(TuneAdMetadata *)metadata;

/// Makes the TuneAdView hidden on the screen.
- (void)hide;

/// Destory the TuneAdView.
- (void)destroy;

/// Returns YES if the banner is ready to be displayed.
- (BOOL)isReady;

/// Layout the banner view for the current orientation
- (void)layout;

@end
