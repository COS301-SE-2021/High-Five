package com.bdpsolutions.highfive.adapters.video.adapter

import androidx.recyclerview.widget.RecyclerView
import com.bdpsolutions.highfive.view.views.VideoItemView

abstract class VideoFragmentRecyclerViewAdapter:
    RecyclerView.Adapter<VideoFragmentRecyclerViewAdapterImpl.ViewHolder>() {
        protected var dataSet: Array<VideoItemView>? = null

        fun setData(value: Array<VideoItemView>) {
            dataSet = value
        }
    }