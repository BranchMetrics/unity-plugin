// Copyright 2014 Tune Inc. All Rights Reserved.

#import <Foundation/Foundation.h>

/// A cache to hold onto objects while Unity is still referencing them.
@interface TuneObjectCache : NSObject

+ (instancetype)sharedInstance;

/// References to objects Tune ads objects created from Unity.
@property(nonatomic, strong) NSMutableDictionary *references;

@end

@interface NSObject (TuneOwnershipAdditions)

/// Returns a key used to lookup a Tune Ads object. This method is intended to only be used
/// by Tune Ads objects.

- (NSString *)Tune_referenceKey;

@end