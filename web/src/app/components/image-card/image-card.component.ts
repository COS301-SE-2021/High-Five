import {Component, Input, OnInit} from '@angular/core';
import {ImageMetaData} from '../../models/imageMetaData';
import {PopoverController} from '@ionic/angular';
import {AddItemComponent} from '../add-item/add-item.component';
import {PipelineService} from '../../services/pipeline/pipeline.service';
import {ImagesService} from '../../services/images/images.service';
import {Pipeline} from '../../models/pipeline';
import {WebsocketService} from '../../services/websocket/websocket.service';

@Component({
  selector: 'app-image-card',
  templateUrl: './image-card.component.html',
  styleUrls: ['./image-card.component.scss'],
})
export class ImageCardComponent implements OnInit {
  @Input() image: ImageMetaData;
  public alt = 'assets/images/defaultprofile.svg';

  constructor(private popoverController: PopoverController, private pipelineService: PipelineService,
              private imagesService: ImagesService, private webSocketService: WebsocketService) {
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
  public async viewImageFullScreen() {
    const newWindow = window.open(this.image.url, '_system');
    newWindow.focus();
  }


  /**
   * This function will iterate through the passed in pipelines, get their ids and then use the pipelineService
   * to send a request to analyze the present image with (the image property)
   *
   * @param pipelines, a string of pipeline names , which the user would like to analyze the image with
   * @private
   */
  private async analyseImage(pipelines: string[]) {
    for (const pipelineName of pipelines) {
      const selectedPipeline = this.pipelineService.pipelines.find((pipeline: Pipeline) => pipeline.name === pipelineName);
      await this.webSocketService.analyzeImage(this.image.id, selectedPipeline.id);
    }
  }
}
