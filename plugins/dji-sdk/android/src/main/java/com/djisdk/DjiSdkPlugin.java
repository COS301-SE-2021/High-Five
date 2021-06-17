package com.djisdk;

import android.app.AlertDialog;

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

    @PluginMethod
    public void present(PluginCall call) {
        String message = call.getString("message");
        AlertDialog.Builder builder = new AlertDialog.Builder(this.bridge.getWebView().getContext());
        builder.setMessage(message).setTitle("Alert");
        AlertDialog dialog = builder.create();
        dialog.show();
    }
}
