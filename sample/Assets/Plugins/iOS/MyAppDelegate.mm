#import "LifeCycleListener.h"
#import "Tune.h"

#pragma mark - Tune Plugin Helper Category

@interface Tune (TuneUnityPlugin)

+ (void)setPluginName:(NSString *)pluginName;

@end

@interface MyAppDelegate : NSObject<LifeCycleListener>

+(MyAppDelegate *)sharedInstance;

@end

static MyAppDelegate *_instance = [MyAppDelegate sharedInstance];

@implementation MyAppDelegate

#pragma mark - MyAppDelegate Methods

+(MyAppDelegate *)sharedInstance {
    return _instance;
}

+ (void)initialize {
    if (!_instance) {
        _instance = [MyAppDelegate new];
    }
}

- (id)init {
    if (_instance != nil) {
        return _instance;
    }

    self = [super init];
    if (!self) {
        return nil;
    }

    _instance = self;

    UnityRegisterLifeCycleListener(self);

    return self;
}

#pragma mark - Unity LifeCycleListener Callback Methods

- (void)didFinishLaunching:(NSNotification*)notification {
    [Tune initializeWithTuneAdvertiserId:@"877"
                       tuneConversionKey:@"8c14d6bbe466b65211e781d62e301eec"];
    [Tune setPluginName:@"unity"];
    [Tune setPackageName:@"com.hasoffers.unitytestapp"];
}

@end
