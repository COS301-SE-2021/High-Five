import {Component, Input, OnInit, Optional} from '@angular/core';
import {ToolsetConstants} from '../../../constants/toolset-constants';
import {PipelineService} from '../../services/pipeline/pipeline.service';


@Component({
  selector: 'app-pipeline',
  templateUrl: './pipeline.component.html',
  styleUrls: ['./pipeline.component.scss'],
})
export class PipelineComponent implements OnInit {

  constructor(public constants: ToolsetConstants, public  pipelineService: PipelineService) {
  }

  ngOnInit() {}

  async deletePipeline(){

  }
}
