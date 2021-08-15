import { Component, OnInit} from '@angular/core';
import {AnimationOptions} from "ngx-lottie";
import SwiperCore, {Pagination, Mousewheel, Navigation, Autoplay } from 'swiper/core';
import {ModalController} from "@ionic/angular";
import {MoreInfoComponent} from "../more-info/more-info.component";

SwiperCore.use([Pagination, Mousewheel, Navigation, Autoplay]);
@Component({
  selector: 'app-about-page2',
  templateUrl: './about-page2.component.html',
  styleUrls: ['./about-page2.component.scss'],
})
export class AboutPage2Component implements OnInit{

  lottieConfig: AnimationOptions ={
    path:'/assets/lottie-animations/lf30_editor_5gpajdty.json'
  }

  lottieMulticastConfig: AnimationOptions= {
    path:'/assets/lottie-animations/multicast.json',
  }

  lottieMobileAppConfig: AnimationOptions= {
    path:'/assets/lottie-animations/72680-mobile-app.json',
  }

  lottieLatencyConfig: AnimationOptions={
    path:'/assets/lottie-animations/speed.json',
  }

  constructor(private modalController : ModalController) { }

  ngOnInit() {}


  async displayModal(){
    const modal = await this.modalController.create({
      component: MoreInfoComponent,
      showBackdrop: true,
      animated: true,
      backdropDismiss: true,
      componentProps: {
        title: 'Some Title',
        description : 'Some Description'
      }
    });
    await modal.present();
  }

}
