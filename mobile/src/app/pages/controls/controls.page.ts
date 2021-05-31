import { Component, OnInit } from '@angular/core';
import {StreamingMedia, StreamingVideoOptions} from "@ionic-native/streaming-media/ngx";

@Component({
  selector: 'app-controls',
  templateUrl: './controls.page.html',
  styleUrls: ['./controls.page.scss'],
})
export class ControlsPage implements OnInit {

  constructor(private streamingMedia: StreamingMedia) {  }


  startVideo(){
    let options: StreamingVideoOptions = {
      successCallback: () => { console.log('Video played') },
      errorCallback: (e) => { console.log('Error streaming') },
      orientation: 'portrait',
    };
    this.streamingMedia.playVideo('/media/jeanre/Additional/University/2021/Semester 1/COS 314/Lectures/L6.mp4',options);
  }

  ngOnInit() {
  }

}
