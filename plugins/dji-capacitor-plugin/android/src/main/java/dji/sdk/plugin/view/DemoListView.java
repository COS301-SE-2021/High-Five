package dji.sdk.plugin.view;

import android.content.Context;
import android.util.AttributeSet;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.ExpandableListView;
import android.widget.FrameLayout;


import com.squareup.otto.Subscribe;

import dji.sdk.plugin.DJISampleApplication;
import dji.sdk.plugin.DjiActivity;
import dji.sdk.plugin.ExpandableListAdapter;
import dji.sdk.plugin.R;
import dji.sdk.plugin.demo.accessory.AccessoryAggregationView;
import dji.sdk.plugin.demo.accessory.AudioFileListManagerView;
import dji.sdk.plugin.demo.airlink.RebootWiFiAirlinkView;
import dji.sdk.plugin.demo.airlink.SetGetWiFiLinkSSIDView;
import dji.sdk.plugin.demo.appactivation.AppActivationView;
import dji.sdk.plugin.demo.battery.PushBatteryDataView;
import dji.sdk.plugin.demo.battery.SetGetDischargeDayView;
import dji.sdk.plugin.demo.camera.FetchMediaView;
import dji.sdk.plugin.demo.camera.LiveStreamView;
import dji.sdk.plugin.demo.camera.MediaPlaybackView;
import dji.sdk.plugin.demo.camera.PlaybackCommandsView;
import dji.sdk.plugin.demo.camera.PlaybackDownloadView;
import dji.sdk.plugin.demo.camera.PlaybackPushInfoView;
import dji.sdk.plugin.demo.camera.PushCameraDataView;
import dji.sdk.plugin.demo.camera.RecordVideoView;
import dji.sdk.plugin.demo.camera.SetGetISOView;
import dji.sdk.plugin.demo.camera.ShootSinglePhotoView;
import dji.sdk.plugin.demo.camera.VideoFeederView;
import dji.sdk.plugin.demo.camera.XT2CameraView;
import dji.sdk.plugin.demo.datalocker.AccessLockerView;
import dji.sdk.plugin.demo.firmwareupgrade.FirmwareUpgradeView;
import dji.sdk.plugin.demo.flightcontroller.CompassCalibrationView;
import dji.sdk.plugin.demo.flightcontroller.FlightAssistantPushDataView;
import dji.sdk.plugin.demo.flightcontroller.FlightHubView;
import dji.sdk.plugin.demo.flightcontroller.FlightLimitationView;
import dji.sdk.plugin.demo.flightcontroller.OrientationModeView;
import dji.sdk.plugin.demo.flightcontroller.VirtualStickView;
import dji.sdk.plugin.demo.gimbal.GimbalCapabilityView;
import dji.sdk.plugin.demo.gimbal.MoveGimbalWithSpeedView;
import dji.sdk.plugin.demo.gimbal.PushGimbalDataView;
import dji.sdk.plugin.demo.key.KeyedInterfaceView;
import dji.sdk.plugin.demo.keymanager.KeyManagerView;
import dji.sdk.plugin.demo.missionoperator.WaypointMissionOperatorView;
import dji.sdk.plugin.demo.missionoperator.WaypointV2MissionOperatorView;
import dji.sdk.plugin.demo.mobileremotecontroller.MobileRemoteControllerView;
//import dji.sdk.plugin.demo.radar.RadarView;
import dji.sdk.plugin.demo.remotecontroller.PushRemoteControllerDataView;
import dji.sdk.plugin.demo.timeline.TimelineMissionControlView;
//import dji.sdk.plugin.demo.useraccount.LDMView;
import dji.sdk.plugin.model.GroupHeader;
import dji.sdk.plugin.model.GroupItem;
import dji.sdk.plugin.model.ListItem;


/**
 * This view is in charge of showing all the demos in a list.
 */

public class DemoListView extends FrameLayout {

    private ExpandableListAdapter listAdapter;
    private ExpandableListView expandableListView;

    public DemoListView(Context context) {
        this(context, null, 0);
    }

    public DemoListView(Context context, AttributeSet attrs) {
        this(context, attrs, 0);
    }

    public DemoListView(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
        initView(context);
    }

    private void initView(Context context) {
        final LayoutInflater inflater = LayoutInflater.from(context);
        View view = inflater.inflate(R.layout.demo_list_view, this);

        // Build model for ListView
        ListItem.ListBuilder builder = new ListItem.ListBuilder();
//        builder.addGroup(R.string.component_listview_sdk_4_14,
//                false,
//                new GroupItem(R.string.component_listview_radar, RadarView.class),
//                new GroupItem(R.string.component_listview_ldm, LDMView.class));
//        builder.addGroup(R.string.component_listview_sdk_4_12,
//                false,
//                new GroupItem(R.string.component_listview_multiple_lens_camera, MultipleLensCameraView.class),
//                new GroupItem(R.string.component_listview_health_information, HealthInformationView.class),
//                new GroupItem(R.string.component_listview_waypointv2, WaypointV2MissionOperatorView.class),
//                new GroupItem(R.string.component_listview_utmiss, StartUTMISSActivityView.class));
        builder.addGroup(R.string.component_listview_sdk_4_11,
                false,
                new GroupItem(R.string.component_listview_firmware_upgrade, FirmwareUpgradeView.class));
        builder.addGroup(R.string.component_listview_sdk_4_9,
                false,
                new GroupItem(R.string.component_listview_live_stream, LiveStreamView.class));
        builder.addGroup(R.string.component_listview_sdk_4_8,
                false,
                new GroupItem(R.string.component_listview_access_locker, AccessLockerView.class),
                new GroupItem(R.string.component_listview_accessory_aggregation,
                        AccessoryAggregationView.class),
                new GroupItem(R.string.component_listview_audio_file_list_manager,
                        AudioFileListManagerView.class));
        builder.addGroup(R.string.component_listview_sdk_4_6,
                false,
                new GroupItem(R.string.component_listview_payload,
                        StartPayloadAcitivityView.class),
                new GroupItem(R.string.component_listview_redirect_to_djigo,
                        StartRedirectGoAcitivityView.class));
        builder.addGroup(R.string.component_listview_sdk_4_5,
                false,
                new GroupItem(R.string.component_listview_flight_hub,
                        FlightHubView.class));

        builder.addGroup(R.string.component_listview_sdk_4_1,
                false,
                new GroupItem(R.string.component_listview_app_activation,
                        AppActivationView.class));


        builder.addGroup(R.string.component_listview_sdk_4_0,
                false,
                new GroupItem(R.string.component_listview_waypoint_mission_operator,
                        WaypointMissionOperatorView.class),
                new GroupItem(R.string.component_listview_keyed_interface, KeyedInterfaceView.class),
                new GroupItem(R.string.component_listview_timeline_mission_control,
                        TimelineMissionControlView.class));

        builder.addGroup(R.string.component_listview_key_manager, false,
                new GroupItem(R.string.key_manager_listview_key_Interface, KeyManagerView.class));

        builder.addGroup(R.string.component_listview_camera,
                false,
                new GroupItem(R.string.camera_listview_push_info, PushCameraDataView.class),
                new GroupItem(R.string.camera_listview_iso, SetGetISOView.class),
                new GroupItem(R.string.camera_listview_shoot_single_photo, ShootSinglePhotoView.class),
                new GroupItem(R.string.camera_listview_record_video, RecordVideoView.class),
                new GroupItem(R.string.camera_listview_playback_push_info, PlaybackPushInfoView.class),
                new GroupItem(R.string.camera_listview_playback_command, PlaybackCommandsView.class),
                new GroupItem(R.string.camera_listview_playback_download, PlaybackDownloadView.class),
                new GroupItem(R.string.camera_listview_download_media, FetchMediaView.class),
                new GroupItem(R.string.camera_listview_media_playback, MediaPlaybackView.class),
                new GroupItem(R.string.component_listview_video_feeder, VideoFeederView.class),
                new GroupItem(R.string.component_xt2_camera_view, XT2CameraView.class));

        builder.addGroup(R.string.component_listview_gimbal,
                false,
                new GroupItem(R.string.gimbal_listview_push_info, PushGimbalDataView.class),
                new GroupItem(R.string.gimbal_listview_rotate_gimbal, MoveGimbalWithSpeedView.class),
                new GroupItem(R.string.gimbal_listview_gimbal_capability, GimbalCapabilityView.class));

        builder.addGroup(R.string.component_listview_battery,
                false,
                new GroupItem(R.string.battery_listview_push_info, PushBatteryDataView.class),
                new GroupItem(R.string.battery_listview_set_get_discharge_day, SetGetDischargeDayView.class));

        builder.addGroup(R.string.component_listview_airlink,
                false,
                new GroupItem(R.string.airlink_listview_wifi_set_get_ssid, SetGetWiFiLinkSSIDView.class),
                new GroupItem(R.string.airlink_listview_wifi_reboot_wifi, RebootWiFiAirlinkView.class),
                new GroupItem(R.string.airlink_listview_lb_set_get_channel, SetGetWiFiLinkSSIDView.class));

        builder.addGroup(R.string.component_listview_flight_controller,
                false,
                new GroupItem(R.string.flight_controller_listview_compass_calibration,
                        CompassCalibrationView.class),
                new GroupItem(R.string.flight_controller_listview_flight_limitation,
                        FlightLimitationView.class),
                new GroupItem(R.string.flight_controller_listview_orientation_mode, OrientationModeView.class),
                new GroupItem(R.string.flight_controller_listview_virtual_stick, VirtualStickView.class),
                new GroupItem(R.string.flight_controller_listview_intelligent_flight_assistant,
                        FlightAssistantPushDataView.class));

        builder.addGroup(R.string.component_listview_remote_controller,
                false,
                new GroupItem(R.string.remote_controller_listview_push_info,
                        PushRemoteControllerDataView.class),
                new GroupItem(R.string.component_listview_mobile_remote_controller,
                        MobileRemoteControllerView.class));

        // Set-up ExpandableListView
        expandableListView = (ExpandableListView) view.findViewById(R.id.expandable_list);
        listAdapter = new ExpandableListAdapter(context, builder.build());
        expandableListView.setAdapter(listAdapter);
        DJISampleApplication.getEventBus().register(this);
        expandAllGroupIfNeeded();
    }

    @Subscribe
    public void onSearchQueryEvent(DjiActivity.SearchQueryEvent event) {
        listAdapter.filterData(event.getQuery());
        expandAllGroup();
    }

    /**
     * Expands all the group that has children
     */
    private void expandAllGroup() {
        int count = listAdapter.getGroupCount();
        for (int i = 0; i < count; i++) {
            expandableListView.expandGroup(i);
        }
    }

    /**
     * Expands all the group that has children
     */
    private void expandAllGroupIfNeeded() {
        int count = listAdapter.getGroupCount();
        for (int i = 0; i < count; i++) {
            if (listAdapter.getGroup(i) instanceof GroupHeader
                    && ((GroupHeader) listAdapter.getGroup(i)).shouldCollapseByDefault()) {
                expandableListView.collapseGroup(i);
            } else {
                expandableListView.expandGroup(i);
            }
        }
    }
}
