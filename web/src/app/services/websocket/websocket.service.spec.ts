import {TestBed} from '@angular/core/testing';

import {WebsocketService} from './websocket.service';
import {SnotifyService, ToastDefaults} from 'ng-snotify';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {AnalysisService} from '../../apis/analysis.service';
import {LivestreamService} from '../../apis/livestream.service';
import {DateTimeProvider, OAuthLogger, OAuthService, UrlHelperService} from 'angular-oauth2-oidc';

describe('WebsocketService', () => {
  let service: WebsocketService;

  class MockOAuthService extends OAuthService {
    getIdentityClaims() {
      return {oid: ''};
    }
  }

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [SnotifyService, {
        provide: 'SnotifyToastConfig',
        useValue: ToastDefaults
      }, MediaStorageService, AnalysisService, LivestreamService, {provide: OAuthService, useClass: MockOAuthService},
        UrlHelperService, OAuthLogger, DateTimeProvider]
    });
    service = TestBed.inject(WebsocketService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
