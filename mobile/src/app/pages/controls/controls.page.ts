import { Component, OnInit } from '@angular/core';
import {DroneControlsService} from '../../services/drone-controls.service';
@Component({
  selector: 'app-controls',
  templateUrl: './controls.page.html',
  styleUrls: ['./controls.page.scss'],
})
export class ControlsPage implements OnInit {
  private drone;
  constructor(private droneControlsService: DroneControlsService) {
    this.drone=droneControlsService;
  }

  ngOnInit() {
  }

  demo(){
    this.drone.forward(100);
    this.drone.left(100);
  }

  takeoff(){
    this.drone.takeoff();
  }

  land(){
    this.drone.land();
  }

  connect(){
    this.drone.connect();
  }

}
