import {TestBed} from '@angular/core/testing';

import {ImagesService} from './images.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {SnotifyService, ToastDefaults} from 'ng-snotify';

describe('ImagesService', () => {
  let service: ImagesService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [MediaStorageService, SnotifyService, {provide: 'SnotifyToastConfig', useValue: ToastDefaults}],
    });
    service = TestBed.inject(ImagesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
