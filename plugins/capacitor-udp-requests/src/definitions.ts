export interface UdpRequestsPlugin {
  echo(options: { value: string }): Promise<{ value: string }>; 
  sendUdpRequest(options : SendUdpRequestOptions):Promise<{status: string, responseMessage : string}>;
  getVideoStream():Promise<{status: string, responseMessage : string}>;
  stopVideoStream():Promise<{status: string, responseMessage : string}>;
}

export interface SendUdpRequestOptions{
  port : string,
  address : string,
  payload: string
}
