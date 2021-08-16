package com.bdpsolutions.highfive.subsystems.image.adapter

import android.view.LayoutInflater
import android.view.ViewGroup
import com.bdpsolutions.highfive.databinding.FragmentImageItemBinding
import javax.inject.Inject

class ImageRecyclerViewAdapterImpl @Inject constructor() : ImageRecyclerViewAdapter() {
    // Create new views (invoked by the layout manager)
    override fun onCreateViewHolder(viewGroup: ViewGroup, viewType: Int): ViewHolder {
        // Create a new view, which defines the UI of the list item
        val binding = FragmentImageItemBinding.inflate(LayoutInflater.from(viewGroup.context))

        return ViewHolder(binding)
    }

    // Replace the contents of a view (invoked by the layout manager)
    override fun onBindViewHolder(viewHolder: ViewHolder, position: Int) {

        // Get element from your dataset at this position and replace the
        // contents of the view with that element

        viewHolder.imageItem.imageName.text = dataSet!![position].imageName
        viewHolder.imageItem.imageDate.text = dataSet!![position].imageDate
        viewHolder.imageItem.imageId.text = dataSet!![position].imageId
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