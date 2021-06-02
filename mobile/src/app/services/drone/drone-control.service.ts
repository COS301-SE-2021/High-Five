import { Injectable } from '@angular/core';
import {SendUdpRequestOptions, UdpRequests} from '../../../../../plugins/capacitor-udp-requests/src';
const udp = UdpRequests;
export default  udp;
@Injectable({
  providedIn: 'root'
})
export class DroneControlService {

  constructor() { }

  connect(){
    const landOptions: SendUdpRequestOptions = {
      port: '8889',
      address: '192.168.10.1',
      payload: 'command'
    };
    udp.sendUdpRequest(landOptions);
  }

  takeoff(){
    const landOptions: SendUdpRequestOptions = {
      port: '8889',
      address: '192.168.10.1',
      payload: 'takeoff'
    };
    udp.sendUdpRequest(landOptions);
  }

  land(){
    const landOptions: SendUdpRequestOptions = {
      port: '8889',
      address: '192.168.10.1',
      payload: 'land'
    };
    udp.sendUdpRequest(landOptions);
  }

  forward(distance){
    const landOptions: SendUdpRequestOptions = {
      port: '8889',
      address: '192.168.10.1',
      payload: 'forward ' + distance.toString()
    };
    udp.sendUdpRequest(landOptions);
  }

  backward(distance){
    const landOptions: SendUdpRequestOptions = {
      port: '8889',
      address: '192.168.10.1',
      payload: 'back ' + distance.toString()
    };
    udp.sendUdpRequest(landOptions);
  }

  up(distance){
    const landOptions: SendUdpRequestOptions = {
      port: '8889',
      address: '192.168.10.1',
      payload: 'up ' + distance.toString()
    };
    udp.sendUdpRequest(landOptions);
  }

  down(distance){
    const landOptions: SendUdpRequestOptions = {
      port: '8889',
      address: '192.168.10.1',
      payload: 'down ' + distance.toString()
    };
    udp.sendUdpRequest(landOptions);
  }

  left(distance){
    const landOptions: SendUdpRequestOptions = {
      port: '8889',
      address: '192.168.10.1',
      payload: 'left ' + distance.toString()
    };
    udp.sendUdpRequest(landOptions);
  }

  right(distance){
    const landOptions: SendUdpRequestOptions = {
      port: '8889',
      address: '192.168.10.1',
      payload: 'right ' + distance.toString()
    };
    udp.sendUdpRequest(landOptions);
  }

  enableStream(){
    const landOptions: SendUdpRequestOptions = {
      port: '8889',
      address: '192.168.10.1',
      payload: 'streamon'
    };
    udp.sendUdpRequest(landOptions);
  }

  disableStream(){
    const landOptions: SendUdpRequestOptions = {
      port: '8889',
      address: '192.168.10.1',
      payload: 'streamoff'
    };
    udp.sendUdpRequest(landOptions);
  }

  rotateRight(degrees){
    const landOptions: SendUdpRequestOptions = {
      port: '8889',
      address: '192.168.10.1',
      payload: 'cw ' + degrees.toString()
    };
    udp.sendUdpRequest(landOptions);
  }

  rotateLeft(degrees){
    const landOptions: SendUdpRequestOptions = {
      port: '8889',
      address: '192.168.10.1',
      payload: 'ccw ' + degrees.toString()
    };
    udp.sendUdpRequest(landOptions);
  }


  displayVideoStream(){
    udp.getVideoStream();
  }

}
