import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {LiveAnalysisFilterComponent} from './live-analysis-filter.component';
import {PipelinesService} from '../../apis/pipelines.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {SnotifyService, ToastDefaults} from 'ng-snotify';

describe('LiveAnalysisFilterComponent', () => {
  let component: LiveAnalysisFilterComponent;
  let fixture: ComponentFixture<LiveAnalysisFilterComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [LiveAnalysisFilterComponent],
      imports: [IonicModule.forRoot(), HttpClientTestingModule],
      providers: [PipelinesService, SnotifyService, {
        provide: 'SnotifyToastConfig',
        useValue: ToastDefaults
      },],
    }).compileComponents();

    fixture = TestBed.createComponent(LiveAnalysisFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(true).toBeTrue();
  });
});
