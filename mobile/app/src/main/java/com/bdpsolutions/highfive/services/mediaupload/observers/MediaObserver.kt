package com.bdpsolutions.highfive.services.mediaupload.observers

import io.reactivex.Observer
import io.reactivex.disposables.Disposable

class MediaObserver <T>: Observer<T> {

    private lateinit var _progressCallback: (t: T) -> Unit
    private lateinit var _errorCallback: (e: Throwable) -> Unit

    override fun onSubscribe(d: Disposable) {
    }

    override fun onNext(t: T) {
        _progressCallback(t)
    }

    override fun onError(e: Throwable) {
        _errorCallback(e)
    }

    override fun onComplete() {
    }

    fun setProgressCallback(callback: (t: T) -> Unit) {
        _progressCallback = callback
    }

    fun setErrorCallback(callback: (e: Throwable) -> Unit) {
        _errorCallback = callback
    }

}