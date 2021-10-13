import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {AllmediaPage} from './allmedia.page';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {AnalysisService} from '../../apis/analysis.service';
import {SnotifyService, ToastDefaults} from 'ng-snotify';
import {PipelinesService} from '../../apis/pipelines.service';


describe('AllmediaPage', () => {
  let component: AllmediaPage;
  let fixture: ComponentFixture<AllmediaPage>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [AllmediaPage],
      imports: [IonicModule.forRoot(), HttpClientTestingModule],
      providers: [AnalysisService, MediaStorageService, SnotifyService, {
        provide: 'SnotifyToastConfig',
        useValue: ToastDefaults
      }, PipelinesService],
    }).compileComponents();

    fixture = TestBed.createComponent(AllmediaPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
