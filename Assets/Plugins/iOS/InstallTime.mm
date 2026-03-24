#import <Foundation/Foundation.h>

#ifdef __cplusplus
extern "C" {
#endif

__attribute__((visibility("default")))
__attribute__((used))
long long _GetInstallTimestampV2(void) {
    @autoreleasepool {
        NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
        NSString *documentsPath = [paths lastObject];
        
        NSDictionary *attributes = [[NSFileManager defaultManager] attributesOfItemAtPath:documentsPath error:nil];
        NSDate *creationDate = [attributes objectForKey:NSFileCreationDate];
        
        if (creationDate) {
            return (long long)([creationDate timeIntervalSince1970] * 1000.0);
        }
        
        return -1;
    }
}

#ifdef __cplusplus
}
#endif