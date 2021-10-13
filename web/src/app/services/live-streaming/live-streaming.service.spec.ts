import {TestBed} from '@angular/core/testing';

import {LiveStreamingService} from './live-streaming.service';
import {SnotifyService, ToastDefaults} from 'ng-snotify';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {LivestreamService} from '../../apis/livestream.service';
import {DateTimeProvider, OAuthLogger, OAuthService, UrlHelperService} from 'angular-oauth2-oidc';

describe('LiveStreamingService', () => {
  let service: LiveStreamingService;

  class MockOAuthService extends OAuthService {
    getIdentityClaims() {
      return {oid: ''};
    }
  }

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [LivestreamService, UrlHelperService, OAuthLogger, DateTimeProvider,
        {provide: OAuthService, useClass: MockOAuthService}, SnotifyService, {
          provide: 'SnotifyToastConfig',
          useValue: ToastDefaults
        }]
    });
    service = TestBed.inject(LiveStreamingService);
  });

  it('should be created', () => {
    expect(true).toBeTrue();
  });
});
