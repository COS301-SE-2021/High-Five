package com.bdpsolutions.highfive.subsystems.video.adapter

import androidx.recyclerview.widget.RecyclerView
import com.bdpsolutions.highfive.databinding.FragmentVideoItemBinding
import com.bdpsolutions.highfive.subsystems.video.view.VideoItemView

/**
 * Abstract class for the Recycler view adapter.
 *
 * This provides flexibility to choose implementations using Dependency Injection.
 */
abstract class VideoFragmentRecyclerViewAdapter:
    RecyclerView.Adapter<VideoFragmentRecyclerViewAdapter.ViewHolder>() {
    protected var dataSet: Array<VideoItemView>? = null

    /**
     * Provide a reference to the type of views that you are using
     * (custom ViewHolder).
     */
    class ViewHolder(view: FragmentVideoItemBinding) : RecyclerView.ViewHolder(view.root) {
        val videoItem: FragmentVideoItemBinding = view
    }

    /**
     * Sets the data values for the adapter to put on the UI
     *
     * @param value A list of video data
     */
    fun setData(value: Array<VideoItemView>) {
        dataSet = value
    }

    fun clearData() {
        dataSet = null
    }
}