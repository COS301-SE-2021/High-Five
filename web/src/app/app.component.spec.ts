import {CUSTOM_ELEMENTS_SCHEMA} from '@angular/core';
import {TestBed, waitForAsync} from '@angular/core/testing';

import {AppComponent} from './app.component';
import {Router} from '@angular/router';
import {DateTimeProvider, OAuthLogger, OAuthService, UrlHelperService} from 'angular-oauth2-oidc';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {SnotifyService, ToastDefaults} from 'ng-snotify';

describe('AppComponent', () => {

  class MockOAuthService extends OAuthService {
    getIdentityClaims() {
      return {oid: ''};
    }
  }

  const routerMock = {
    navigate: jasmine.createSpy('navigate')
  };

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [AppComponent],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
      imports: [HttpClientTestingModule],
      providers: [
        {provide: Router, useValue: routerMock},
        {provide: OAuthService, useClass: MockOAuthService},
        UrlHelperService, OAuthLogger, DateTimeProvider, SnotifyService, {
          provide: 'SnotifyToastConfig',
          useValue: ToastDefaults
        }
      ]
    }).compileComponents();
  }));

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  });
  // TODO: add more tests!

});
