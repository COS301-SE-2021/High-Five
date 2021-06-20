import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { PipelineComponent } from './pipeline.component';

const mockPipelinesService = jasmine.createSpyObj('PipelinesService', [ 'addedNewPipelineWatch','setNewPipelineAdded']);
mockPipelinesService.addedNewPipelineWatch.and.callFake(
  (func)=>func()
);
mockPipelinesService.setNewPipelineAdded.and.callFake(
  (func)=>func()
);

describe('PipelineComponent', () => {
  let component: PipelineComponent;
  let fixture: ComponentFixture<PipelineComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ PipelineComponent],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(PipelineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  describe('general',()=> {

    it('should create', () => {
      expect(component).toBeTruthy();
    });

    it('generated components',()=>{

      component.pipelines = [{name: 'testName', id: 'testID', tools: ['tool1','tool2']}];
      fixture.detectChanges();
      const fName = fixture.debugElement.nativeElement.querySelector('ion-card-title[id="pipelineName-0"]').innerHTML;
      expect(fName).toBe('testName');
      const fTool1 = fixture.debugElement.nativeElement.querySelector('ion-text[id="pipelineTool-0-0"]').innerHTML;
      expect(fTool1).toBe('tool1');
      const fTool2 = fixture.debugElement.nativeElement.querySelector('ion-text[id="pipelineTool-0-1"]').innerHTML;
      expect(fTool2).toBe('tool2');

    });
  });
});
