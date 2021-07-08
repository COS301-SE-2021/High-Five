package com.bdpsolutions.highfive.utils.adapters

import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import com.bdpsolutions.highfive.databinding.FragmentVideoItemBinding

/**
 * This class serves as an adapter for the video view fragment.
 *
 * Data fetched from the API is passed to this class, which will populate the video data.
 */
class VideoFragmentRecyclerViewAdapter(private val dataSet: Array<String>) :
    RecyclerView.Adapter<VideoFragmentRecyclerViewAdapter.ViewHolder>() {

    /**
     * Provide a reference to the type of views that you are using
     * (custom ViewHolder).
     */
    class ViewHolder(view: FragmentVideoItemBinding) : RecyclerView.ViewHolder(view.root) {
        val videoItem: FragmentVideoItemBinding = view
    }

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

        viewHolder.videoItem.videoName.text = dataSet[position]
        //viewHolder.videoItem.binding?.videoDate?.text = dataSet[position]
    }

    // Return the size of your dataset (invoked by the layout manager)
    override fun getItemCount() = dataSet.size

}
