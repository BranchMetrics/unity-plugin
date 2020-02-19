#import "LifeCycleListener.h"
#import "Tune/Tune.h"

#pragma mark - Tune Plugin Helper Category

// expose a private Tune method
@interface Tune (TuneUnityPlugin)

+ (void)setPluginName:(NSString *)pluginName;

@end

@interface MyAppDelegate : NSObject<LifeCycleListener>

@end

@implementation MyAppDelegate

#pragma mark - MyAppDelegate Methods

+ (MyAppDelegate *)sharedInstance {
    static MyAppDelegate *delegate;
    static dispatch_once_t onceToken;
    
    dispatch_once(&onceToken, ^{
        delegate = [MyAppDelegate new];
    });
    
    return delegate;
}

- (instancetype)init {
    self = [super init];
    if (!self) {
        UnityRegisterLifeCycleListener(self);
    }
    return self;
}

#pragma mark - Unity LifeCycleListener Callback Methods

- (void)didFinishLaunching:(NSNotification*)notification {
    [Tune initializeWithTuneAdvertiserId:@"877" tuneConversionKey:@"8c14d6bbe466b65211e781d62e301eec" tunePackageName:@"com.hasoffers.unitytestapp"];
    [Tune setPluginName:@"unity"];
}

@end
