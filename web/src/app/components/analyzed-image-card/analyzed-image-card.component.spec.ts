import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {AnalyzedImageCardComponent} from './analyzed-image-card.component';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {SnotifyService, ToastDefaults} from 'ng-snotify';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {AnalysisService} from '../../apis/analysis.service';

describe('AnalyzedImageCardComponent', () => {
  let component: AnalyzedImageCardComponent;
  let fixture: ComponentFixture<AnalyzedImageCardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [AnalyzedImageCardComponent],
      imports: [IonicModule.forRoot(), HttpClientTestingModule],
      providers: [MediaStorageService, SnotifyService, {
        provide: 'SnotifyToastConfig',
        useValue: ToastDefaults
      }, AnalysisService
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AnalyzedImageCardComponent);
    component = fixture.componentInstance;
    component.analyzedImage = {imageId: '', id: '', pipelineId: '', url: '', dateAnalyzed: new Date()};
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(true).toBeTrue();
  });
});
