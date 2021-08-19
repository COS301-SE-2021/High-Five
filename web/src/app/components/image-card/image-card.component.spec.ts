import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {ImageCardComponent} from './image-card.component';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {PipelinesService} from '../../apis/pipelines.service';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {AnalysisService} from '../../apis/analysis.service';


describe('ImageCardComponent', () => {
  let component: ImageCardComponent;
  let fixture: ComponentFixture<ImageCardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ImageCardComponent],
      imports: [IonicModule.forRoot(), HttpClientTestingModule],
      providers: [PipelinesService, MediaStorageService, AnalysisService]
    }).compileComponents();

    fixture = TestBed.createComponent(ImageCardComponent);
    component = fixture.componentInstance;
    component.image = {url: 'test', name: 'test_name', dateStored: new Date(), id: 'test_id'};
    fixture.detectChanges();
  }));

  /**
   * Checks the component has been created successfully
   */
  it('should create component', () => {
    expect(component).toBeTruthy();
  });
});
