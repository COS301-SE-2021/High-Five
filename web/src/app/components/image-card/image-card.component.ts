import {Component, Input, OnInit} from '@angular/core';
import {ImageMetaData} from '../../models/imageMetaData';
import {PopoverController} from "@ionic/angular";
import {AddItemComponent} from "../add-item/add-item.component";
import {PipelineService} from "../../services/pipeline/pipeline.service";
import {ImagesService} from "../../services/images/images.service";
import {Pipeline} from "../../models/pipeline";
import {AnalyzedImagesService} from "../../services/analyzed-images/analyzed-images.service";

@Component({
  selector: 'app-image-card',
  templateUrl: './image-card.component.html',
  styleUrls: ['./image-card.component.scss'],
})
export class ImageCardComponent implements OnInit {
  @Input() image: ImageMetaData;
  public alt = '../../../assists/images/defaultprofile.svg';

  constructor(private popoverController: PopoverController, private pipelineService: PipelineService,
              private imagesService: ImagesService, private analyzedImagesService : AnalyzedImagesService) {
    // No constructor body needed as properties are retrieved from angular input
  }

  ngOnInit() {
  }

  public onDeleteImage() {
    this.imagesService.removeImage(this.image.id);
  }

  private analyseImage(pipelines: string[]) {
    const pipelineIds = this.pipelineService.pipelines.filter((pipeline: Pipeline) => {
      return pipelines.filter((pipelineName: string) => {
        return pipelineName == pipeline.name;
      });
    }).map((el :Pipeline)=>{return  el.id});
    for(const pipelineId of pipelineIds){
      this.analyzedImagesService.analyzeImage(this.image.id,pipelineId);
    }
  }

  public viewAnalysedImage() {
    return; // Todo : show a new tab containing the analysed image
  }

  async showAnalyseImagePopover(ev: any) {
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
            this.analyseImage(data.data.items);
          }
        }
      }
    );
  }

  async viewImageFullScreen() {
    const newWindow = window.open(this.image.url, '_system');
    newWindow.focus();
  }
}
