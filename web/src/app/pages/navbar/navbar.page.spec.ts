import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {AngularDelegate, IonicModule, PopoverController} from '@ionic/angular';

import {NavbarPage} from './navbar.page';
import {Router} from '@angular/router';
import {DateTimeProvider, OAuthLogger, OAuthService, UrlHelperService} from 'angular-oauth2-oidc';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {UserService} from '../../apis/user.service';
import {SnotifyService, ToastDefaults} from 'ng-snotify';
import {PipelinesService} from '../../apis/pipelines.service';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {AnalysisService} from '../../apis/analysis.service';
import {ToolsService} from '../../apis/tools.service';
import {LivestreamService} from '../../apis/livestream.service';


describe('NavbarPage', () => {
  let component: NavbarPage;
  let fixture: ComponentFixture<NavbarPage>;

  class MockOAuthService extends OAuthService {
    getIdentityClaims() {
      return {oid: ''};
    }
  }

  const routerMock = {
    navigate: jasmine.createSpy('navigate')
  };

  const msalServiceMock = {
    logout: jasmine.createSpy('logout')
  };

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [NavbarPage],
      imports: [HttpClientTestingModule],
      providers: [
        {provide: Router, useValue: routerMock},
        PopoverController, AngularDelegate, {provide: OAuthService, useClass: MockOAuthService},
        UrlHelperService, OAuthLogger, DateTimeProvider, UserService,
        SnotifyService, {
          provide: 'SnotifyToastConfig',
          useValue: ToastDefaults
        }, PipelinesService, MediaStorageService, AnalysisService, ToolsService, LivestreamService
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(NavbarPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
