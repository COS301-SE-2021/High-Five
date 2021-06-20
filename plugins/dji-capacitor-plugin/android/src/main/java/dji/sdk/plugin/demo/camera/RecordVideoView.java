package dji.sdk.plugin.demo.camera;

import android.content.Context;


import java.util.Timer;
import java.util.TimerTask;
import java.util.concurrent.TimeUnit;

import dji.common.camera.SettingsDefinitions;
import dji.sdk.plugin.DJISampleApplication;
import dji.sdk.plugin.R;
import dji.sdk.plugin.utils.ModuleVerificationUtil;
import dji.sdk.plugin.utils.ToastUtils;
import dji.sdk.plugin.view.BaseThreeBtnView;

/**
 * Created by dji on 16/1/6.
 */
public class RecordVideoView extends BaseThreeBtnView {

    private Timer timer = new Timer();
    private long timeCounter = 0;
    private long hours = 0;
    private long minutes = 0;
    private long seconds = 0;
    private String time = "";

    public RecordVideoView(Context context) {
        super(context);
    }

    @Override
    protected void onAttachedToWindow() {
        super.onAttachedToWindow();

        if (!ModuleVerificationUtil.isCameraModuleAvailable()) {
            return;
        }
        if (ModuleVerificationUtil.isMavicAir2()){
            DJISampleApplication.getProductInstance()
                    .getCamera()
                    .setFlatMode(SettingsDefinitions.FlatCameraMode.VIDEO_NORMAL, djiError -> ToastUtils.setResultToToast("SetCameraMode to recordVideo"));
            return;
        }
        DJISampleApplication.getProductInstance()
                .getCamera()
                .setMode(SettingsDefinitions.CameraMode.RECORD_VIDEO, djiError -> ToastUtils.setResultToToast("SetCameraMode to recordVideo"));
    }

    @Override
    public void onDetachedFromWindow() {
        super.onDetachedFromWindow();

        if (ModuleVerificationUtil.isCameraModuleAvailable()) {
            return;
        }

        if (ModuleVerificationUtil.isMavicAir2()){
            DJISampleApplication.getProductInstance()
                    .getCamera()
                    .setFlatMode(SettingsDefinitions.FlatCameraMode.PHOTO_SINGLE, djiError -> ToastUtils.setResultToToast("SetCameraMode to shootPhoto"));
            return;
        }
        DJISampleApplication.getProductInstance()
                .getCamera()
                .setMode(SettingsDefinitions.CameraMode.SHOOT_PHOTO, djiError -> ToastUtils.setResultToToast("SetCameraMode to shootPhoto"));

    }

    @Override
    protected int getLeftBtnTextResourceId() {
        return R.string.record_video_start_record;
    }

    @Override
    protected int getRightBtnTextResourceId() {
        return R.string.record_video_stop_record;
    }

    @Override
    protected int getMiddleBtnTextResourceId() {
        return DISABLE;
    }

    @Override
    protected int getDescriptionResourceId() {
        return R.string.record_video_initial_time;
    }

    @Override
    protected void handleLeftBtnClick() {

        changeDescription("00:00:00");
        if (ModuleVerificationUtil.isCameraModuleAvailable()) {
            DJISampleApplication.getProductInstance()
                    .getCamera()
                    .startRecordVideo(djiError -> {
                        //success so, start recording
                        if (null == djiError) {
                            ToastUtils.setResultToToast("Start record");
                            timer = new Timer();
                            timer.schedule(new TimerTask() {
                                @Override
                                public void run() {
                                    timeCounter = timeCounter + 1;
                                    hours = TimeUnit.MILLISECONDS.toHours(timeCounter);
                                    minutes =
                                            TimeUnit.MILLISECONDS.toMinutes(timeCounter) - (hours * 60);
                                    seconds = TimeUnit.MILLISECONDS.toSeconds(timeCounter) - ((hours
                                            * 60
                                            * 60) + (minutes * 60));
                                    time = String.format("%02d:%02d:%02d", hours, minutes, seconds);
                                    changeDescription(time);
                                }
                            }, 0, 1);
                        }
                    });
        }
    }

    @Override
    protected void handleRightBtnClick() {

        if (ModuleVerificationUtil.isCameraModuleAvailable()) {
            DJISampleApplication.getProductInstance()
                    .getCamera()
                    .stopRecordVideo(djiError -> {
                        ToastUtils.setResultToToast("StopRecord");
                        changeDescription("00:00:00");
                        timer.cancel();
                        timeCounter = 0;
                    });
        }
    }

    @Override
    protected void handleMiddleBtnClick() {

    }

    @Override
    public int getDescription() {
        return R.string.camera_listview_record_video;
    }
}
