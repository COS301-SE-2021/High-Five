import {Component, OnInit} from '@angular/core';
import {VideosService} from "../../services/videos/videos.service";
import {ImagesService} from "../../services/images/images.service";
import {AnalyzedVideosService} from "../../services/analyzed-videos/analyzed-videos.service";
import {AnalyzedImagesService} from "../../services/analyzed-images/analyzed-images.service";

@Component({
  selector: 'app-allmedia',
  templateUrl: './allmedia.page.html',
  styleUrls: ['./allmedia.page.scss'],
})
export class AllmediaPage implements OnInit {

  analyzedImageTrackFn = (ai, analyzed_image) => analyzed_image.id;
  imageTrackFn = (i, image) => image.id;
  videoTrackFn = (v, video) => video.id;
  analyzedVideoTrackFn = (av, analyzed_video) => analyzed_video.id;
  segment: string;

  constructor(public videosService: VideosService, public imagesService: ImagesService,
              public analyzedVideosService: AnalyzedVideosService, public analyzedImagesService: AnalyzedImagesService) {
    this.segment = 'all'
  }

  ngOnInit() {
  }


  /**
   * Sends an uploaded video to the backend using the videosService service.
   *
   * @param video
   */
  async uploadVideo(video: any) {
    // const newArr = this.videosService.videos.map((video: VideoMetaData)=>{return video.name});
    // if(newArr.filter((value) => {return video.target.files[0].name}).length>0 ) {
    //     await this.toastController.create({
    //         message: 'Videos may not have duplicate names, please choose another name',
    //         duration: 2000,
    //         translucent: true,
    //         position: 'bottom'
    //     }).then((toast)=>{
    //         toast.present();
    //     })
    // }else{
    //     await this.videosService.addVideo(video.target.files[0]);
    // }
    await this.videosService.addVideo(video.target.files[0]);
  }


  /**
   * This function will delete an image from the user's account, optimistic loading updates are used and in the event
   * and error is thrown, the image is added back and an appropriate toast is shown
   *
   * @param image the data of the image we wish to upload
   */

  async uploadImage(image: any) {

    await this.imagesService.addImage(image.target.files[0]);
    //Nothing added here yet
  }
}
