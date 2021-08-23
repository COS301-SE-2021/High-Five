package com.bdpsolutions.highfive.subsystems.image.adapter

import androidx.recyclerview.widget.RecyclerView
import com.bdpsolutions.highfive.databinding.FragmentImageItemBinding
import com.bdpsolutions.highfive.subsystems.image.view.ImageItemView

abstract class ImageRecyclerViewAdapter : RecyclerView.Adapter<ImageRecyclerViewAdapter.ViewHolder>() {
    protected var dataSet: Array<ImageItemView>? = null

    /**
     * Provide a reference to the type of views that you are using
     * (custom ViewHolder).
     */
    class ViewHolder(view: FragmentImageItemBinding) : RecyclerView.ViewHolder(view.root) {
        val imageItem: FragmentImageItemBinding = view
    }

    /**
     * Sets the data values for the adapter to put on the UI
     *
     * @param value A list of video data
     */
    fun setData(value: Array<ImageItemView>) {
        dataSet = value
    }

    fun clearData() {
        dataSet = null
    }
}