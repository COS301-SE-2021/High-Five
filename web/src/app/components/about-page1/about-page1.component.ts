import { Component, OnInit } from '@angular/core';
import {AnimationOptions} from "ngx-lottie";

@Component({
  selector: 'app-about-page1',
  templateUrl: './about-page1.component.html',
  styleUrls: ['./about-page1.component.scss'],
})
export class AboutPage1Component implements OnInit {

  lottieConfig: AnimationOptions ={
    path:'/assets/lottie-animations/67783-drones-isometric-lottie-animation.json'
  }
  constructor() { }

  ngOnInit() {}

}
