import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Endpoints} from '../../../constants/endpoints';

@Injectable({
  providedIn: 'root'
})
export class VideouploadService {

  constructor(private http: HttpClient, private endpoints: Endpoints) { }

  /**
   * Retrieves a list of videos from the backend API and runs a callback function, passing in the
   * retrieved data.
   *
   * @param subscription A function to run once the request is completed
   */
  getAllVideos(subscription: any) {
    const headers = new HttpHeaders({
      // eslint-disable-next-line @typescript-eslint/naming-convention
      'Content-Type' : 'text/plain',
    });
    this.http.post<any>(this.endpoints.labels.getAllVideos,{},{headers}).subscribe(data => {
      subscription(data);
    });
  }

  storeVideo(vidName: string, vidData: any, subscription: any) {
    const formData = new FormData();
    formData.append('file', vidData);
    const headers = new HttpHeaders({
      // eslint-disable-next-line @typescript-eslint/naming-convention
      'Content-Type' : 'multipart/form-data'
    });
    this.http.post<any>(this.endpoints.labels.storeVideo, formData, {}).subscribe(data => {
      subscription(data);
    });
  }

  deleteVideo(vidId: string, subscription: any) {
    const formData = new FormData();
    formData.append('id', vidId);

    const headers = new HttpHeaders({
      // eslint-disable-next-line @typescript-eslint/naming-convention
      'Content-Type' : 'application/json'
    });

    this.http.post<any>(this.endpoints.labels.deleteVideo, JSON.stringify({id: vidId}), {headers}).subscribe(data => {
      subscription(data);
    });
  }
}
