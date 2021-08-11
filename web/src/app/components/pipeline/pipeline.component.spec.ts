import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule, LoadingController, Platform, PopoverController, ToastController} from '@ionic/angular';

import {PipelineComponent} from './pipeline.component';
import {PipelinesService} from '../../apis/pipelines.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';

describe('PipelineComponent', () => {
  let component: PipelineComponent;
  let fixture: ComponentFixture<PipelineComponent>;


  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [PipelineComponent],
      imports: [IonicModule.forRoot(), HttpClientTestingModule],
      providers: [Platform,PipelinesService]
    }).compileComponents();

    fixture = TestBed.createComponent(PipelineComponent);
    component = fixture.componentInstance;
    component.pipeline = {
      name: 'test',
      id :'test_id',
      tools: ['Car']
    };
    fixture.detectChanges();
  }));



  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
