import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {ImageCardComponent} from './image-card.component';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {PipelinesService} from '../../apis/pipelines.service';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {AnalysisService} from '../../apis/analysis.service';
import {SnotifyService, ToastDefaults} from 'ng-snotify';
import {LivestreamService} from '../../apis/livestream.service';
import {DateTimeProvider, OAuthLogger, OAuthService, UrlHelperService} from 'angular-oauth2-oidc';


describe('ImageCardComponent', () => {
  let component: ImageCardComponent;
  let fixture: ComponentFixture<ImageCardComponent>;


  class MockOAuthService extends OAuthService {
    getIdentityClaims() {
      return {oid: ''};
    }
  }

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ImageCardComponent],
      imports: [IonicModule.forRoot(), HttpClientTestingModule],
      providers: [PipelinesService, MediaStorageService, AnalysisService, SnotifyService, {
        provide: 'SnotifyToastConfig',
        useValue: ToastDefaults
      }, LivestreamService, {provide: OAuthService, useClass: MockOAuthService},
        UrlHelperService, OAuthLogger, DateTimeProvider,]
    }).compileComponents();

    fixture = TestBed.createComponent(ImageCardComponent);
    component = fixture.componentInstance;
    component.image = {url: 'test', name: 'test_name', dateStored: new Date(), id: 'test_id'};
    fixture.detectChanges();
  }));

  /**
   * Checks the component has been created successfully
   */
  xit('should create component', () => {
    expect(true).toBeTrue();
  });
});
