#!/usr/bin/env python

import argparse
import os
import shutil
import subprocess

# Update this value to the same SDK version that TunePluginDependencies.xml is using
SDK_AAR_FILENAME = '../../../Plugins/Android/com.tune.tune-marketing-console-sdk-5.0.2.aar'
SDK_JAR_FILENAME = 'tune-sdk.jar'

def compile_java(package_name, sdk_path, api_level):
    if 'JAVA_HOME' not in os.environ:
        try:
            java_home = subprocess.check_output('/usr/libexec/java_home').strip()
            os.environ['JAVA_HOME'] = java_home
        except:
            print 'Must set JAVA_HOME environment variable, exiting'
            raise ValueError('must set JAVA_HOME environment variable')

    os.chdir('./Plugins/Android')

    # Convert package name to folder path
    package_name_path = package_name.replace('.', '/')

    java_file = package_name_path + '/MyApplication.java'

    print 'Extracting jar from TUNE SDK aar...\n'

    # Unzip Tune SDK aar
    success = subprocess.call('unzip -p ' + SDK_AAR_FILENAME + ' classes.jar > ' + SDK_JAR_FILENAME, shell=True, env=os.environ)
    if success != 0:
        print 'Failed extracting classes.jar from ' + SDK_AAR_FILENAME + ', exiting'
        raise Exception('Failed extracting classes.jar from ' + SDK_AAR_FILENAME)

    print 'Compiling ' + java_file + '...\n'

    success = subprocess.call('javac -source 1.7 -target 1.7 ' +
        '-classpath ' + SDK_JAR_FILENAME + ':' +
        sdk_path + '/platforms/android-' + api_level + '/android.jar ' +
        java_file, shell=True, env=os.environ)

    if success != 0:
        print 'Failed compiling MyApplication.java, exiting'
        raise Exception('Failed compiling MyApplication.java')

    print 'Created .class files in /Tune/Plugins/Android/' + package_name_path + '...\n',

    # Delete extracted classes.jar file
    os.remove(SDK_JAR_FILENAME)

    compile_jar(package_name_path)

    return

def compile_jar(package_name_path):
    compile_call = 'jar cf MyApplication.jar ' + package_name_path + '/*.class'

    success = subprocess.call(compile_call, shell=True, env=os.environ)
    if success != 0:
        print 'Failed building MyApplication.jar, exiting'
        raise Exception('Failed building MyApplication.jar')

    # Delete generated .class files
    classes = os.popen('ls ' + package_name_path + '| grep ".class$"').read()
    for classfile in classes.splitlines():
        os.remove(package_name_path + '/' + classfile)

    print 'Created /Tune/Plugins/Android/MyApplication.jar'

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
    print 'Android API level ' + args.api_level + '...\n',

    compile_java(args.package_name, args.android_sdk, args.api_level)
