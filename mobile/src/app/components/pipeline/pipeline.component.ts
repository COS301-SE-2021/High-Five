import {Component, Input, OnInit, Optional} from '@angular/core';
import {ToolsetConstants} from '../../../constants/toolset-constants';
import {PipelineModel} from '../../models/pipeline.model';
import {PipelineService} from '../../services/pipeline/pipeline.service';
import {AddPipelineComponent} from '../add-pipeline/add-pipeline.component';
import {ModalController} from '@ionic/angular';

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
    for (let i = 0; i < 10; i++) {
      pipelineService.addPipeline(x);
    }

  }

  ngOnInit() {}

  async deletePipeline(){

  }
  async openCreatePipelineModal(){

  }
}
