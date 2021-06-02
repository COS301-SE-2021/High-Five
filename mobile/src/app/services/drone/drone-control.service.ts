import { Injectable } from '@angular/core';
//import {SendUdpRequestOptions, UdpRequests} from 'capacitor-udp-requests';
// const udp = UdpRequests;
// export default  udp;
@Injectable({
  providedIn: 'root'
})
export class DroneControlService {

  constructor() { }

  // connect(){
  //   const landOptions: SendUdpRequestOptions = {
  //     port: '8889',
  //     address: '192.168.10.1',
  //     payload: 'command'
  //   };
  //   udp.sendUdpRequest(landOptions);
  // }
  //
  // takeoff(){
  //   const landOptions: SendUdpRequestOptions = {
  //     port: '8889',
  //     address: '192.168.10.1',
  //     payload: 'takeoff'
  //   };
  //   udp.sendUdpRequest(landOptions);
  // }
  //
  // land(){
  //   const landOptions: SendUdpRequestOptions = {
  //     port: '8889',
  //     address: '192.168.10.1',
  //     payload: 'land'
  //   };
  //   udp.sendUdpRequest(landOptions);
  // }
}
