// Copyright 2014 Tune Inc. All Rights Reserved.

#import <Foundation/Foundation.h>

#import "TuneTypes.h"

@class TuneAdView;
@class TuneInterstitial;
@class TuneAdMetadata;

@protocol TuneAdDelegate;


/// A wrapper around TuneAdView. Includes the ability to create TuneAdView interstitial objects, load
/// them with ads, show them, and listen for ad events.
@interface TuneNativeInterstitial : NSObject

/// The interstitial ad.
@property (nonatomic, retain) TuneInterstitial *interstitial;


/// Initializes a TuneInterstitial.
- (id)initWithAdDelegate:(id<TuneAdDelegate>)adDelegate;

/// Caches an ad for the given placement.
- (void)cacheForPlacement:(NSString *)placement;

/// Caches an ad for the given placement and additional targeting options supplied through object.
- (void)cacheForPlacement:(NSString *)placement adMetadata:(TuneAdMetadata *)metadata;

/// Shows an interstitial for the given placement.
- (void)showForPlacement:(NSString *)placement;

/// Shows an ad for the given placement and additional targeting options supplied through object.
- (void)showForPlacement:(NSString *)placement adMetadata:(TuneAdMetadata *)metadata;

/// Returns YES if the interstitial is ready to be displayed.
- (BOOL)isReady;

/// Layout the interstitial view for the current orientation.
- (void)layout;

/// Destroy the interstitial view to free up memory.
- (void)destroy;

@end
