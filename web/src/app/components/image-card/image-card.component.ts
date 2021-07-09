import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Image} from '../../models/image';

@Component({
  selector: 'app-image-card',
  templateUrl: './image-card.component.html',
  styleUrls: ['./image-card.component.scss'],
})
export class ImageCardComponent implements OnInit {
  @Input() image: Image;
  @Output() deleteImage:  EventEmitter<string> = new EventEmitter<string>();
  public alt = '../../../assists/images/defaultprofile.svg';
  constructor() { }

  ngOnInit() {}

}
