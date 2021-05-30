import {Component, Input, OnInit} from '@angular/core';
import {VideoPreviewData} from "../../pages/videostore/videostore.page";

@Component({
  selector: 'app-videostore-card',
  templateUrl: './videostore-card.component.html',
  styleUrls: ['./videostore-card.component.scss'],
})
export class VideostoreCardComponent implements OnInit {
  @Input() data: VideoPreviewData;  //be specific later

  constructor() { }

  ngOnInit() {}

}
