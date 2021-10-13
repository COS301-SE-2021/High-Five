import {TestBed} from '@angular/core/testing';

import {UserPreferencesService} from './user-preferences.service';
import {SnotifyService, ToastDefaults} from 'ng-snotify';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {PipelinesService} from '../../apis/pipelines.service';

describe('UserPreferencesService', () => {
  let service: UserPreferencesService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [SnotifyService, {
        provide: 'SnotifyToastConfig',
        useValue: ToastDefaults
      }, PipelinesService],
    });
    service = TestBed.inject(UserPreferencesService);
  });

  it('should be created', () => {
    expect(true).toBeTrue();
  });
});
