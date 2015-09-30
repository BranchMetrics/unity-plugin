// Copyright 2014 Tune Inc. All Rights Reserved.

#import <CoreGraphics/CoreGraphics.h>
#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

#import "TuneNativeInterstitial.h"
#import "TuneInterstitial.h"
#import "TuneAdView.h"
#import "UnityAppController.h"


@interface TuneNativeInterstitial ()

@property(nonatomic, assign) id<TuneAdDelegate> adDelegate;

@end


@implementation TuneNativeInterstitial

+ (UIViewController *)unityGLViewController {
    return ((UnityAppController *)[UIApplication sharedApplication].delegate).rootViewController;
}

- (id)initWithAdDelegate:(id<TuneAdDelegate>)adDelegate
{
    self = [super init];
    if (self) {
        _adDelegate = adDelegate;
        _interstitial = [[TuneInterstitial adViewWithDelegate:_adDelegate] retain];
    }
    return self;
}

- (void)dealloc {
    
    _interstitial.delegate = nil;
    [_interstitial release]; _interstitial = nil;
    [super dealloc];
}

- (void)cacheForPlacement:(NSString *)placement
{
    if (!self.interstitial) {
        NSLog(@"TuneAdsPlugin: interstitial is nil. Ignoring ad caching request.");
        return;
    }
    [self.interstitial cacheForPlacement:placement];
}

- (void)cacheForPlacement:(NSString *)placement adMetadata:(TuneAdMetadata *)metadata
{
    if (!self.interstitial) {
        NSLog(@"TuneAdsPlugin: interstitial is nil. Ignoring ad caching request.");
        return;
    }
    [self.interstitial cacheForPlacement:placement adMetadata:metadata];
}

- (void)showForPlacement:(NSString *)placement
{
    if (!self.interstitial) {
        NSLog(@"TuneAdsPlugin: interstitial is nil. Ignoring ad show request.");
        return;
    }
    
    UIViewController *unityController = [[self class] unityGLViewController];
    
    NSLog(@"show: unityController visible = %d", unityController.isViewLoaded && unityController.view.window);
    
    if(unityController.isViewLoaded && unityController.view.window)
    {
        [self.interstitial showForPlacement:placement viewController:unityController];
    }
}

- (void)showForPlacement:(NSString *)placement adMetadata:(TuneAdMetadata *)metadata
{
    if (!self.interstitial) {
        NSLog(@"TuneAdsPlugin: interstitial is nil. Ignoring ad show request.");
        return;
    }
    UIViewController *unityController = [[self class] unityGLViewController];
    
    NSLog(@"show: unityController visible = %d", unityController.isViewLoaded && unityController.view.window);
    
    if(unityController.isViewLoaded && unityController.view.window)
    {
        [self.interstitial showForPlacement:placement viewController:unityController adMetadata:metadata];
    }
}

- (BOOL)isReady {
    return self.interstitial.isReady;
}

- (void)layout
{
    UIView *unityView = [[[self class] unityGLViewController] view];
    
    self.interstitial.frame = unityView.frame;
}

- (void)destroy {
    if (!self.interstitial) {
        NSLog(@"TuneAdsPlugin: interstitial is nil. Ignoring call to destroy");
        return;
    }
    
    _interstitial.delegate = nil;
    [_interstitial release], _interstitial = nil;
}

@end
