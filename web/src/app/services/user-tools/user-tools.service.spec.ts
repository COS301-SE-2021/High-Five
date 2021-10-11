import {TestBed} from '@angular/core/testing';

import {UserToolsService} from './user-tools.service';
import {SnotifyService, ToastDefaults} from 'ng-snotify';
import {ToolsService} from '../../apis/tools.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';

describe('UserToolsService', () => {
  let service: UserToolsService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [SnotifyService, {
        provide: 'SnotifyToastConfig',
        useValue: ToastDefaults
      }, ToolsService],
    });
    service = TestBed.inject(UserToolsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
