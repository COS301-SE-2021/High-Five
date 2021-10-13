import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {LivePage} from './live.page';
import {LivestreamService} from '../../apis/livestream.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {SnotifyService, ToastDefaults} from 'ng-snotify';
import {DateTimeProvider, OAuthLogger, OAuthService, UrlHelperService} from 'angular-oauth2-oidc';
import {PipelinesService} from '../../apis/pipelines.service';

describe('LivePage', () => {
  let component: LivePage;
  let fixture: ComponentFixture<LivePage>;

  class MockOAuthService extends OAuthService {
    getIdentityClaims() {
      return {oid: ''};
    }
  }

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [LivePage],
      imports: [IonicModule.forRoot(), HttpClientTestingModule],
      providers: [LivestreamService, SnotifyService, {
        provide: 'SnotifyToastConfig',
        useValue: ToastDefaults
      }, {provide: OAuthService, useClass: MockOAuthService},
        UrlHelperService, OAuthLogger, DateTimeProvider, PipelinesService],
    }).compileComponents();

    fixture = TestBed.createComponent(LivePage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
