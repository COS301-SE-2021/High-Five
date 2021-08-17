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
import {AnalyzedVideosService} from "../../services/analyzed-videos/analyzed-videos.service";
import {AnalyzedImagesService} from "../../services/analyzed-images/analyzed-images.service";
import {MediaFilterComponent} from "../../components/media-filter/media-filter.component";

@Component({
    selector: 'app-videostore',
    templateUrl: './videostore.page.html',
    styleUrls: ['./videostore.page.scss'],
})
export class VideostorePage implements OnInit {

    @ViewChild(IonInfiniteScroll) infiniteScroll: IonInfiniteScroll;
    imagesTrackFn = (i, image) => image.id;
    videoTrackFn = (v, video) => video.id;
    analyzedVideoTrackFn = (av, analyzed_video) => analyzed_video.id;
    public segment: string;
    public analyzed : boolean;
    constructor(private modal: ModalController,
                public toastController: ToastController,
                private loadingController: LoadingController,
                private constants: VideoStoreConstants, public imagesService: ImagesService,
                public videosService: VideosService, private popoverController: PopoverController,
                private pipelineService: PipelineService, public analyzedVideosService : AnalyzedVideosService,) {
        this.segment = 'all';
        this.pipelineService.fetchAllPipelines();
        this.pipelineService.fetchAllTools();
    }

    ngOnInit() {

    }


    async displayFilterPopover(ev: any) {
        const filterPopover = await this.popoverController.create({
            component: MediaFilterComponent,
            cssClass: 'media-filter',
            animated: true,
            translucent: true,
            backdropDismiss: true,
            event: ev,
        });
        await filterPopover.present();
        await filterPopover.onDidDismiss().then(
            data => {
                if (data.data != undefined) {
                    if (data.data.segment != undefined) {
                        this.segment = data.data.segment;
                    }
                }
            }
        );
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

}
