import {TestBed} from '@angular/core/testing';

import {WebsocketService} from './websocket.service';
import {SnotifyService, ToastDefaults} from 'ng-snotify';

describe('WebsocketService', () => {
  let service: WebsocketService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [],
      providers: [SnotifyService, {
        provide: 'SnotifyToastConfig',
        useValue: ToastDefaults
      }]
    });
    service = TestBed.inject(WebsocketService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
