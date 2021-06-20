package dji.sdk.plugin.demo.useraccount;
import android.app.Service
import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.widget.LinearLayout
import android.widget.TextView
import dji.common.error.DJIError
import dji.common.util.CommonCallbacks.CompletionCallbackWith
import dji.sdk.plugin.R
import dji.sdk.plugin.utils.ToastUtils
import dji.sdk.plugin.view.PresentableView
import dji.sdk.sdkmanager.DJISDKManager
import dji.sdk.sdkmanager.LDMManager.LDMCallback
import dji.sdk.sdkmanager.LDMModule
import dji.sdk.sdkmanager.LDMModuleType

class LDMView(context: Context?) : LinearLayout(context), PresentableView, View.OnClickListener {

    private lateinit var ldmInfoText: TextView
    private var isLDMSupported: Boolean = false
    private var isLDMEnabled: Boolean = false

    init {
        initUI(context)
        initListener()
    }

    private fun initUI(context: Context?) {
        isClickable = true
        orientation = VERTICAL
        val layoutInflater = context!!.getSystemService(Service.LAYOUT_INFLATER_SERVICE) as LayoutInflater
        layoutInflater.inflate(R.layout.view_ldm, this, true)
        ldmInfoText = findViewById(R.id.ldm_info)
        findViewById<View>(R.id.btn_enable_ldm).setOnClickListener(this)
        findViewById<View>(R.id.btn_disable_ldm).setOnClickListener(this)
        findViewById<View>(R.id.btn_enable_rtk_network).setOnClickListener(this)
        findViewById<View>(R.id.btn_disable_rtk_network).setOnClickListener(this)
        findViewById<View>(R.id.btn_is_enable_user_account).setOnClickListener(this)
        findViewById<View>(R.id.btn_is_disable_user_account).setOnClickListener(this)
        findViewById<View>(R.id.btn_get_ldm_license).setOnClickListener(this)
    }

    private fun initListener() {
        DJISDKManager.getInstance().ldmManager.setCallback(object : LDMCallback {
            override fun onLDMEnabledChange(isEnabled: Boolean) {
                isLDMEnabled = isEnabled;
                handler.post { updateLdmInfo() }
            }

            override fun onLDMSupportedChange(isSupported: Boolean) {
                isLDMSupported = isSupported;
                handler.post { updateLdmInfo() }
            }
        })
    }

    fun disableLDM() {
        DJISDKManager.getInstance().ldmManager.disableLDM()
        ToastUtils.setResultToToast("LDM Disabled!")
    }

    fun enableLDM() {
        DJISDKManager.getInstance().ldmManager.enableLDM {
            if (it == null)
                ToastUtils.setResultToToast("LDM Enabled")
            else
                ToastUtils.setResultToToast("Enable LDM failed=${it.description}")
        }
    }

    fun setRTKEnabled() {
        var error = DJISDKManager.getInstance().ldmManager.setModuleNetworkServiceEnabled(LDMModule.Builder()
                .enabled(true)
                .moduleType(LDMModuleType.RTK)
                .build())
        ToastUtils.setResultToToast("setRTKEnabled " + if (error == null) "success" else "error=" + error.description)
        updateLdmInfo()
    }

    fun setRTKDisabled() {
        var error = DJISDKManager.getInstance().ldmManager.setModuleNetworkServiceEnabled(LDMModule.Builder()
                .enabled(false)
                .moduleType(LDMModuleType.RTK)
                .build())
        ToastUtils.setResultToToast("setRTKDisabled " + if (error == null) "success" else "error=" + error.description)
        updateLdmInfo()
    }

    fun setUserAccountEnabled() {
        var error = DJISDKManager.getInstance().ldmManager.setModuleNetworkServiceEnabled(LDMModule.Builder()
                .enabled(true)
                .moduleType(LDMModuleType.USER_ACCOUNT)
                .build())
        ToastUtils.setResultToToast("setUserAccountEnabled " + if (error == null) "success" else "error=" + error.description)
        updateLdmInfo()
    }

    fun setUserAccountDisabled() {
        var error = DJISDKManager.getInstance().ldmManager.setModuleNetworkServiceEnabled(LDMModule.Builder()
                .enabled(false)
                .moduleType(LDMModuleType.USER_ACCOUNT)
                .build())
        ToastUtils.setResultToToast("setUserAccountDisabled " + if (error == null) "success" else "error=" + error.description)
        updateLdmInfo()
    }

    fun getLdmLicense() {
        DJISDKManager.getInstance().ldmManager.getLDMSupported(object : CompletionCallbackWith<Boolean> {
            override fun onSuccess(aBoolean: Boolean) {
                ToastUtils.setResultToToast("getLdmLicense result=$aBoolean")
            }

            override fun onFailure(error: DJIError) {
                ToastUtils.setResultToToast("getLdmLicense error=" + error.description)
            }
        })
        updateLdmInfo()
    }

    private fun updateLdmInfo() {
        isLDMEnabled =  DJISDKManager.getInstance().ldmManager.isLDMEnabled
        var isRTKEnabled = DJISDKManager.getInstance().ldmManager.isModuleNetworkServiceEnabled(LDMModuleType.RTK)
        var isUserAccountEnabled = DJISDKManager.getInstance().ldmManager.isModuleNetworkServiceEnabled(LDMModuleType.USER_ACCOUNT)

        ldmInfoText.text = "LDM enabled: $isLDMEnabled\n" +
                "LDM supported: $isLDMSupported\n" +
                "isRTKEnabled: $isRTKEnabled\n" +
                "isUserAccountEnabled: $isUserAccountEnabled\n"
    }

    override fun getDescription(): Int = R.string.component_listview_ldm

    override fun getHint(): String = this.javaClass.simpleName

    override fun onClick(v: View?) {
        when (v?.id) {
            R.id.btn_enable_ldm -> enableLDM()
            R.id.btn_disable_ldm -> disableLDM()
            R.id.btn_enable_rtk_network -> setRTKEnabled()
            R.id.btn_disable_rtk_network -> setRTKDisabled()
            R.id.btn_is_enable_user_account -> setUserAccountEnabled()
            R.id.btn_is_disable_user_account -> setUserAccountDisabled()
            R.id.btn_get_ldm_license -> getLdmLicense()
        }
    }
}