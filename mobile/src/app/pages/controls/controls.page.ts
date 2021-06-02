import {Component, OnInit, ViewChild} from '@angular/core';
import {DroneControlService} from '../../services/drone/drone-control.service';
import { JoystickEvent, NgxJoystickComponent } from '../../../../../plugins/ngx-joystick';
import { JoystickManagerOptions, JoystickOutputData } from '../../../../../plugins/nipplejs';

@Component({
  selector: 'app-controls',
  templateUrl: './controls.page.html',
  styleUrls: ['./controls.page.scss'],
})
export class ControlsPage implements OnInit {

  @ViewChild('dynamicJoystick') dynamicJoystick: NgxJoystickComponent;
  title = 'ngx-joystick-demo';

  dynamicJoystickOptions: JoystickManagerOptions = {
    mode: 'static',
    color: 'white',
    multitouch: true
  };

  staticOutputData: JoystickOutputData;
  semiOutputData: JoystickOutputData;
  dynamicOutputData: JoystickOutputData;

  directionStatic: string;
  interactingStatic: boolean;

  constructor(private controls: DroneControlService) {
  }

  ngOnInit() {
  }

  onStartStatic(event: JoystickEvent) {
    this.interactingStatic = true;
  }

  onEndStatic(event: JoystickEvent) {
    this.interactingStatic = false;
  }

  onMoveStatic(event: JoystickEvent) {
    this.staticOutputData = event.data;
  }

  onPlainUpStatic(event: JoystickEvent) {
    this.directionStatic = 'UP';
  }

  onPlainDownStatic(event: JoystickEvent) {
    this.directionStatic = 'DOWN';
  }

  onPlainLeftStatic(event: JoystickEvent) {
    this.directionStatic = 'LEFT';
  }

  onPlainRightStatic(event: JoystickEvent) {
    this.directionStatic = 'RIGHT';
  }

  onMoveSemi(event: JoystickEvent) {
    this.semiOutputData = event.data;
  }

  onMoveDynamic(event: JoystickEvent) {
    this.dynamicOutputData = event.data;
  }

  // connect(){
  //   this.controls.connect();
  // }
  //
  //
  // // demo(){
  // //
  // // }
  //
  // takeoff(){
  //   this.controls.takeoff();
  // }
  //
  // land(){
  //   this.controls.land();
  // }
  //
  // up(){
  //   this.controls.up(50);
  // }
  // down()
  // {
  //   this.controls.down(50);
  // }
  // forward(){
  //   this.controls.forward(50);
  // }
  // backward(){
  //   this.controls.backward(50);
  // }
  // left(){
  //   this.controls.left(50);
  // }
  // right(){
  //   this.controls.right(50);
  // }
  //
  // //Rotate left
  // rtl(){
  //
  // }
  // //Rotate right
  // rtr(){
  //
  // }


}
