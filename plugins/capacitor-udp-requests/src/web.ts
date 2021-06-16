import {WebPlugin} from "@capacitor/core";

import type {SendUdpRequestOptions, UdpRequestsPlugin} from './definitions';

export class UdpRequestsWeb extends WebPlugin implements UdpRequestsPlugin {

  async echo(options: { value: string }): Promise<{ value: string }> {
    console.log('ECHO', options);
    return options;
  }

  async sendUdpRequest(options : SendUdpRequestOptions): Promise<{ status : string, responseMessage: string }> {
      console.log("Options ",options);
      throw this.unavailable("The udp requests plugin is not supported on web");
  }
}
