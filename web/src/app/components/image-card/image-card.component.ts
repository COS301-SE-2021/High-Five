import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Image} from '../../models/image';
import {ModalController} from '@ionic/angular';
import {EditPipelineComponent} from '../edit-pipeline/edit-pipeline.component';
import {FullscreenImageComponent} from '../fullscreen-image/fullscreen-image.component';

@Component({
  selector: 'app-image-card',
  templateUrl: './image-card.component.html',
  styleUrls: ['./image-card.component.scss'],
})
export class ImageCardComponent implements OnInit {
  @Input() image: Image;
  @Output() deleteImage: EventEmitter<string> = new EventEmitter<string>();
  public alt = '../../../assists/images/defaultprofile.svg';

  constructor(private modalController: ModalController) {
    // No constructor body needed as properties are retrieved from angular input
  }

  ngOnInit() {
    // No ngOnInit body defined as redux patter has not yet been implemented
  }

  public onDeleteImage() {
    this.deleteImage.emit(this.image.id);
  }

  public analyseImage() {
    this.image.analysed = true;
  }

  public viewAnalysedImage() {
    return; // Todo : show a modal containing the analysed image
  }

  async viewImageFullScreen() {
    const modal = await this.modalController.create({
      component: FullscreenImageComponent,
      cssClass: 'viewFullScreenImage',
      componentProps: {
        imageSrc: this.image.url
      }
    });
    modal.style.backgroundColor = 'rgba(0,0,0,0.85)';
    return modal.present();

  }
}
