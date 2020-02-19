#import <Foundation/Foundation.h>
#import "Tune/Tune.h"
#import "UnityAppController.h"

@interface TuneUnityAppController : UnityAppController

@end

@implementation TuneUnityAppController : UnityAppController

- (BOOL)application:(UIApplication *)application continueUserActivity:(NSUserActivity *)userActivity restorationHandler:(void (^)(NSArray *))restorationHandler {
    return [Tune handleContinueUserActivity:userActivity
                         restorationHandler:restorationHandler];
}

- (BOOL) application:(UIApplication *)application openURL:(nonnull NSURL *)url sourceApplication:(nullable NSString *)sourceApplication annotation:(nonnull id)annotation {
    [Tune handleOpenURL:url sourceApplication:sourceApplication];

    return [super application:application
                      openURL:url
            sourceApplication:sourceApplication
                   annotation:annotation];
}

- (BOOL)application:(UIApplication *)app openURL:(NSURL *)url options:(NSDictionary<UIApplicationOpenURLOptionsKey,id> *)options {
    [Tune handleOpenURL:url options:options];

    return [super application:app
                      openURL:url
            sourceApplication:options[UIApplicationOpenURLOptionsSourceApplicationKey]
                   annotation:nil];
}

@end

IMPL_APP_CONTROLLER_SUBCLASS(TuneUnityAppController)
