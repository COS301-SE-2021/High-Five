import {Component, Input, OnInit, Optional} from '@angular/core';
import {ToolsetConstants} from '../../../constants/toolset-constants';
import {PipelineModel} from '../../models/pipeline.model';
import {PipelineService} from '../../services/pipeline/pipeline.service';

@Component({
  selector: 'app-pipeline',
  templateUrl: './pipeline.component.html',
  styleUrls: ['./pipeline.component.scss'],
})
export class PipelineComponent implements OnInit {

  constructor(public constants: ToolsetConstants, public  pipelineService: PipelineService) {
    const x = new PipelineModel('XD');
    x.selectedTools = [true,true,false];
    x.pipelineId= 123123;
    pipelineService.addPipeline(x);
  }

  ngOnInit() {}

  deletePipeline(): void{

  }
}
