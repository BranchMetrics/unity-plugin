./ndk-build -B NDK_APPLICATION_MK=Application.mk -C "$NDK_PROJECT_PATH"
cd "$NDK_PROJECT_PATH"
rm -f ../libmobileapptracker.so
mv libs/armeabi/libmobileapptracker.so ..
rm -rf libs
rm -rf obj
