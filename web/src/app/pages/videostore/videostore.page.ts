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
import {PipelineService} from "../../services/pipeline/pipeline.service";

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
                public videosService: VideosService, private popoverController: PopoverController,
                private pipelineService: PipelineService) {
        this.segment = 'all';
        this.pipelineService.fetchAllPipelines();
        this.pipelineService.fetchAllTools();
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
