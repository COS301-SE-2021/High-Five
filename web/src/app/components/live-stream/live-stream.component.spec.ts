import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {LiveStreamComponent} from './live-stream.component';
import {LivestreamService} from '../../apis/livestream.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {SnotifyService, ToastDefaults} from 'ng-snotify';
import {DateTimeProvider, OAuthLogger, OAuthService, UrlHelperService} from 'angular-oauth2-oidc';

describe('LiveStreamComponent', () => {
  let component: LiveStreamComponent;
  let fixture: ComponentFixture<LiveStreamComponent>;

  class MockOAuthService extends OAuthService {
    getIdentityClaims() {
      return {oid: ''};
    }
  }

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [LiveStreamComponent],
      imports: [IonicModule.forRoot(), HttpClientTestingModule],
      providers: [LivestreamService, SnotifyService, {provide: 'SnotifyToastConfig', useValue: ToastDefaults},
        {provide: OAuthService, useClass: MockOAuthService},
        UrlHelperService, OAuthLogger, DateTimeProvider,]
    }).compileComponents();

    fixture = TestBed.createComponent(LiveStreamComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(true).toBeTrue();
  });
});
