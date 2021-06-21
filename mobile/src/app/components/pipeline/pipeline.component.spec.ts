import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { PipelineComponent } from './pipeline.component';
import {PipelinesService} from '../../apis/pipelines.service';
import {BehaviorSubject, Observable} from 'rxjs';
import {GetPipelinesResponse} from '../../models/getPipelinesResponse';
import {distinctUntilChanged} from 'rxjs/operators';

const mockPipelinesService = jasmine.createSpyObj('PipelinesService', [ 'getPipelines','deletePipeline']);
mockPipelinesService.getPipelines.and.callFake(
  ()=>((new BehaviorSubject(false).asObservable().pipe(distinctUntilChanged())))
);
mockPipelinesService.deletePipeline.and.callFake(
  (piplineId='id')=>((new BehaviorSubject(false).asObservable().pipe(distinctUntilChanged())))
);

describe('PipelineComponent', () => {
  let component: PipelineComponent;
  let fixture: ComponentFixture<PipelineComponent>;

  const setBeforeEach=(imports, providers) =>{
    beforeEach(waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [ PipelineComponent ],
        imports,
        providers
      }).compileComponents();

      fixture = TestBed.createComponent(PipelineComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
    }));
  };

  describe('general',()=> {

    setBeforeEach([IonicModule.forRoot()],[{provide: PipelinesService, useValue: mockPipelinesService}]);

    it('should create', () => {
      expect(component).toBeTruthy();
    });

    it('generated components',()=>{

      component.pipelines = [{name: 'testName', id: 'testID', tools: ['tool1','tool2']}];
      fixture.detectChanges();
      const fName = fixture.debugElement.nativeElement.querySelector('ion-card-title[id="pipelineName-0"]').innerHTML;
      expect(fName.trim()).toBe('testName');
      const fTool1 = fixture.debugElement.nativeElement.querySelector('ion-text[id="pipelineTool-0-0"]').innerHTML;
      expect(fTool1.trim()).toBe('Object Identification');
      const fTool2 = fixture.debugElement.nativeElement.querySelector('ion-text[id="pipelineTool-0-1"]').innerHTML;
      expect(fTool2.trim()).toBe('Object Counting');

    });
  });
});