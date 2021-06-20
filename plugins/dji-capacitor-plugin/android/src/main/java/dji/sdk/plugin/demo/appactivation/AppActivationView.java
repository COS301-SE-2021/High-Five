package dji.sdk.plugin.demo.appactivation;

import android.content.Context;
import android.util.Log;

import dji.common.flightcontroller.adsb.AirSenseAirplaneState;
import dji.common.flightcontroller.adsb.AirSenseSystemInformation;
import dji.common.realname.AircraftBindingState;
import dji.common.realname.AircraftBindingState.AircraftBindingStateListener;
import dji.common.realname.AppActivationState;
import dji.common.realname.AppActivationState.AppActivationStateListener;
import dji.sdk.flightcontroller.FlightController;
import dji.sdk.plugin.DJISampleApplication;
import dji.sdk.plugin.R;
import dji.sdk.plugin.utils.ModuleVerificationUtil;
import dji.sdk.plugin.utils.ToastUtils;
import dji.sdk.plugin.view.BaseAppActivationView;
import dji.sdk.products.Aircraft;
import dji.sdk.realname.AppActivationManager;
import dji.sdk.sdkmanager.DJISDKManager;

/**
 * Class for determining whether the App is Activated.
 */
public class AppActivationView extends BaseAppActivationView {

    private static final String TAG = AppActivationView.class.getSimpleName();
    private AppActivationManager appActivationManager= DJISDKManager.getInstance().getAppActivationManager();
    private AppActivationStateListener activationStatelistener;
    private AircraftBindingStateListener bindingStateListener;

    public AppActivationView(Context context) {
        super(context);
    }

    //region View Life-Cycle
    @Override
    protected void onAttachedToWindow() {
        super.onAttachedToWindow();
        setUpListener();

        if (appActivationManager != null) {
            appActivationManager.addAppActivationStateListener(activationStatelistener);

            appActivationStateTV.setText("Activation State: " + appActivationManager.getAppActivationState());

            appActivationManager.addAircraftBindingStateListener(bindingStateListener);
            bindingStateTV.setText("Binding State: " + appActivationManager.getAircraftBindingState());
        }

        if (ModuleVerificationUtil.isFlightControllerAvailable()) {
            FlightController flightController =
                ((Aircraft) DJISampleApplication.getProductInstance()).getFlightController();

            if (flightController != null) {
                flightController.setASBInformationCallback(new AirSenseSystemInformation.Callback() {
                    @Override
                    public void onUpdate(AirSenseSystemInformation information) {
                        final StringBuffer sb = new StringBuffer();
                        addLineToSB(sb, "ADSB Info", "");
                        if (information.getWarningLevel() != null) {
                            addLineToSB(sb, "WarningLevel", "" + information.getWarningLevel().name());
                        }
                        if (information.getAirplaneStates() != null && information.getAirplaneStates().length > 0) {
                            for (int i = 0; i < information.getAirplaneStates().length; i++) {

                                AirSenseAirplaneState state = information.getAirplaneStates()[i];
                                if (state != null) {
                                    addLineToSB(sb, "", "");
                                    addLineToSB(sb, "flight ID", "" + i);
                                    addLineToSB(sb, "ICAO Code", "" + state.getCode());
                                    addLineToSB(sb, "Heading", "" + state.getHeading());
                                    addLineToSB(sb, "Direction", "" + state.getRelativeDirection());
                                    addLineToSB(sb, "Distance", "" + state.getDistance());
                                    addLineToSB(sb, "Warning level", "" + state.getWarningLevel());
                                }
                            }
                        }

                        ToastUtils.setResultToText(adsbStateTV, sb.toString());
                    }
                });
            }
        } else {
            Log.i(DJISampleApplication.TAG, "onAttachedToWindow FC NOT Available");
        }

    }

    @Override
    protected void onDetachedFromWindow() {
        tearDownListener();
        super.onDetachedFromWindow();
    }
    //endregion

    //region
    private void setUpListener() {
        // Example of Listener
        activationStatelistener = new AppActivationStateListener() {
            @Override
            public void onUpdate(final AppActivationState appActivationState) {

                ToastUtils.setResultToText(appActivationStateTV, "Activation State: " + appActivationState);
            }
        };

        bindingStateListener = new AircraftBindingStateListener() {

            @Override
            public void onUpdate(final AircraftBindingState bindingState) {

                ToastUtils.setResultToText(bindingStateTV, "Binding State: " + bindingState);
            }
        };
    }

    private void tearDownListener() {
        if (activationStatelistener != null) {
            // Example of removing listeners
            appActivationManager.removeAppActivationStateListener(activationStatelistener);
        }
        if (bindingStateListener !=null) {
            appActivationManager.removeAircraftBindingStateListener(bindingStateListener);
        }
    }

    public static void addLineToSB(StringBuffer sb, String name, Object value) {
        if (sb == null) return;
        sb.
              append((name == null || "".equals(name)) ? "" : name + ": ").
              append(value == null ? "" : value + "").
              append("\n");
    }

    @Override
    public int getDescription() {
        return R.string.component_listview_app_activation;
    }
}
