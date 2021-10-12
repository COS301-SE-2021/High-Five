import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {AccountPopoverComponent} from './account-popover.component';
import {RouterTestingModule} from '@angular/router/testing';
import {ToolsService} from '../../apis/tools.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {SnotifyService, ToastDefaults} from 'ng-snotify';
import {DateTimeProvider, OAuthLogger, OAuthService, UrlHelperService} from 'angular-oauth2-oidc';

describe('AccountPopoverComponent', () => {
  let component: AccountPopoverComponent;
  let fixture: ComponentFixture<AccountPopoverComponent>;

  class MockOAuthService extends OAuthService {
    getIdentityClaims() {
      return {oid: ''};
    }
  }

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [AccountPopoverComponent],
      imports: [IonicModule.forRoot(), RouterTestingModule, HttpClientTestingModule],
      providers: [ToolsService, SnotifyService, {
        provide: 'SnotifyToastConfig',
        useValue: ToastDefaults
      }, {provide: OAuthService, useClass: MockOAuthService},
        UrlHelperService, OAuthLogger, DateTimeProvider]
    }).compileComponents();

    fixture = TestBed.createComponent(AccountPopoverComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(true).toBeTrue();
  });
});
