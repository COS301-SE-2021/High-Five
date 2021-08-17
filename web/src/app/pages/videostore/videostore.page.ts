import {Component, OnInit, ViewChild} from '@angular/core';
import {
    IonInfiniteScroll,
    LoadingController,
    ModalController,
    PopoverController,
    ToastController
} from '@ionic/angular';
import {VideoMetaData} from '../../models/videoMetaData';
import {VideoStoreConstants} from '../../../constants/pages/videostore-constants';
import {ImageMetaData} from '../../models/imageMetaData';
import {ImagesService} from "../../services/images/images.service";
import {VideosService} from "../../services/videos/videos.service";

@Component({
    selector: 'app-videostore',
    templateUrl: './videostore.page.html',
    styleUrls: ['./videostore.page.scss'],
})
export class VideostorePage implements OnInit {

    @ViewChild(IonInfiniteScroll) infiniteScroll: IonInfiniteScroll;
    imagesTrackFn = (i, image) => image.id;
    videoTrackFn = (v, video) => video.id;

    public videos: VideoMetaData[] = [];
    public videosFetched = false;
    public segment: string;
    public images: ImageMetaData[] = [];

    constructor(private modal: ModalController,
                public toastController: ToastController,
                private loadingController: LoadingController,
                private constants: VideoStoreConstants, public imagesService: ImagesService,
                public videosService: VideosService, private popoverController: PopoverController) {
        this.segment = 'all';
    }

    ngOnInit() {
    }


    /**
     * Sends an uploaded video to the backend using the VideoUpload service.
     *
     * @param video
     */
    async uploadVideo(video: any) {

        // const loading = await this.loadingController.create({
        //   spinner: 'circles',
        //   animated: true,
        // });
        // await loading.present();
        // this.mediaStorageService.storeVideoForm(video.target.files[0]).subscribe(() => {
        //   this.updateVideos();
        //   loading.dismiss();
        // });
        await this.videosService.addVideo(video.target.files[0]);
    }

    /**
     * Shows a toast once a video is successfully uploaded.
     */
    // async presentAlert() {
    //   const alert = await this.toastController.create({
    //     cssClass: 'alert-style',
    //     header: this.constants.toastLabels.header,
    //     message: this.constants.toastLabels.message,
    //     buttons: this.constants.toastLabels.buttons
    //   });
    //   await alert.present();
    // }

    /**
     * This function will delete an image from the user's account, optimistic loading updates are used and in the event
     * and error is thrown, the image is added back and an appropriate toast is shown
     *
     * @param imageId the ID of the image we wish to delete
     */

    async uploadImage(image: any) {
        // const loading = await this.loadingController.create({
        //   spinner: 'circles',
        //   animated: true,
        // });
        // await loading.present();
        // this.mediaStorageService.storeImageForm(image.target.files[0]).subscribe(() => {
        //   this.updateImages();
        //   loading.dismiss();
        // });
        await this.imagesService.addImage(image.target.files[0]);
        //Nothing added here yet
    }

}
