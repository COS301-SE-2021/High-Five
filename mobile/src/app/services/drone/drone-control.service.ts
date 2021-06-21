import { Injectable } from '@angular/core';
import {SendUdpRequestOptions, UdpRequests} from 'capacitor-udp-requests/src';
const udp = UdpRequests;
export default  udp;
@Injectable({
  providedIn: 'root'
})
export class DroneControlService {
  private portNumber = '8889';
  private telloIp =  '192.168.10.1';
  constructor() { }

  connect(){
    const landOptions: SendUdpRequestOptions = {
      port: this.portNumber,
      address: this.telloIp,
      payload: 'command'
    };
    udp.sendUdpRequest(landOptions);
  }

  takeoff(){
    const landOptions: SendUdpRequestOptions = {
      port: this.portNumber,
      address: this.telloIp,
      payload: 'takeoff'
    };
    udp.sendUdpRequest(landOptions);
  }

  land(){
    const landOptions: SendUdpRequestOptions = {
      port: this.portNumber,
      address: this.telloIp,
      payload: 'land'
    };
    udp.sendUdpRequest(landOptions);
  }

  forward(distance){
    const landOptions: SendUdpRequestOptions = {
      port: this.portNumber,
      address: this.telloIp,
      payload: 'forward ' + distance.toString()
    };
    udp.sendUdpRequest(landOptions);
  }

  backward(distance){
    const landOptions: SendUdpRequestOptions = {
      port: this.portNumber,
      address: this.telloIp,
      payload: 'back ' + distance.toString()
    };
    udp.sendUdpRequest(landOptions);
  }

  up(distance){
    const landOptions: SendUdpRequestOptions = {
      port: this.portNumber,
      address: this.telloIp,
      payload: 'up ' + distance.toString()
    };
    udp.sendUdpRequest(landOptions);
  }

  down(distance){
    const landOptions: SendUdpRequestOptions = {
      port: this.portNumber,
      address: this.telloIp,
      payload: 'down ' + distance.toString()
    };
    udp.sendUdpRequest(landOptions);
  }

  left(distance){
    const landOptions: SendUdpRequestOptions = {
      port: this.portNumber,
      address: this.telloIp,
      payload: 'left ' + distance.toString()
    };
    udp.sendUdpRequest(landOptions);
  }

  right(distance){
    const landOptions: SendUdpRequestOptions = {
      port: this.portNumber,
      address: this.telloIp,
      payload: 'right ' + distance.toString()
    };
    udp.sendUdpRequest(landOptions);
  }

  rotateLeft(angle){
    const landOptions: SendUdpRequestOptions = {
      port: this.portNumber,
      address: this.telloIp,
      payload: 'ccw ' + angle.toString()
    };
    udp.sendUdpRequest(landOptions);
  }

  rotateRight(angle){
    const landOptions: SendUdpRequestOptions = {
      port: this.portNumber,
      address: this.telloIp,
      payload: 'cw ' + angle.toString()
    };
    udp.sendUdpRequest(landOptions);
  }

  enableStream(){
    const landOptions: SendUdpRequestOptions = {
      port: this.portNumber,
      address: this.telloIp,
      payload: 'streamon'
    };
    udp.sendUdpRequest(landOptions);
  }

  disableStream(){
    const landOptions: SendUdpRequestOptions = {
      port: this.portNumber,
      address: this.telloIp,
      payload: 'streamoff'
    };
    udp.sendUdpRequest(landOptions);
  }


  displayVideoStream(){
    udp.getVideoStream();
  }

}
