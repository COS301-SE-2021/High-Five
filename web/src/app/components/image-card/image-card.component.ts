import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {ImageMetaData} from '../../models/imageMetaData';
import {PopoverController} from "@ionic/angular";
import {AddItemComponent} from "../add-item/add-item.component";
import {Pipeline} from "../../models/pipeline";
import {PipelinesService} from "../../apis/pipelines.service";

@Component({
  selector: 'app-image-card',
  templateUrl: './image-card.component.html',
  styleUrls: ['./image-card.component.scss'],
})
export class ImageCardComponent implements OnInit {
  @Input() image: ImageMetaData;
  @Output() deleteImage: EventEmitter<string> = new EventEmitter<string>();
  public alt = '../../../assists/images/defaultprofile.svg';
  private pipelines : Pipeline[] = []
  constructor(private popoverController : PopoverController, private pipelinesService : PipelinesService) {
    // No constructor body needed as properties are retrieved from angular input
  }

  ngOnInit() {
    this.loadPipelines();
  }

  public onDeleteImage() {
    this.deleteImage.emit(this.image.id);
  }

  private async loadPipelines(){
    this.pipelinesService.getPipelines().subscribe((res)=>{
      this.pipelines= res.pipelines;
    });
  }

  public analyseImage(pipelines : string[]) {
    // Send requests to analyse image with passed in pipeline here.
  }

  public viewAnalysedImage() {
    return; // Todo : show a new tab containing the analysed image
  }

  public async showAnalyseImagePopover(ev : any){
    /**
     * A popover which contains all the pipelines that the user can analyse the image with
     */
    const addToolPopover = await this.popoverController.create({
      component: AddItemComponent,
      event: ev,
      translucent: true,
      componentProps: {
        availableItems: this.pipelines.map(a => a.name),
        title: "Choose pipeline"
      }
    });
    await addToolPopover.present();
    await addToolPopover.onDidDismiss().then(
      data => {
        if(data.data){
          if(data.data.items){
            this.analyseImage(data.data.items);
          }
        }
      }
    );
  }

  async viewImageFullScreen() {
    const newWindow = window.open(this.image.url,'_system');
    newWindow.focus();
  }
}
