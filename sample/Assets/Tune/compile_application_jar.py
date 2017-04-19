#!/usr/bin/env python

import argparse
import os
import shutil
import subprocess

def compile_java(package_name, sdk_path, api_level):
    if 'JAVA_HOME' not in os.environ:
        try:
            java_home = subprocess.check_output('/usr/libexec/java_home').strip()
            os.environ['JAVA_HOME'] = java_home
        except:
            raise ValueError('must set JAVA_HOME environment variable')

    os.chdir('../Plugins/Android')

    # Convert package name to folder path
    package_name_path = package_name.replace('.', '/')

    java_file = package_name_path + '/MyApplication.java'

    success = subprocess.call('javac -source 1.7 -target 1.7 ' +
        '-classpath android-support-v4.jar:' +
        'eventbus-2.4.0.jar:' +
        'TuneMarketingConsoleSDK-4.10.1.jar:' +
        sdk_path + '/platforms/android-' + api_level + '/android.jar ' +
        java_file, shell=True, env=os.environ)

    if success != 0:
        raise Exception('Failed compiling MyApplication.java')

    print 'Created .class files in /Plugins/Android/' + package_name_path + '...',

    compile_jar(package_name_path)

    return

def compile_jar(package_name_path):
    compile_call = 'jar cf MyApplication.jar ' + package_name_path + '/*.class'

    success = subprocess.call(compile_call, shell=True, env=os.environ)
    if success != 0:
        raise Exception('Failed building MyApplication.jar')

    # Delete generated .class files
    classes = os.popen('ls ' + package_name_path + '| grep ".class$"').read()
    for classfile in classes.splitlines():
        os.remove(package_name_path + '/' + classfile)

    print 'Created /Plugins/Android/MyApplication.jar'

    return

if __name__ == '__main__':
    '''
    Parse command line argument
    '''
    parser = argparse.ArgumentParser()
    parser.add_argument('-p', help='App package name/bundle identifier', dest='package_name', required=True)
    parser.add_argument('-s', help='Android SDK path', dest='android_sdk', required = True)
    parser.add_argument('-l', help='Android SDK API level', dest='api_level', required = True)

    args = parser.parse_args()

    print 'Running MyApplication.jar build script with package name ' + args.package_name + ', ',
    print 'Android SDK ' + args.android_sdk + ', ',
    print 'Android API level ' + args.api_level + '...',

    compile_java(args.package_name, args.android_sdk, args.api_level)
