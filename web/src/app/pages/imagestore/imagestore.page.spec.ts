import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {ImagestorePage} from './imagestore.page';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {PipelinesService} from '../../apis/pipelines.service';
import {AnalysisService} from '../../apis/analysis.service';

describe('ImagestorePage', () => {
  let component: ImagestorePage;
  let fixture: ComponentFixture<ImagestorePage>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ImagestorePage],
      imports: [IonicModule.forRoot(), HttpClientTestingModule],
      providers: [MediaStorageService, PipelinesService, AnalysisService]
    }).compileComponents();

    fixture = TestBed.createComponent(ImagestorePage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
