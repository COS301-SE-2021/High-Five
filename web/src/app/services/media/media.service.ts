import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Endpoints} from '../../../constants/endpoints';

@Injectable({
  providedIn: 'root'
})
export class MediaService {

  constructor(private http: HttpClient, private endpoints: Endpoints) { }

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

  storeImage(imageName: string, imageData: any, subscription: any) {
    const formData = new FormData();
    formData.append('file', imageData);
    const headers = new HttpHeaders({
      // eslint-disable-next-line @typescript-eslint/naming-convention
      'Content-Type' : 'multipart/form-data'
    });
    this.http.post<any>(this.endpoints.labels.storeImage, formData, {}).subscribe(data => {
      subscription(data);
    });
  }
}
