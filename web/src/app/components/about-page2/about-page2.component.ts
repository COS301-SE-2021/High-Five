import {AfterContentChecked, Component, OnInit, ViewChild} from '@angular/core';
import {AnimationOptions} from "ngx-lottie";
import {SwiperComponent} from "swiper/angular";
import {SwiperOptions} from "swiper";
import SwiperCore, {Pagination, Mousewheel, Navigation, Autoplay } from 'swiper/core';
import {ModalController} from "@ionic/angular";

SwiperCore.use([Pagination, Mousewheel, Navigation, Autoplay]);
@Component({
  selector: 'app-about-page2',
  templateUrl: './about-page2.component.html',
  styleUrls: ['./about-page2.component.scss'],
})
export class AboutPage2Component implements OnInit, AfterContentChecked {

  lottieConfig: AnimationOptions ={
    path:'/assets/lottie-animations/lf30_editor_5gpajdty.json'
  }

  lottieMulticastConfig: AnimationOptions= {
    path:'/assets/lottie-animations/multicast.json',
  }

  lottieMobileAppConfig: AnimationOptions= {
    path:'/assets/lottie-animations/72680-mobile-app.json',
  }

  @ViewChild('swiper2') swiper2: SwiperComponent;
  swiperConfig2: SwiperOptions= {
    slidesPerView: 3,
    spaceBetween: 10,
    pagination: true,
    mousewheel: true,
    speed: 800,
    allowTouchMove: true,
    slideToClickedSlide: true,
    autoplay: {
      delay: 12000,
      disableOnInteraction: true,
    }
  }

  constructor(private modalController : ModalController) { }

  ngOnInit() {}

  ngAfterContentChecked(): void {
    if(this.swiper2){
      this.swiper2.updateSwiper({});
    }
  }

  async displayModal(){
    // Display modal containing info here
  }

}
