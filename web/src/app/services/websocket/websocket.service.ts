import {Injectable} from '@angular/core';
import {WebSocketSubject} from 'rxjs/internal-compatibility';
import {environment} from '../../../environments/environment';
import {webSocket} from 'rxjs/webSocket';
import {JsonObject} from '@angular/compiler-cli/ngcc/src/packages/entry_point';
import {SnotifyService} from 'ng-snotify';


@Injectable({
  providedIn: 'root'
})
export class WebsocketService {
  private socket: WebSocketSubject<any>;

  constructor(private ngSnotify: SnotifyService) {
    this.socket = webSocket({
      url: environment.websocketEndpoint, closeObserver: {
        // eslint-disable-next-line prefer-arrow/prefer-arrow-functions
        next(closeEvent) {
          const customError = {code: 6666, reason: 'Custom reason message'};
          console.log(`code: ${customError.code}, reason: ${customError.reason}`);
        }
      }
    });
    this.socket.subscribe((msg) => {
      console.log(msg);
      this.handleMessage(msg);
    });
    this.sendMessage({
      // eslint-disable-next-line @typescript-eslint/naming-convention
      Request: 'Synchronize'
    });
  }

  public sendMessage(message: JsonObject) {
    message.Authorization = JSON.parse(sessionStorage.getItem(sessionStorage.key(0))).secret;
    this.socket.next(message);
  }

  private handleMessage(msg: JsonObject) {
    if (msg.type === 'info') {
      // @ts-ignore
      this.ngSnotify.info(msg.message, msg.title);
    } else if (msg.type === 'error') {
      // @ts-ignore
      this.ngSnotify.error(msg.message, msg.title);

    } else if (msg.type === 'success') {
      // @ts-ignore
      this.ngSnotify.success(msg.message, msg.title);
    }
  }


}
