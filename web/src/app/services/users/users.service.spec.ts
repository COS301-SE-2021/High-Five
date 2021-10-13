import {TestBed} from '@angular/core/testing';

import {UsersService} from './users.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {UserService} from '../../apis/user.service';
import {SnotifyService, ToastDefaults} from 'ng-snotify';
import {PipelinesService} from '../../apis/pipelines.service';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {AnalysisService} from '../../apis/analysis.service';
import {ToolsService} from '../../apis/tools.service';

describe('UsersService', () => {
  let service: UsersService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [UserService, SnotifyService, {
        provide: 'SnotifyToastConfig',
        useValue: ToastDefaults
      }, PipelinesService, MediaStorageService, AnalysisService, ToolsService]
    });
    service = TestBed.inject(UsersService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
