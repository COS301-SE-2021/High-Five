package com.bdpsolutions.highfive.subsystems.video.adapter

import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import com.bdpsolutions.highfive.databinding.FragmentVideoItemBinding
import javax.inject.Inject

/**
 * This class serves as an adapter for the video view fragment.
 *
 * Data fetched from the API is passed to this class, which will populate the video data.
 */
class VideoFragmentRecyclerViewAdapterImpl @Inject constructor():
    VideoFragmentRecyclerViewAdapter() {

    // Create new views (invoked by the layout manager)
    override fun onCreateViewHolder(viewGroup: ViewGroup, viewType: Int): ViewHolder {
        // Create a new view, which defines the UI of the list item
        val binding = FragmentVideoItemBinding.inflate(LayoutInflater.from(viewGroup.context))

        return ViewHolder(binding)
    }

    // Replace the contents of a view (invoked by the layout manager)
    override fun onBindViewHolder(viewHolder: ViewHolder, position: Int) {

        // Get element from your dataset at this position and replace the
        // contents of the view with that element

        viewHolder.videoItem.videoName.text = dataSet!![position].videoName
        viewHolder.videoItem.videoDate.text = dataSet!![position].videoDate
        viewHolder.videoItem.videoId.text = dataSet!![position].videoId
    }

    // Return the size of your dataset (invoked by the layout manager)
    override fun getItemCount() : Int {
        return if (dataSet != null) {
            dataSet!!.size
        } else {
            0
        }
    }
}
