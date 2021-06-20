import {Component, Input, OnInit} from '@angular/core';
import {ModalController} from '@ionic/angular';
import {ToolsetConstants} from '../../../constants/toolset-constants';
import {PipelineService} from '../../services/pipeline/pipeline.service';
import {PipelineModel} from '../../models/pipeline.model';

@Component({
  selector: 'app-add-pipeline',
  templateUrl: './add-pipeline.component.html',
  styleUrls: ['./add-pipeline.component.scss'],
})
export class AddPipelineComponent implements OnInit {
  selectedTools: boolean[];
  pipelineName: string;
  constructor(public constants: ToolsetConstants, public pipelineService: PipelineService) {
    this.selectedTools = new Array<boolean>(this.constants.labels.tools.length);
  }

  addPipeline(){
    this.pipelineService.addPipeline(this.selectedTools,this.pipelineName);

  }
  ngOnInit() {}

}
