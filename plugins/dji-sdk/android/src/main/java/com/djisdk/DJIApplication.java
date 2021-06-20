package com.djisdk;

import android.app.Application;
import android.content.Context;
import androidx.multidex.MultiDex;
import com.secneo.sdk.Helper;
import com.squareup.otto.Bus;
import com.squareup.otto.ThreadEnforcer;
import dji.sdk.base.BaseProduct;
import dji.sdk.products.Aircraft;
import dji.sdk.products.HandHeld;
import dji.sdk.sdkmanager.BluetoothProductConnector;
import dji.sdk.sdkmanager.DJISDKManager;

public class DJIApplication extends Application {

    public static final String TAG = DJIApplication.class.getName();

    private static BaseProduct product;
    private static BluetoothProductConnector bluetoothConnector = null;
    private static Bus bus = new Bus(ThreadEnforcer.ANY);
    private static Application app = null;

    /**
     * Gets instance of the specific product connected after the
     * API KEY is successfully validated.
     */
    public static synchronized BaseProduct getProductInstance() {
        product = DJISDKManager.getInstance().getProduct();
        return product;
    }


    /**
     * Function to check if the drone is connected to the application
     * @return true is aircraft is connected else false.
     */
    public static boolean isAircraftConnected() {
        return getProductInstance() != null && getProductInstance() instanceof Aircraft;
    }

    /**
     * Function to check whether the controller is connected to the application
     * @return true if handheld controller is connected else false.
     */
    public static boolean isHandHeldConnected() {
        return getProductInstance() != null && getProductInstance() instanceof HandHeld;
    }

    /**
     * Function to get instance of the connected drone.
     * @return Instance of connected Aircraft.
     */
    public static synchronized Aircraft getAircraftInstance() {
        if (!isAircraftConnected()) {
            return null;
        }
        return (Aircraft) getProductInstance();
    }

    /**
     * Function to get the instance of the Handheld controller
     * @return Instance of current handheld controller.
     */
    public static synchronized HandHeld getHandHeldInstance() {
        if (!isHandHeldConnected()) {
            return null;
        }
        return (HandHeld) getProductInstance();
    }

    /**
     * Function to get the instance of this application
     * @return Instance of this application.
     */
    public static Application getInstance() {
        return DJIApplication.app;
    }

    /**
     * Function to get the current event bus.
     * @return Current event Bus.
     */
    public static Bus getEventBus() {
        return bus;
    }

    /**
     * Attaches paramContext to baseContext.
     * Activates MultiDex for this application.
     * Installs the sdk helper for this application.
     * @param paramContext
     */
    @Override
    protected void attachBaseContext(Context paramContext) {
        super.attachBaseContext(paramContext);
        MultiDex.install(this);
        com.secneo.sdk.Helper.install(this);
        app = this;
    }
}
