#!/usr/bin/env python

import argparse
import os
import shutil
import subprocess

def create_java_file(advertiser_id, conversion_key, package_name):
    os.chdir('./Plugins/Android')

    # Convert package name to folder path
    package_name_path = package_name.replace('.', '/')

    application_java_path = package_name_path + '/MyApplication.java'

    print 'Creating /Tune/Plugins/Android/' + application_java_path + '...',

    # If package name folders do not exist, create them
    if not os.path.exists(os.path.dirname(application_java_path)):
        try:
            os.makedirs(os.path.dirname(application_java_path))
        except OSError as exc: # Guard against race condition
            if exc.errno != errno.EEXIST:
                raise

    # If MyApplication.java does not exist, create it
    if not os.path.isfile(application_java_path):
        # Create a new file
        with open(application_java_path, 'w') as file:
            # Write a default Application implementation
            file.write('package ' + package_name + ';\n\n')
            file.write('import com.tune.Tune;\n')
            file.write('import com.tune.ma.application.TuneApplication;\n\n')
            file.write('public class MyApplication extends TuneApplication {\n')
            file.write('    @Override\n')
            file.write('    public void onCreate() {\n')
            file.write('        super.onCreate();\n\n')
            file.write('        Tune tune = Tune.init(this, "' + advertiser_id + '", "' + conversion_key +'", true);\n')
            file.write('        tune.setPluginName("unity");\n')
            file.write('        tune.setPackageName("' + package_name + '");\n')
            file.write('    }\n')
            file.write('}\n')
        file.close()
        print 'Successfully created /Tune/Plugins/Android/' + application_java_path + '!'
    else:
        print '/Tune/Plugins/Android/' + application_java_path + ' already exists, exiting'

def create_lifecycle_listener(advertiser_id, conversion_key, package_name):
    os.chdir('./Plugins/iOS')

    filename = 'MyAppDelegate.mm'

    print 'Creating /Tune/Plugins/iOS/' + filename + '...',

    # If MyAppDelegate.mm does not exist, create it
    if not os.path.isfile(filename):
        # Create a new file
        with open(filename, 'w') as file:
            # Write a default LifecycleListener implementation
            file.write('#import "LifeCycleListener.h"\n')
            file.write('#import "Tune/Tune.h"\n\n')
            file.write('#pragma mark - Tune Plugin Helper Category\n\n')
            file.write('@interface Tune (TuneUnityPlugin)\n\n')
            file.write('+ (void)setPluginName:(NSString *)pluginName;\n\n')
            file.write('@end\n\n')
            file.write('@interface MyAppDelegate : NSObject<LifeCycleListener>\n\n')
            file.write('+(MyAppDelegate *)sharedInstance;\n\n')
            file.write('@end\n\n')
            file.write('static MyAppDelegate *_instance = [MyAppDelegate sharedInstance];\n\n')
            file.write('@implementation MyAppDelegate\n\n')
            file.write('#pragma mark - MyAppDelegate Methods\n\n')
            file.write('+(MyAppDelegate *)sharedInstance {\n')
            file.write('    return _instance;\n')
            file.write('}\n\n')
            file.write('+ (void)initialize {\n')
            file.write('    if (!_instance) {\n')
            file.write('        _instance = [MyAppDelegate new];\n')
            file.write('    }\n')
            file.write('}\n\n')
            file.write('- (id)init {\n')
            file.write('    if (_instance != nil) {\n')
            file.write('        return _instance;\n')
            file.write('    }\n\n')
            file.write('    self = [super init];\n')
            file.write('    if (!self) {\n')
            file.write('        return nil;\n')
            file.write('    }\n\n')
            file.write('    _instance = self;\n\n')
            file.write('    UnityRegisterLifeCycleListener(self);\n\n')
            file.write('    return self;\n')
            file.write('}\n\n')
            file.write('#pragma mark - Unity LifeCycleListener Callback Methods\n\n')
            file.write('- (void)didFinishLaunching:(NSNotification*)notification {\n')
            file.write('    [Tune initializeWithTuneAdvertiserId:@"' + advertiser_id + '"\n')
            file.write('                       tuneConversionKey:@"' + conversion_key + '"];\n')
            file.write('    [Tune setPluginName:@"unity"];\n')
            file.write('    [Tune setPackageName:@"' + package_name +'"];\n')
            file.write('}\n\n')
            file.write('@end\n')
        file.close()
        print 'Successfully created /Tune/Plugins/iOS/' + filename + '!'
    else:
        print '/Tune/Plugins/iOS/' + filename + ' already exists, exiting'

if __name__ == '__main__':
    '''
    Parse command line argument
    '''
    parser = argparse.ArgumentParser()
    parser.add_argument('-p', help='Platform', dest='platform', required = True)
    parser.add_argument('-a', help='Advertiser ID', dest='advertiser_id', required = True)
    parser.add_argument('-c', help='Conversion key', dest='conversion_key', required = True)
    parser.add_argument('-pn', help='App package name/bundle identifier', dest='package_name', required=True)

    args = parser.parse_args()

    if (args.platform == 'android'):
        create_java_file(args.advertiser_id, args.conversion_key, args.package_name)
    elif (args.platform == 'ios'):
        create_lifecycle_listener(args.advertiser_id, args.conversion_key, args.package_name)
