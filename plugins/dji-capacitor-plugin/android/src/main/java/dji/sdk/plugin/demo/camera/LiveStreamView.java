package dji.sdk.plugin.demo.camera;

import android.app.Service;
import android.content.Context;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;

import androidx.annotation.NonNull;


import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;

import dji.sdk.base.BaseProduct;
import dji.sdk.camera.VideoFeeder;
import dji.sdk.plugin.DJISampleApplication;
import dji.sdk.plugin.R;
import dji.sdk.plugin.utils.Helper;
import dji.sdk.plugin.utils.ToastUtils;
import dji.sdk.plugin.utils.VideoFeedView;
import dji.sdk.plugin.view.PresentableView;
import dji.sdk.sdkmanager.DJISDKManager;
import dji.sdk.sdkmanager.LiveStreamManager;

/**
 * Class for live stream demo.
 *
 * @author Hoker
 * @date 2019/1/28
 * <p>
 * Copyright (c) 2019, DJI All Rights Reserved.
 */
public class LiveStreamView extends LinearLayout implements PresentableView, View.OnClickListener {

    private String liveShowUrl = "please input your live show url here";

    private VideoFeedView primaryVideoFeedView;
    private VideoFeedView fpvVideoFeedView;
    private EditText showUrlInputEdit;

    private Button startLiveShowBtn;
    private Button enableVideoEncodingBtn;
    private Button disableVideoEncodingBtn;
    private Button stopLiveShowBtn;
    private Button soundOnBtn;
    private Button soundOffBtn;
    private Button isLiveShowOnBtn;
    private Button showInfoBtn;
    private Button showLiveStartTimeBtn;
    private Button showCurrentVideoSourceBtn;
    private Button changeVideoSourceBtn;

    private LiveStreamManager.OnLiveChangeListener listener;
    private LiveStreamManager.LiveStreamVideoSource currentVideoSource = LiveStreamManager.LiveStreamVideoSource.Primary;

    public LiveStreamView(Context context) {
        super(context);
        initUI(context);
        initListener();
    }

    private void initUI(Context context) {
        setClickable(true);
        setOrientation(VERTICAL);
        LayoutInflater layoutInflater = (LayoutInflater) context.getSystemService(Service.LAYOUT_INFLATER_SERVICE);
        layoutInflater.inflate(R.layout.view_live_stream, this, true);

        primaryVideoFeedView = (VideoFeedView) findViewById(R.id.video_view_primary_video_feed);
        primaryVideoFeedView.registerLiveVideo(VideoFeeder.getInstance().getPrimaryVideoFeed(), true);

        fpvVideoFeedView = (VideoFeedView) findViewById(R.id.video_view_fpv_video_feed);
        fpvVideoFeedView.registerLiveVideo(VideoFeeder.getInstance().getSecondaryVideoFeed(), false);
        if (Helper.isMultiStreamPlatform()){
            fpvVideoFeedView.setVisibility(VISIBLE);
        }

        showUrlInputEdit = (EditText) findViewById(R.id.edit_live_show_url_input);
        showUrlInputEdit.setText(liveShowUrl);

        startLiveShowBtn = (Button) findViewById(R.id.btn_start_live_show);
        enableVideoEncodingBtn = (Button) findViewById(R.id.btn_enable_video_encode);
        disableVideoEncodingBtn = (Button) findViewById(R.id.btn_disable_video_encode);
        stopLiveShowBtn = (Button) findViewById(R.id.btn_stop_live_show);
        soundOnBtn = (Button) findViewById(R.id.btn_sound_on);
        soundOffBtn = (Button) findViewById(R.id.btn_sound_off);
        isLiveShowOnBtn = (Button) findViewById(R.id.btn_is_live_show_on);
        showInfoBtn = (Button) findViewById(R.id.btn_show_info);
        showLiveStartTimeBtn = (Button) findViewById(R.id.btn_show_live_start_time);
        showCurrentVideoSourceBtn = (Button) findViewById(R.id.btn_show_current_video_source);
        changeVideoSourceBtn = (Button) findViewById(R.id.btn_change_video_source);

        startLiveShowBtn.setOnClickListener(this);
        enableVideoEncodingBtn.setOnClickListener(this);
        disableVideoEncodingBtn.setOnClickListener(this);
        stopLiveShowBtn.setOnClickListener(this);
        soundOnBtn.setOnClickListener(this);
        soundOffBtn.setOnClickListener(this);
        isLiveShowOnBtn.setOnClickListener(this);
        showInfoBtn.setOnClickListener(this);
        showLiveStartTimeBtn.setOnClickListener(this);
        showCurrentVideoSourceBtn.setOnClickListener(this);
        changeVideoSourceBtn.setOnClickListener(this);
    }

    private void initListener() {
        showUrlInputEdit.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {
                liveShowUrl = s.toString();
            }

            @Override
            public void afterTextChanged(Editable s) {

            }
        });
        listener = new LiveStreamManager.OnLiveChangeListener() {
            @Override
            public void onStatusChanged(int i) {
                ToastUtils.setResultToToast("status changed : " + i);
            }
        };
    }

    @Override
    protected void onAttachedToWindow() {
        super.onAttachedToWindow();
        BaseProduct product = DJISampleApplication.getProductInstance();
        if (product == null || !product.isConnected()) {
            ToastUtils.setResultToToast("Disconnect");
            return;
        }
        if (isLiveStreamManagerOn()){
            DJISDKManager.getInstance().getLiveStreamManager().registerListener(listener);
        }
    }

    @Override
    protected void onDetachedFromWindow() {
        super.onDetachedFromWindow();
        if (isLiveStreamManagerOn()){
            DJISDKManager.getInstance().getLiveStreamManager().unregisterListener(listener);
        }
    }

    @Override
    public int getDescription() {
        return R.string.component_listview_live_stream;
    }

    @NonNull
    @Override
    public String getHint() {
        return this.getClass().getSimpleName() + ".java";
    }

    void startLiveShow() {
        ToastUtils.setResultToToast("Start Live Show");
        if (!isLiveStreamManagerOn()) {
            return;
        }
        if (DJISDKManager.getInstance().getLiveStreamManager().isStreaming()) {
            ToastUtils.setResultToToast("already started!");
            return;
        }
        new Thread() {
            @Override
            public void run() {
                DJISDKManager.getInstance().getLiveStreamManager().setLiveUrl(liveShowUrl);
                int result = DJISDKManager.getInstance().getLiveStreamManager().startStream();
                DJISDKManager.getInstance().getLiveStreamManager().setStartTime();
                ToastUtils.setResultToToast("startLive:" + result +
                        "\n isVideoStreamSpeedConfigurable:" + DJISDKManager.getInstance().getLiveStreamManager().isVideoStreamSpeedConfigurable() +
                        "\n isLiveAudioEnabled:" + DJISDKManager.getInstance().getLiveStreamManager().isLiveAudioEnabled());
            }
        }.start();
    }

    private void enableReEncoder() {
        if (!isLiveStreamManagerOn()) {
            return;
        }
        DJISDKManager.getInstance().getLiveStreamManager().setVideoEncodingEnabled(true);
        ToastUtils.setResultToToast("Force Re-Encoder Enabled!");
    }

    private void disableReEncoder() {
        if (!isLiveStreamManagerOn()) {
            return;
        }
        DJISDKManager.getInstance().getLiveStreamManager().setVideoEncodingEnabled(false);
        ToastUtils.setResultToToast("Disable Force Re-Encoder!");
    }

    private void stopLiveShow() {
        if (!isLiveStreamManagerOn()) {
            return;
        }
        DJISDKManager.getInstance().getLiveStreamManager().stopStream();
        ToastUtils.setResultToToast("Stop Live Show");
    }

    private void soundOn() {
        if (!isLiveStreamManagerOn()) {
            return;
        }
        DJISDKManager.getInstance().getLiveStreamManager().setAudioMuted(false);
        ToastUtils.setResultToToast("Sound On");
    }

    private void soundOff() {
        if (!isLiveStreamManagerOn()) {
            return;
        }
        DJISDKManager.getInstance().getLiveStreamManager().setAudioMuted(true);
        ToastUtils.setResultToToast("Sound Off");
    }

    private void isLiveShowOn() {
        if (!isLiveStreamManagerOn()) {
            return;
        }
        ToastUtils.setResultToToast("Is Live Show On:" + DJISDKManager.getInstance().getLiveStreamManager().isStreaming());
    }

    private void showInfo() {
        if (!isLiveStreamManagerOn()) {
            return;
        }
        StringBuilder sb = new StringBuilder();
        sb.append("Video BitRate:").append(DJISDKManager.getInstance().getLiveStreamManager().getLiveVideoBitRate()).append(" kpbs\n");
        sb.append("Audio BitRate:").append(DJISDKManager.getInstance().getLiveStreamManager().getLiveAudioBitRate()).append(" kpbs\n");
        sb.append("Video FPS:").append(DJISDKManager.getInstance().getLiveStreamManager().getLiveVideoFps()).append("\n");
        sb.append("Video Cache size:").append(DJISDKManager.getInstance().getLiveStreamManager().getLiveVideoCacheSize()).append(" frame");
        ToastUtils.setResultToToast(sb.toString());
    }

    private void showLiveStartTime() {
        if (!isLiveStreamManagerOn()) {
            return;
        }
        if (!DJISDKManager.getInstance().getLiveStreamManager().isStreaming()){
            ToastUtils.setResultToToast("Please Start Live First");
            return;
        }
        long startTime = DJISDKManager.getInstance().getLiveStreamManager().getStartTime();
        SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss",Locale.getDefault());
        String sd = sdf.format(new Date(Long.parseLong(String.valueOf(startTime))));
        ToastUtils.setResultToToast("Live Start Time: " + sd);
    }

    private void changeVideoSource() {
        if (!isLiveStreamManagerOn()) {
            return;
        }
        if (!isSupportSecondaryVideo()) {
            return;
        }
        if (DJISDKManager.getInstance().getLiveStreamManager().isStreaming()) {
            ToastUtils.setResultToToast("Before change live source, you should stop live stream!");
            return;
        }
        currentVideoSource = (currentVideoSource == LiveStreamManager.LiveStreamVideoSource.Primary) ?
                LiveStreamManager.LiveStreamVideoSource.Secoundary :
                LiveStreamManager.LiveStreamVideoSource.Primary;
        DJISDKManager.getInstance().getLiveStreamManager().setVideoSource(currentVideoSource);

        ToastUtils.setResultToToast("Change Success ! Video Source : " + currentVideoSource.name());
    }

    private void showCurrentVideoSource(){
        ToastUtils.setResultToToast("Video Source : " + currentVideoSource.name());
    }

    private boolean isLiveStreamManagerOn() {
        if (DJISDKManager.getInstance().getLiveStreamManager() == null) {
            ToastUtils.setResultToToast("No live stream manager!");
            return false;
        }
        return true;
    }

    private boolean isSupportSecondaryVideo(){
        if (!Helper.isMultiStreamPlatform()) {
            ToastUtils.setResultToToast("No secondary video!");
            return false;
        }
        return true;
    }

    @Override
    public void onClick(View v) {
        int id = v.getId();
        if (id == R.id.btn_start_live_show) {
            startLiveShow();
        } else if (id == R.id.btn_enable_video_encode) {
            enableReEncoder();
        } else if (id == R.id.btn_disable_video_encode) {
            disableReEncoder();
        } else if (id == R.id.btn_stop_live_show) {
            stopLiveShow();
        } else if (id == R.id.btn_sound_on) {
            soundOn();
        } else if (id == R.id.btn_sound_off) {
            soundOff();
        } else if (id == R.id.btn_is_live_show_on) {
            isLiveShowOn();
        } else if (id == R.id.btn_show_info) {
            showInfo();
        } else if (id == R.id.btn_show_live_start_time) {
            showLiveStartTime();
        } else if (id == R.id.btn_show_current_video_source) {
            showCurrentVideoSource();
        } else if (id == R.id.btn_change_video_source) {
            changeVideoSource();
        }
    }
}
