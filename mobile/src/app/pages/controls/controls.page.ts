import { Component, OnInit } from '@angular/core';
import {DroneControlService} from '../../services/drone/drone-control.service';

@Component({
  selector: 'app-controls',
  templateUrl: './controls.page.html',
  styleUrls: ['./controls.page.scss'],
})
export class ControlsPage implements OnInit {

  constructor(private controls: DroneControlService) {

  }

  connect(){
    this.controls.connect();
  }

  ngOnInit() {

  }

  // demo(){
  //
  // }

  takeoff(){
    this.controls.takeoff();
  }

  land(){
    this.controls.land();
  }

  up(){
    this.controls.up(50);
  }
  down()
  {
    this.controls.down(50);
  }
  forward(){
    this.controls.forward(50);
  }
  backward(){
    this.controls.backward(50);
  }
  left(){
    this.controls.left(50);
  }
  right(){
    this.controls.right(50);
  }

  //Rotate left
  rtl(){

  }
  //Rotate right
  rtr(){

  }


}
