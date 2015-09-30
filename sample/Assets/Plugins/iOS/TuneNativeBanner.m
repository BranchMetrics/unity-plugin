// Copyright 2014 Tune Inc. All Rights Reserved.

#import <CoreGraphics/CoreGraphics.h>
#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

#import "TuneNativeBanner.h"

#import "TuneBanner.h"
#import "TuneAdView.h"
#import "UnityAppController.h"


@interface TuneNativeBanner ()

@property (nonatomic, assign) id<TuneAdDelegate> adDelegate;

@end

BOOL shouldHide = NO;


@implementation TuneNativeBanner

/// Returns the Unity view controller.
+ (UIViewController *)unityGLViewController {
    return ((UnityAppController *)[UIApplication sharedApplication].delegate).rootViewController;
}

- (id)initWithAdDelegate:(id<TuneAdDelegate>)adDelegate adPosition:(TuneAdPosition)position
{
    self = [super init];
    if (self) {
        _adPosition = position;
        _adDelegate = adDelegate;
        _bannerView = [[TuneBanner adViewWithDelegate:_adDelegate] retain];
        
        NSLog(@"new banner = %@", _bannerView);
        
        // hide the banner view to start with
        _bannerView.hidden = YES;
        
        // include the banner in the current view hierarchy
        UIView *unityView = [[[self class] unityGLViewController] view];
        [unityView addSubview:self.bannerView];
    }
    return self;
}

- (void)hide {
    
    shouldHide = YES;
    
    if (!self.bannerView) {
        NSLog(@"TuneAdsPlugin: BannerView is nil. Ignoring call to hide");
        return;
    }
    
    self.bannerView.hidden = YES;
}

- (void)showForPlacement:(NSString *)placement
              adMetadata:(TuneAdMetadata *)metadata {
    
    shouldHide = NO;
    NSLog(@"TuneAdsPlugin: show: isReady = %d", [self isReady]);
    
    [self.bannerView showForPlacement:placement adMetadata:metadata];
}

- (BOOL)isReady {
    return self.bannerView.isReady;
}

- (void)destroy {
    if (!self.bannerView) {
        NSLog(@"TuneAdsPlugin: BannerView is nil. Ignoring call to destroy");
        return;
    }
    
    _bannerView.delegate = nil;
    
    [_bannerView removeFromSuperview];
    [_bannerView release], _bannerView = nil;
}

- (void)layout
{
    if (!self.bannerView) {
        NSLog(@"TuneAdsPlugin: BannerView is nil. Ignoring call to layout");
        return;
    }
    
    UIView *unityView = [[[self class] unityGLViewController] view];
    
    CGFloat yPos = 0;
    // Position the TuneBannerView.
    switch (self.adPosition)
    {
        case kTuneAdPositionTopOfScreen:
            yPos = 0;
            break;
        case kTuneAdPositionBottomOfScreen:
        default:
            yPos = unityView.frame.size.height - self.bannerView.frame.size.height;
            break;
    }
    self.bannerView.frame = CGRectMake(0, yPos, unityView.frame.size.width, self.bannerView.frame.size.height);
}


#pragma mark Cleanup

- (void)dealloc {
    _bannerView.delegate = nil;
    [_bannerView release]; _bannerView = nil;
    [super dealloc];
}

@end
