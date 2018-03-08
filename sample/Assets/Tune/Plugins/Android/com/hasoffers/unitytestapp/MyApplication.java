package com.hasoffers.unitytestapp;

import com.tune.Tune;
import com.tune.ma.application.TuneApplication;

public class MyApplication extends TuneApplication {
    @Override
    public void onCreate() {
        super.onCreate();

        Tune tune = Tune.init(this, "877", "8c14d6bbe466b65211e781d62e301eec", true);
        tune.setPluginName("unity");
        tune.setPackageName("com.hasoffers.unitytestapp");
    }
}
