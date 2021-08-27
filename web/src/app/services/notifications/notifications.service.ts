import {Injectable} from '@angular/core';
import {Observable, Subject} from 'rxjs';
import {webSocket} from 'rxjs/webSocket';
import {environment} from '../../../environments/environment';
import {SnotifyService} from 'ng-snotify';

@Injectable({
  providedIn: 'root'
})
export class NotificationsService {

  private subject: Subject<MessageEvent>;

  constructor(private snotifyService: SnotifyService) {
    this.subject = webSocket(environment.websocketEndpoint);
    this.subject.subscribe((message) => {
      console.log(message.data);
    });
  }

  private displayNotification(): void {}

}
