import {Component, ElementRef, Input, OnInit, ViewChild} from '@angular/core';
import {VideoPreviewData} from "../../pages/videostore/videostore.page";
import {Platform} from "@ionic/angular";

@Component({
  selector: 'app-videostore-card',
  templateUrl: './videostore-card.component.html',
  styleUrls: ['./videostore-card.component.scss'],
})
export class VideostoreCardComponent implements OnInit {
  @Input() data: VideoPreviewData;  //be specific later
  @ViewChild('desktopImage') desktopImage : HTMLImageElement;
  @ViewChild('mobileImage') mobileImage : HTMLImageElement;

  private mobile : boolean;

  constructor(public platform: Platform) { }

  ngOnInit() {
  }

  private isMobile() : boolean {
    return this.platform.width() < 700;
  }

  onResize(event) {
    if (this.isMobile()) {
      if (this.desktopImage != undefined && this.mobileImage != undefined) {
        this.desktopImage.style.display = "none";
        this.mobileImage.style.display = "block";
      }
    } else {
      if (this.desktopImage != undefined && this.mobileImage != undefined) {
        this.desktopImage.style.display = "block";
        this.mobileImage.style.display = "none";
      }
    }
  }
  onLoad(event) {
    if (this.isMobile()) {
      if (this.desktopImage != undefined && this.mobileImage != undefined) {
        this.desktopImage.style.display = "none";
        this.mobileImage.style.display = "block";
      }
    } else {
      if (this.desktopImage != undefined && this.mobileImage != undefined) {
        this.desktopImage.style.display = "block";
        this.mobileImage.style.display = "none";
      }
    }
  }
}
