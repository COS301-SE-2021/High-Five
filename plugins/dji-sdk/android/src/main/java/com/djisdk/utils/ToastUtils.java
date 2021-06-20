package com.djisdk.utils;

import android.os.Handler;
import android.os.Looper;
import android.os.Message;
import android.util.Pair;
import android.widget.TextView;
import android.widget.Toast;

import com.djisdk.DJIApplication;

/**
 * Utility class that manages showing toasts to the user.
 */
public class ToastUtils {
    private static final int MESSAGE_UPDATE = 1;
    private static final int MESSAGE_TOAST = 2;
    /**
     * The UIHandler object with its anonymous function to handle receieving a message.
     */
    private static Handler mUIHandler = new Handler(Looper.getMainLooper()) {
        @Override
        public void handleMessage(Message msg) {
            //Get the message string
            switch ((msg.what)) {
                case MESSAGE_UPDATE:
                    showMessage((Pair<TextView, String>) msg.obj);
                    break;
                case MESSAGE_TOAST:
                    showToast((String) msg.obj);
                    break;
                default:
                    super.handleMessage(msg);
            }
        }
    };

    /**
     * Prints the toast message to the screen.
     * @param msg - The message to be printed to screen.
     */
    private static void showMessage(Pair<TextView, String> msg) {
        if (msg != null) {
            if (msg.first == null) {
                Toast.makeText(DJIApplication.getInstance(), "tv is null", Toast.LENGTH_SHORT).show();
            } else {
                msg.first.setText(msg.second);
            }
        }
    }

    /**
     * Shows a string message to the user.
     * @param msg - The message to show to the user.
     */
    public static void showToast(String msg) {
        Toast.makeText(DJIApplication.getInstance(), msg, Toast.LENGTH_SHORT).show();
    }

    /**
     * Creates a new message, sets the value and sends it to the UIHandler.
     * @param string - The string message to send in the UIHandler.
     */
    public static void setResultToToast(final String string) {
        Message msg = new Message();
        msg.what = MESSAGE_TOAST;
        msg.obj = string;
        mUIHandler.sendMessage(msg);
    }

    /**
     * Function that creates a message, sets the value as a pair of textview and string objects and then sends the message to the UIHandler
     * @param tv - The textView to update.
     * @param s - The string to send
     */
    public static void setResultToText(final TextView tv, final String s) {
        Message msg = new Message();
        msg.what = MESSAGE_UPDATE;
        msg.obj = new Pair<>(tv, s);
        mUIHandler.sendMessage(msg);
    }
}
