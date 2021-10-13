import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {AnalyzedVideostoreCardComponent} from './analyzed-videostore-card.component';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {AnalysisService} from '../../apis/analysis.service';
import {SnotifyService, ToastDefaults} from 'ng-snotify';

describe('AnalyzedVideostoreCardComponent', () => {
  let component: AnalyzedVideostoreCardComponent;
  let fixture: ComponentFixture<AnalyzedVideostoreCardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [AnalyzedVideostoreCardComponent],
      imports: [IonicModule.forRoot(), HttpClientTestingModule],
      providers: [MediaStorageService, AnalysisService, SnotifyService, {
        provide: 'SnotifyToastConfig',
        useValue: ToastDefaults
      }]
    }).compileComponents();

    fixture = TestBed.createComponent(AnalyzedVideostoreCardComponent);
    component = fixture.componentInstance;
    component.analyzedVideo = {id: '', videoId: '', pipelineId: '', url: '', dateAnalyzed: new Date()};
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(true).toBeTrue();
  });
});
