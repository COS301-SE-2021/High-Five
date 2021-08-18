import {Component, Input, OnInit} from '@angular/core';
import {AnalyzedImageMetaData} from '../../models/analyzedImageMetaData';

@Component({
  selector: 'app-analyzed-image-card',
  templateUrl: './analyzed-image-card.component.html',
  styleUrls: ['./analyzed-image-card.component.scss'],
})
export class AnalyzedImageCardComponent implements OnInit {

  @Input() analyzedImage: AnalyzedImageMetaData;

  constructor() {
  }

  ngOnInit() {
  }

  async viewImageFullScreen() {
    const newWindow = window.open(this.analyzedImage.url, '_system');
    newWindow.focus();
  }
}
