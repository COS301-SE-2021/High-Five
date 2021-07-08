package com.bdpsolutions.highfive.adapters.video.adapter

import androidx.recyclerview.widget.RecyclerView
import com.bdpsolutions.highfive.view.views.VideoItemView

/**
 * Abstract class for the Recycler view adapter.
 *
 * This provides flexibility to choose implementations using Dependency Injection.
 */
abstract class VideoFragmentRecyclerViewAdapter:
    RecyclerView.Adapter<VideoFragmentRecyclerViewAdapterImpl.ViewHolder>() {
        protected var dataSet: Array<VideoItemView>? = null

    /**
     * Sets the data values for the adapter to put on the UI
     *
     * @param value A list of video data
     */
    fun setData(value: Array<VideoItemView>) {
            dataSet = value
        }
    }