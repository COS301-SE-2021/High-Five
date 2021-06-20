package dji.sdk.plugin;

import android.content.Intent;

import com.getcapacitor.JSObject;
import com.getcapacitor.Plugin;
import com.getcapacitor.PluginCall;
import com.getcapacitor.PluginMethod;
import com.getcapacitor.annotation.CapacitorPlugin;
import com.secneo.sdk.DexInstall;

@CapacitorPlugin(name = "DjiSdkPLugin")
public class DjiSdkPluginPlugin extends Plugin {

    private DjiSdkPlugin implementation = new DjiSdkPlugin();

    @PluginMethod
    public void echo(PluginCall call) {
        Intent startDji = new Intent(getContext(), DjiActivity.class);
        getContext().startActivity(startDji);
    }
}
