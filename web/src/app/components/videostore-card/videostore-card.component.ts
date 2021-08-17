import {Component,  Input, OnInit} from '@angular/core';
import {ModalController, Platform, PopoverController} from '@ionic/angular';
import {VideostreamCardComponent} from '../videostream-card/videostream-card.component';
import {VideoMetaData} from '../../models/videoMetaData';
import {VideosService} from "../../services/videos/videos.service";
import {AddItemComponent} from "../add-item/add-item.component";
import {PipelineService} from "../../services/pipeline/pipeline.service";
import {Pipeline} from "../../models/pipeline";
import {AnalyzedVideosService} from "../../services/analyzed-videos/analyzed-videos.service";


@Component({
  selector: 'app-videostore-card',
  templateUrl: './videostore-card.component.html',
  styleUrls: ['./videostore-card.component.scss'],
})
export class VideostoreCardComponent implements OnInit {
  @Input() video: VideoMetaData;

  constructor(public platform: Platform, private modalController: ModalController, private videoService : VideosService,
              private popoverController : PopoverController, private pipelineService : PipelineService, private analyzeVideosService : AnalyzedVideosService) {
  }

  ngOnInit() {
    //Nothing added here yet

  }

  /**
   * This function creates a modal where the recorded drone footage can be
   * replayed to the user.
   */
  async playVideo() {
    const videoModal = await this.modalController.create({
      component: VideostreamCardComponent,
      componentProps: {
        modal: this.modalController,
        videoUrl: this.video.url
      }
    });
    videoModal.style.backgroundColor = 'rgba(0,0,0,0.85)'; //make the background for the modal darker.

    await videoModal.present();
  }

  /**
   * Deletes this video by emitting the deleteVideo event
   */
  async onDeleteVideo() {
    await this.videoService.removeVideo(this.video.id);
  }

  private async analyseVideo(pipelines : string[]){
    const pipelineIds = this.pipelineService.pipelines.filter((pipeline: Pipeline) => {
      return pipelines.filter((pipelineName: string) => {
        return pipelineName == pipeline.name;
      });
    }).map((el :Pipeline)=>{return  el.id});
    for(const pipelineId of pipelineIds){
      await this.analyzeVideosService.analyzeVideo(this.video.id, pipelineId);
    }
  }

  public async showAnalyseVideoPopover(ev: any) {
    /**
     * A popover which contains all the pipelines that the user can analyse the image with
     */
    const pipelinesPopover = await this.popoverController.create({
      component: AddItemComponent,
      event: ev,
      translucent: true,
      componentProps: {
        availableItems: this.pipelineService.pipelines.map(a => a.name),
        title: "Choose pipeline"
      }
    });
    await pipelinesPopover.present();
    await pipelinesPopover.onDidDismiss().then(
      data => {
        if (data.data != undefined) {
          if (data.data.items != undefined) {
            this.analyseVideo(data.data.items);
          }
        }
      }
    );
  }
}
