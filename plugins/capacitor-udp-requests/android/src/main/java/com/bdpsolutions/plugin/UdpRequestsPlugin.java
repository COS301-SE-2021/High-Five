package com.bdpsolutions.plugin;

import android.Manifest;

import com.bdpsolutions.plugin.sender.Sender;
import com.bdpsolutions.plugin.server.UdpVideoServer;
import com.getcapacitor.JSObject;
import com.getcapacitor.PermissionState;
import com.getcapacitor.Plugin;
import com.getcapacitor.PluginCall;
import com.getcapacitor.PluginMethod;
import com.getcapacitor.annotation.CapacitorPlugin;
import com.getcapacitor.annotation.Permission;
import com.getcapacitor.annotation.PermissionCallback;


@CapacitorPlugin(name = "UdpRequests",
permissions ={
        @Permission(
                alias = "requests",
                strings = {
                        Manifest.permission.INTERNET,
                        Manifest.permission.ACCESS_NETWORK_STATE,
                        Manifest.permission.ACCESS_WIFI_STATE,

                }
        )
})
public class UdpRequestsPlugin extends Plugin {

    private UdpRequests implementation = new UdpRequests();
    private UdpVideoServer videoServer;
    @PluginMethod
    public void echo(PluginCall call) {
        String value = call.getString("value");

        JSObject ret = new JSObject();
        ret.put("value", implementation.echo(value));
        call.resolve(ret);
    }

    @PluginMethod
    public void sendUdpRequest(PluginCall call){
        if(getPermissionState("requests")!= PermissionState.GRANTED){
            requestPermissionForAlias("requests", call , "requestsPermissionCallback");
        }else{
           handleUdpRequest(call);
        }
    }

    @PluginMethod
    public void getVideoStream(PluginCall call){
        if(getPermissionState("requests")!= PermissionState.GRANTED){
            requestPermissionForAlias("requests", call , "requestsPermissionCallback");
        }else{
            startVideoStreamServer();
        }
    }

    @PluginMethod
    public void stopVideoStream(PluginCall call){
        if(getPermissionState("requests")!= PermissionState.GRANTED){
            requestPermissionForAlias("requests", call , "requestsPermissionCallback");
        }else{
            stopVideoStreamServer();
        }
    }

    private void startVideoStreamServer(){
        videoServer = new UdpVideoServer();
    }

    private void stopVideoStreamServer(){
        videoServer.stopServer();
    }



    private void handleUdpRequest(PluginCall call){
        String address = call.getString("address");
        String port = call.getString("port");
        String payload = call.getString("payload");
        if(payload!=null && port != null && address != null){
            Sender sender = new Sender(address,port);
            String response;
            if(payload.equals("command")){
                response = sender.sendMessage(payload);
            }else{
                sender.sendMessageVoid(payload);
                response = "ok";
            }

            sender.close();
            JSObject ret = new JSObject();
            ret.put("status" , "Ok");
            ret.put("responseMessage" , response);
            call.resolve(ret);
        }else{
            JSObject ret = new JSObject();
            ret.put("status" , "Error");
            ret.put("responseMessage" , "All fields were not provided (port, address and payload)");
            call.resolve(ret);
        }
    }

    @PermissionCallback
    private void requestsPermissionCallback(PluginCall call){
        if (getPermissionState("requests") == PermissionState.GRANTED){
            handleUdpRequest(call);
        }else{
            call.reject("Needed permissions not granted to send requests");
        }
    }


}
