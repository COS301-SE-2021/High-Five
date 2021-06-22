import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { AddPipelineComponent } from './add-pipeline.component';
import {PipelinesService} from '../../apis/pipelines.service';
import {BehaviorSubject} from 'rxjs';
import {distinctUntilChanged} from 'rxjs/operators';

const mockPipelinesService = jasmine.createSpyObj('PipelinesService', [ 'getPipelines','deletePipeline']);
mockPipelinesService.getPipelines.and.callFake(
  ()=>((new BehaviorSubject(false).asObservable().pipe(distinctUntilChanged())))
);
mockPipelinesService.deletePipeline.and.callFake(
  (piplineId='id')=>((new BehaviorSubject(false).asObservable().pipe(distinctUntilChanged())))
);

describe('AddPipelineComponent', () => {
  let component: AddPipelineComponent;
  let fixture: ComponentFixture<AddPipelineComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ AddPipelineComponent ],
      imports: [IonicModule.forRoot()],
      providers: [{provide: PipelinesService, useValue: mockPipelinesService}]
    }).compileComponents();

    fixture = TestBed.createComponent(AddPipelineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
