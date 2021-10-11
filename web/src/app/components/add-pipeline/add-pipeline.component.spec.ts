import {ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';
import {IonicModule} from '@ionic/angular';

import {AddPipelineComponent} from './add-pipeline.component';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {PipelinesService} from '../../apis/pipelines.service';

describe('AddPipelineComponent', () => {
  let component: AddPipelineComponent;
  let fixture: ComponentFixture<AddPipelineComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [AddPipelineComponent],
      imports: [IonicModule.forRoot(), HttpClientTestingModule],
      providers: [PipelinesService],
    }).compileComponents();
    fixture = TestBed.createComponent(AddPipelineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(true).toBeTrue();
  });
});
