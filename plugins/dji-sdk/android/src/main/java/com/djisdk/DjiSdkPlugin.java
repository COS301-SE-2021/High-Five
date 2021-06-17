package com.djisdk;

import android.content.Intent;

import com.getcapacitor.JSObject;
import com.getcapacitor.Plugin;
import com.getcapacitor.PluginCall;
import com.getcapacitor.PluginMethod;
import com.getcapacitor.annotation.CapacitorPlugin;

@CapacitorPlugin(name = "DjiSdk")
public class DjiSdkPlugin extends Plugin {

    private DjiSdk implementation = new DjiSdk();

    @PluginMethod
    public void echo(PluginCall call) {
        String value = call.getString("value");

        JSObject ret = new JSObject();
        ret.put("value", implementation.echo(value));
        call.resolve(ret);
    }

    /**
     * Plugin method that will start the main DJI activity to interface with the DJI SDK
     * @param call Call method passed in by Capacitor
     */
    @PluginMethod
    public void present(PluginCall call) {
        Intent startDJI = new Intent(getContext(), DJIActivity.class);
        getContext().startActivity(startDJI);
    }
}
