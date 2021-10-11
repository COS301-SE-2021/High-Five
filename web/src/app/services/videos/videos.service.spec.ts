import {TestBed} from '@angular/core/testing';

import {VideosService} from './videos.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {SnotifyService, ToastDefaults} from 'ng-snotify';

describe('VideosService', () => {
  let service: VideosService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [MediaStorageService, SnotifyService, {
        provide: 'SnotifyToastConfig',
        useValue: ToastDefaults
      }]
    });
    service = TestBed.inject(VideosService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
