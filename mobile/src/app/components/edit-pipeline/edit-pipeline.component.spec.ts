import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { EditPipelineComponent } from './edit-pipeline.component';
import {BehaviorSubject} from 'rxjs';
import {distinctUntilChanged} from 'rxjs/operators';
import {PipelinesService} from '../../apis/pipelines.service';

const mockPipeline = jasmine.createSpyObj('Pipeline',[], {name: 'test', id: 'testid', tools: ['hello', 'test']});

const mockPipelinesService = jasmine.createSpyObj('PipelinesService', [ 'getPipelines','deletePipeline']);
mockPipelinesService.getPipelines.and.callFake(
  ()=>((new BehaviorSubject(false).asObservable().pipe(distinctUntilChanged())))
);
mockPipelinesService.deletePipeline.and.callFake(
  (piplineId='id')=>((new BehaviorSubject(false).asObservable().pipe(distinctUntilChanged())))
);

describe('EditPipelineComponent', () => {
  let component: EditPipelineComponent;
  let fixture: ComponentFixture<EditPipelineComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EditPipelineComponent ],
      imports: [IonicModule.forRoot()
      ],
      providers: [{provide: PipelinesService, useValue: mockPipelinesService}]
    }).compileComponents();

    fixture = TestBed.createComponent(EditPipelineComponent);
    component = fixture.componentInstance;
    component.pipeline = mockPipeline;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
