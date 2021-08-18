import {Component, Input, OnInit} from '@angular/core';
import {ImageMetaData} from '../../models/imageMetaData';
import {PopoverController} from '@ionic/angular';
import {AddItemComponent} from '../add-item/add-item.component';
import {PipelineService} from '../../services/pipeline/pipeline.service';
import {ImagesService} from '../../services/images/images.service';
import {Pipeline} from '../../models/pipeline';
import {AnalyzedImagesService} from '../../services/analyzed-images/analyzed-images.service';

@Component({
  selector: 'app-image-card',
  templateUrl: './image-card.component.html',
  styleUrls: ['./image-card.component.scss'],
})
export class ImageCardComponent implements OnInit {
  @Input() image: ImageMetaData;
  public alt = 'assets/images/defaultprofile.svg';

  constructor(private popoverController: PopoverController, private pipelineService: PipelineService,
              private imagesService: ImagesService, private analyzedImagesService: AnalyzedImagesService) {
    // No constructor body needed as properties are retrieved from angular input
  }

  ngOnInit() {
  }


  /**
   * Function that deletes the image from the user's account
   */
  public onDeleteImage() {
    this.imagesService.removeImage(this.image.id);
  }


  public viewAnalysedImage() {
    return; // Todo : show a new tab containing the analysed image
  }

  /**
   * A popover which contains all the pipelines that the user can analyse the image with
   *
   * @param ev the event needed to display the popover
   */
  public async showAnalyseImagePopover(ev: any) {

    const pipelinesPopover = await this.popoverController.create({
      component: AddItemComponent,
      event: ev,
      translucent: true,
      componentProps: {
        availableItems: this.pipelineService.pipelines.map(a => a.name),
        title: 'Choose pipeline'
      }
    });
    await pipelinesPopover.present();
    await pipelinesPopover.onDidDismiss().then(
      data => {
        if (data.data !== undefined) {
          if (data.data.items !== undefined) {
            this.analyseImage(data.data.items);
          }
        }
      }
    );
  }

  /**
   * Opens the image in a new tab , to view fullscreen
   */
  public async viewImageFullScreen(){
    const newWindow = window.open(this.image.url, '_system');
    newWindow.focus();
  }

  private analyseImage(pipelines: string[]) {
    const pipelineIds = this.pipelineService.pipelines.filter((pipeline: Pipeline) => pipelines.filter(
      (pipelineName: string) => pipelineName === pipeline.name)).map((el: Pipeline) => el.id);
    for (const pipelineId of pipelineIds) {
      this.analyzedImagesService.analyzeImage(this.image.id, pipelineId);
    }
  }
}
