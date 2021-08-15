package com.bdpsolutions.highfive.subsystems.media.viewmodel

import androidx.lifecycle.ViewModel

class MediaViewModel private constructor(): ViewModel() {
    // TODO: Implement the ViewModel

    companion object {
        fun create(): MediaViewModel {
            return MediaViewModel()
        }
    }
}