import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';

import {WelcomeCardComponent} from './welcome-card.component';
import {DateTimeProvider, OAuthLogger, OAuthService, UrlHelperService} from 'angular-oauth2-oidc';
import {HttpClientTestingModule} from '@angular/common/http/testing';

describe('WelcomeCardComponent', () => {
  let component: WelcomeCardComponent;
  let fixture: ComponentFixture<WelcomeCardComponent>;

  class MockOAuthService extends OAuthService {
    getIdentityClaims() {
      return {oid: ''};
    }
  }

  const msalServiceMock = {
    loginRedirect: jasmine.createSpy('loginRedirect')
  };

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [WelcomeCardComponent],
      imports: [HttpClientTestingModule],
      providers: [
        {provide: OAuthService, useClass: MockOAuthService},
        UrlHelperService, OAuthLogger, DateTimeProvider
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(WelcomeCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(true).toBeTrue();
  });
});
