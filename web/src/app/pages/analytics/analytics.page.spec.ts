import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {AnalyticsPage} from './analytics.page';
import {PipelinesService} from '../../apis/pipelines.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {SnotifyService, ToastDefaults} from 'ng-snotify';

describe('AnalyticsPage', () => {
  let component: AnalyticsPage;
  let fixture: ComponentFixture<AnalyticsPage>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [AnalyticsPage],
      imports: [IonicModule.forRoot(), HttpClientTestingModule],
      providers: [PipelinesService, SnotifyService, {
        provide: 'SnotifyToastConfig',
        useValue: ToastDefaults
      }]
    }).compileComponents();

    fixture = TestBed.createComponent(AnalyticsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
