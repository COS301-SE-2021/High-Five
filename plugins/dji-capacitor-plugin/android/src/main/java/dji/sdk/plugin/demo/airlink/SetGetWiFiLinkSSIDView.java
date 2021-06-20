package dji.sdk.plugin.demo.airlink;

import android.content.Context;
import android.widget.ArrayAdapter;


import java.util.ArrayList;
import java.util.List;

import dji.common.error.DJIError;
import dji.common.util.CommonCallbacks;
import dji.sdk.plugin.DJISampleApplication;
import dji.sdk.plugin.R;
import dji.sdk.plugin.utils.DialogUtils;
import dji.sdk.plugin.utils.ModuleVerificationUtil;
import dji.sdk.plugin.utils.ToastUtils;
import dji.sdk.plugin.view.BaseSetGetView;

/**
 * Class for setting and getting SSID in WiFiLink.
 */
public class SetGetWiFiLinkSSIDView extends BaseSetGetView {

    private static final String SSID_NAME1 = "Happy";
    private static final String SSID_NAME2 = "Super";

    private List<String> mSSIDName;

    public SetGetWiFiLinkSSIDView(Context context) {
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
    protected void setMethod() {
        if (ModuleVerificationUtil.isWiFiLinkAvailable()) {
            DJISampleApplication.getProductInstance()
                    .getAirLink()
                    .getWiFiLink()
                    .setSSID(mSSIDName.get(mSpinnerSet.getSelectedItemPosition()),
                            new CommonCallbacks.CompletionCallback() {
                                @Override
                                public void onResult(DJIError djiError) {
                                    DialogUtils.showDialogBasedOnError(getContext(), djiError);
                                }
                            });
        }
    }

    @Override
    protected void getMethod() {
        if (ModuleVerificationUtil.isWiFiLinkAvailable()) {
            DJISampleApplication.getProductInstance()
                    .getAirLink()
                    .getWiFiLink()
                    .getSSID(new CommonCallbacks.CompletionCallbackWith<String>() {
                        @Override
                        public void onSuccess(String s) {
                            mGetTextString = s;
                            mHandler.sendEmptyMessage(SET_GET_TEXTVIEW_WITH_RESULT);
                        }

                        @Override
                        public void onFailure(DJIError djiError) {

                        }
                    });
        }
    }

    @Override
    protected ArrayAdapter getArrayAdapter() {
        if (null == mSSIDName) {
            mSSIDName = new ArrayList<>();
        }
        mSSIDName.add(SSID_NAME1);
        mSSIDName.add(SSID_NAME2);
        return new ArrayAdapter(this.getContext(), R.layout.simple_list_item, mSSIDName);
    }

    @Override
    public int getDescription() {
        return R.string.set_get_wifi_airlink_ssid_description;
    }
}
