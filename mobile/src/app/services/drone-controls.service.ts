import { Injectable } from '@angular/core';
import {Drone} from 'dji-tello';

@Injectable({
  providedIn: 'root'
})
export class DroneControlsService {

  constructor() {
    Drone.connect();
  }

  takeoff(){
    Drone.takeoff();
  }

  land(){
    Drone.land();
  }

  stop(){
    Drone.event('stop');
  }

  forward(x){
    Drone.forward(x);
  }

  backward(x){
    Drone.back(x);
  }

  left(x){
    Drone.left(x);
  }

  right(x){
    Drone.right(x);
  }

  up(x){
    Drone.event('up '+x);
  }

  down(x){
    Drone.event('down '+x);
  }

  rotateLeft(x){
    Drone.event('ccw '+x);
  }

  rotateRight(x){
    Drone.event('cw '+x);
  }

}
