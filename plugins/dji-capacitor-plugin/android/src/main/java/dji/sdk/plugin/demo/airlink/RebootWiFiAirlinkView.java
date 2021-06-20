package dji.sdk.plugin.demo.airlink;

import android.content.Context;
import android.content.DialogInterface;



import dji.common.error.DJIError;
import dji.common.util.CommonCallbacks;
import dji.sdk.plugin.DJISampleApplication;
import dji.sdk.plugin.R;
import dji.sdk.plugin.utils.DialogUtils;
import dji.sdk.plugin.utils.ModuleVerificationUtil;
import dji.sdk.plugin.utils.ToastUtils;
import dji.sdk.plugin.view.BaseThreeBtnView;

/**
 * Class for rebooting the WiFi airlink. If the users set the WiFi airlink with SSID, password and frequency
 * , reboot the WiFi airlink so that it will take effect.
 */
public class RebootWiFiAirlinkView extends BaseThreeBtnView {
    public RebootWiFiAirlinkView(Context context) {
        super(context);
    }

    @Override
    protected void onAttachedToWindow() {
        super.onAttachedToWindow();
        if (!ModuleVerificationUtil.isWiFiLinkAvailable()) {
            ToastUtils.setResultToToast("Not Supported");
        }
    }

    @Override
    protected int getLeftBtnTextResourceId() {
        return DISABLE;
    }

    @Override
    protected int getDescriptionResourceId() {
        return R.string.reboot_wifi_airlink_description;
    }

    @Override
    protected void handleLeftBtnClick() {

    }

    @Override
    protected void handleRightBtnClick() {
    }

    @Override
    protected void handleMiddleBtnClick() {
        // Reboot Button
        DialogUtils.showConfirmationDialog(getContext(),
                R.string.reboot_wifi_airlink_hint,
                new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        if (ModuleVerificationUtil.isWiFiLinkAvailable()) {
                            DJISampleApplication.getProductInstance()
                                    .getAirLink()
                                    .getWiFiLink()
                                    .reboot(new CommonCallbacks.CompletionCallback() {
                                        @Override
                                        public void onResult(DJIError djiError) {
                                            if (djiError == null) {
                                                DialogUtils.showDialog(getContext(),
                                                        getResources()
                                                                .getString(
                                                                        R.string.success));
                                            } else {
                                                DialogUtils.showDialog(getContext(),
                                                        djiError.getDescription());
                                            }
                                        }
                                    });
                        }
                    }
                });
    }

    @Override
    protected int getRightBtnTextResourceId() {
        return DISABLE;
    }

    @Override
    protected int getMiddleBtnTextResourceId() {
        return R.string.reboot_wifi_airlink_reboot_wifi;
    }

    @Override
    public int getDescription() {
        return R.string.airlink_listview_wifi_reboot_wifi;
    }
}
