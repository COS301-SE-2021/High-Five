import {Component, OnInit, Renderer2} from '@angular/core';

@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.page.html',
  styleUrls: ['./welcome.page.scss'],
})
export class WelcomePage implements OnInit {

  hills: any;
  constructor(public renderer: Renderer2) { }

  ngOnInit() {
    //Nothing added here yet

  }

}
