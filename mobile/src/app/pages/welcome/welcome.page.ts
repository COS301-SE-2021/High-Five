import {Component, OnInit, Renderer2} from '@angular/core';

@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.page.html',
  styleUrls: ['./welcome.page.scss'],
})
export class WelcomePage implements OnInit {

  hills: any;
  constructor(public renderer: Renderer2) { }

  // startAnimations(){
  //   this.hills = document.querySelector('#Group');
  //   this.hills.forEach((hill)=>{
  //     this.renderer.setStyle(hill,'transition','1.5s ease-in-out infinite alternate');
  //   });
  // }

  ngOnInit() {
  }

}
