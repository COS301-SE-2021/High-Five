package com.bdpsolutions.highfive.subsystems.image.adapter

import android.content.Intent
import android.net.Uri
import android.util.Log
import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.core.content.ContextCompat.startActivity
import com.bdpsolutions.highfive.databinding.FragmentImageItemBinding
import com.bdpsolutions.highfive.utils.ContextHolder
import com.nostra13.universalimageloader.core.ImageLoader
import com.squareup.picasso.Picasso
import javax.inject.Inject
import com.squareup.picasso.Callback
import java.lang.Exception
import android.graphics.Bitmap
import android.view.View
import androidx.core.content.ContextCompat

import com.nostra13.universalimageloader.core.listener.SimpleImageLoadingListener
import java.io.ByteArrayOutputStream
import java.io.File
import java.io.FileOutputStream
import androidx.core.content.ContextCompat.startActivity
import com.nostra13.universalimageloader.core.ImageLoaderConfiguration
import androidx.core.content.FileProvider

import android.os.Build.VERSION_CODES

import android.os.Build.VERSION
import androidx.browser.customtabs.CustomTabsClient.getPackageName


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

        Picasso.get()
            .load(dataSet!![position].imageUrl)
            .into(viewHolder.imageItem.imageThumbnail, object : Callback {
                override fun onSuccess() {
                    viewHolder.imageItem.imageName.text = dataSet!![viewHolder.adapterPosition].imageName
                    viewHolder.imageItem.imageDate.text = dataSet!![viewHolder.adapterPosition].imageDate
                    viewHolder.imageItem.imageId.text = dataSet!![viewHolder.adapterPosition].imageId

                    viewHolder.imageItem.imageThumbnail.setOnClickListener {
                        val imageLoader: ImageLoader = ImageLoader.getInstance()
                        imageLoader.init(ImageLoaderConfiguration.createDefault(ContextHolder.appContext))
                        imageLoader.loadImage(dataSet!![viewHolder.adapterPosition].imageUrl.toString(), object : SimpleImageLoadingListener() {
                            override fun onLoadingComplete(
                                imageUri: String?,
                                view: View?,
                                loadedImage: Bitmap?
                            ) {
                                val bos = ByteArrayOutputStream()
                                loadedImage?.compress(Bitmap.CompressFormat.PNG, 100, bos)
                                val bitmapdata: ByteArray = bos.toByteArray()
                                val outputDir: File =
                                    ContextHolder.appContext!!.cacheDir

                                val outputFile = File(outputDir, "tmp.png")
                                val fos = FileOutputStream(outputFile)
                                fos.write(bitmapdata)
                                fos.flush()
                                fos.close()

                                val intent = Intent(Intent.ACTION_VIEW) //
                                    .setDataAndType(
                                        if (VERSION.SDK_INT >= VERSION_CODES.N) FileProvider.getUriForFile(
                                            ContextHolder.appContext!!,
                                            ContextHolder.appContext!!.packageName.toString() + ".provider",
                                            outputFile
                                        ) else Uri.fromFile(outputFile),
                                        "image/*"
                                    ).addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION)


                                ContextHolder.appContext!!.startActivity(intent)
                            }
                        })
                    }

                }

                override fun onError(e: Exception?) {
                    Log.e("Picasso", e?.message!!)
                }
            })
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