package com.bdpsolutions.highfive.utils.retrofit

import androidx.annotation.NonNull
import okhttp3.MediaType
import okhttp3.RequestBody
import okio.ForwardingSink

import okio.Okio

import okio.BufferedSink
import okio.Sink
import java.io.IOException
import okio.Buffer


/**
 * @author Leo Nikkilä (https://medium.com/@PaulinaSadowska/display-progress-of-multipart-request-with-retrofit-and-rxjava-23a4a779e6ba)
 * with modifications made by Paulina Sadowska
 */
class CountingRequestBody(private val delegate: RequestBody, private val listener: Listener) : RequestBody() {

    override fun contentType(): MediaType? {
        return delegate.contentType()
    }

    override fun contentLength(): Long {
        try {
            return delegate.contentLength()
        } catch (e: IOException) {
            e.printStackTrace()
        }
        return -1
    }

    @Throws(IOException::class)
    override fun writeTo(@NonNull sink: BufferedSink) {
        val countingSink: CountingSink = CountingSink(sink)
        val bufferedSink = Okio.buffer(countingSink)
        delegate.writeTo(bufferedSink)
        bufferedSink.flush()
    }

    internal inner class CountingSink(delegate: Sink) : ForwardingSink(delegate) {
        private var bytesWritten: Long = 0

        @Throws(IOException::class)
        override fun write(@NonNull source: Buffer, byteCount: Long) {
            super.write(source, byteCount)
            bytesWritten += byteCount
            listener.onRequestProgress(bytesWritten, contentLength())
        }
    }

    interface Listener {
        fun onRequestProgress(bytesWritten: Long, contentLength: Long)
    }
}