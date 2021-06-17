import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Endpoints} from '../../../constants/endpoints';

@Injectable({
  providedIn: 'root'
})
export class VideouploadService {

  constructor(private http: HttpClient, private endpoints: Endpoints) { }

  /**
   * Retrieves a list of videos from the backend API
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
}
