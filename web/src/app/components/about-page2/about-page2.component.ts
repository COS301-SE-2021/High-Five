import { Component, OnInit } from '@angular/core';
import {AnimationOptions} from "ngx-lottie";

@Component({
  selector: 'app-about-page2',
  templateUrl: './about-page2.component.html',
  styleUrls: ['./about-page2.component.scss'],
})
export class AboutPage2Component implements OnInit {

  lottieConfig: AnimationOptions ={
    path:'/assets/lottie-animations/lf30_editor_5gpajdty.json'
  }
  constructor() { }

  ngOnInit() {}

}
