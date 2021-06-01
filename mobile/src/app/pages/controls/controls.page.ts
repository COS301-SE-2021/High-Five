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

}
