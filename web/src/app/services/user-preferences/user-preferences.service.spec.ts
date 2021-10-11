import {TestBed} from '@angular/core/testing';

import {UserPreferencesService} from './user-preferences.service';
import {SnotifyService, ToastDefaults} from 'ng-snotify';

describe('UserPreferencesService', () => {
  let service: UserPreferencesService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [],
      providers: [SnotifyService, {
        provide: 'SnotifyToastConfig',
        useValue: ToastDefaults
      }],
    });
    service = TestBed.inject(UserPreferencesService);
  });

  it('should be created', () => {
    expect(true).toBeTrue();
  });
});
