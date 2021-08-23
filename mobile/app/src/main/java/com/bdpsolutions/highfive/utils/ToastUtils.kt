package com.bdpsolutions.highfive.utils

import android.os.Handler
import android.os.Looper
import android.os.Message
import android.util.Pair
import android.widget.TextView
import android.widget.Toast
import com.bdpsolutions.highfive.subsystems.main.HighFiveApplication

object ToastUtils {
    private const val MESSAGE_UPDATE = 1
    private const val MESSAGE_TOAST = 2
    private val mUIHandler: Handler = object : Handler(Looper.getMainLooper()) {
        override fun handleMessage(msg: Message) {
            //Get the message string
            when (msg.what) {
                MESSAGE_UPDATE -> showMessage(msg.obj as Pair<TextView?, String>)
                MESSAGE_TOAST -> showToast(msg.obj as String)
                else -> super.handleMessage(msg)
            }
        }
    }

    private fun showMessage(msg: Pair<TextView?, String>?) {
        if (msg != null) {
            if (msg.first == null) {
                Toast.makeText(HighFiveApplication.getInstance(), "tv is null", Toast.LENGTH_SHORT)
                    .show()
            } else {
                msg.first!!.text = msg.second
            }
        }
    }

    fun showToast(msg: String?) {
        Toast.makeText(HighFiveApplication.getInstance(), msg, Toast.LENGTH_SHORT).show()
    }

    fun setResultToToast(string: String?) {
        val msg = Message()
        msg.what = MESSAGE_TOAST
        msg.obj = string
        mUIHandler.sendMessage(msg)
    }

    fun setResultToText(tv: TextView, s: String) {
        val msg = Message()
        msg.what = MESSAGE_UPDATE
        msg.obj = Pair(tv, s)
        mUIHandler.sendMessage(msg)
    }
}
