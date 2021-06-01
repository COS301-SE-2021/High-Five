import {Component, ElementRef, Input, OnInit, ViewChild} from '@angular/core';
import {VideoPreviewData} from "../../pages/videostore/videostore.page";
import {ModalController, Platform} from "@ionic/angular";
import {VideostreamCardComponent} from "../videostream-card/videostream-card.component";

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

  constructor(public platform: Platform, private modal: ModalController) { }

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

  async playVideo() {
    console.log("playing video")
    const videoModal = await this.modal.create({
      component: VideostreamCardComponent
    })

    videoModal.style.backgroundColor = "rgba(0,0,0,0.85)"

    await videoModal.present();
  }
}
