import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {ImageMetaData} from '../../models/imageMetaData';

@Component({
  selector: 'app-image-card',
  templateUrl: './image-card.component.html',
  styleUrls: ['./image-card.component.scss'],
})
export class ImageCardComponent implements OnInit {
  @Input() image: ImageMetaData;
  @Output() deleteImage: EventEmitter<string> = new EventEmitter<string>();
  public alt = '../../../assists/images/defaultprofile.svg';

  constructor() {
    // No constructor body needed as properties are retrieved from angular input
  }

  ngOnInit() {
    // No ngOnInit body defined as redux patter has not yet been implemented
  }

  public onDeleteImage() {
    this.deleteImage.emit(this.image.id);
  }

  public analyseImage() {
    // Nothing added here
  }

  public viewAnalysedImage() {
    return; // Todo : show a modal containing the analysed image
  }

  async viewImageFullScreen() {
    const newWindow = window.open(this.image.url,'_system');
    newWindow.focus();

  }
}
