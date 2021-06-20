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
  }

  ngOnInit() {}

  async deletePipeline(){

  }
}
