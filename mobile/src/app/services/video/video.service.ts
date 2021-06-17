import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class VideoService {

  constructor() { }

  /**
   * @summary Requests videos from the backend service
   */
  retrieveVideos(resolve, reject) {
    return new Promise((resolve, reject) => {
      resolve(4); reject(3);
    });
  }
}
