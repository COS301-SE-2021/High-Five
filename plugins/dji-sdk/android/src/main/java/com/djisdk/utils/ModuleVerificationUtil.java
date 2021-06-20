package com.djisdk.utils;

import androidx.annotation.Nullable;

import com.djisdk.DJIApplication;

import dji.common.product.Model;
import dji.sdk.accessory.AccessoryAggregation;
import dji.sdk.accessory.beacon.Beacon;
import dji.sdk.accessory.speaker.Speaker;
import dji.sdk.accessory.spotlight.Spotlight;
import dji.sdk.base.BaseProduct;
import dji.sdk.flightcontroller.FlightController;
import dji.sdk.flightcontroller.Simulator;
import dji.sdk.products.Aircraft;
import dji.sdk.products.HandHeld;

public class ModuleVerificationUtil {
    public static boolean isProductModuleAvailable() {
        return (null != DJIApplication.getProductInstance());
    }

    public static boolean isAircraft() {
        return DJIApplication.getProductInstance() instanceof Aircraft;
    }

    public static boolean isHandHeld() {
        return DJIApplication.getProductInstance() instanceof HandHeld;
    }

    public static boolean isCameraModuleAvailable() {
        return isProductModuleAvailable() && (null != DJIApplication.getProductInstance().getCamera());
    }

    public static boolean isPlaybackAvailable() {
        return isCameraModuleAvailable() && (null != DJIApplication.getProductInstance()
                .getCamera()
                .getPlaybackManager());
    }

    public static boolean isMediaManagerAvailable() {
        return isCameraModuleAvailable() && (null != DJIApplication.getProductInstance()
                .getCamera()
                .getMediaManager());
    }

    public static boolean isRemoteControllerAvailable() {
        return isProductModuleAvailable() && isAircraft() && (null != DJIApplication.getAircraftInstance()
                .getRemoteController());
    }

    public static boolean isFlightControllerAvailable() {
        return isProductModuleAvailable() && isAircraft() && (null != DJIApplication.getAircraftInstance()
                .getFlightController());
    }

    public static boolean isCompassAvailable() {
        return isFlightControllerAvailable() && isAircraft() && (null != DJIApplication.getAircraftInstance()
                .getFlightController()
                .getCompass());
    }

    public static boolean isFlightLimitationAvailable() {
        return isFlightControllerAvailable() && isAircraft();
    }

    public static boolean isGimbalModuleAvailable() {
        return isProductModuleAvailable() && (null != DJIApplication.getProductInstance().getGimbal());
    }

    public static boolean isAirlinkAvailable() {
        return isProductModuleAvailable() && (null != DJIApplication.getProductInstance().getAirLink());
    }

    public static boolean isWiFiLinkAvailable() {
        return isAirlinkAvailable() && (null != DJIApplication.getProductInstance().getAirLink().getWiFiLink());
    }

    public static boolean isLightbridgeLinkAvailable() {
        return isAirlinkAvailable() && (null != DJIApplication.getProductInstance()
                .getAirLink()
                .getLightbridgeLink());
    }

    public static AccessoryAggregation getAccessoryAggregation() {
        Aircraft aircraft = (Aircraft) DJIApplication.getProductInstance();

        if (aircraft != null && null != aircraft.getAccessoryAggregation()) {
            return aircraft.getAccessoryAggregation();
        }
        return null;
    }

    public static Speaker getSpeaker() {
        Aircraft aircraft = (Aircraft) DJIApplication.getProductInstance();

        if (aircraft != null && null != aircraft.getAccessoryAggregation() && null != aircraft.getAccessoryAggregation().getSpeaker()) {
            return aircraft.getAccessoryAggregation().getSpeaker();
        }
        return null;
    }

    public static Beacon getBeacon() {
        Aircraft aircraft = (Aircraft) DJIApplication.getProductInstance();

        if (aircraft != null && null != aircraft.getAccessoryAggregation() && null != aircraft.getAccessoryAggregation().getBeacon()) {
            return aircraft.getAccessoryAggregation().getBeacon();
        }
        return null;
    }

    public static Spotlight getSpotlight() {
        Aircraft aircraft = (Aircraft) DJIApplication.getProductInstance();

        if (aircraft != null && null != aircraft.getAccessoryAggregation() && null != aircraft.getAccessoryAggregation().getSpotlight()) {
            return aircraft.getAccessoryAggregation().getSpotlight();
        }
        return null;
    }

    @Nullable
    public static Simulator getSimulator() {
        Aircraft aircraft = DJIApplication.getAircraftInstance();
        if (aircraft != null) {
            FlightController flightController = aircraft.getFlightController();
            if (flightController != null) {
                return flightController.getSimulator();
            }
        }
        return null;
    }

    @Nullable
    public static FlightController getFlightController() {
        Aircraft aircraft = DJIApplication.getAircraftInstance();
        if (aircraft != null) {
            return aircraft.getFlightController();
        }
        return null;
    }

    @Nullable
    public static boolean isMavic2Product() {
        BaseProduct baseProduct = DJIApplication.getProductInstance();
        if (baseProduct != null) {
            return baseProduct.getModel() == Model.MAVIC_2_PRO || baseProduct.getModel() == Model.MAVIC_2_ZOOM;
        }
        return false;
    }

    public static boolean isMavicAir2(){
        BaseProduct baseProduct = DJIApplication.getProductInstance();
        if (baseProduct != null) {
            return baseProduct.getModel() == Model.MAVIC_AIR_2;
        }
        return false;
    }

}