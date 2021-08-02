import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-fullscreen-image',
  templateUrl: './fullscreen-image.component.html',
  styleUrls: ['./fullscreen-image.component.scss'],
})
export class FullscreenImageComponent implements OnInit {
  @Input() imageSrc: string;

  constructor() {
  }

  ngOnInit() {
  }

}
