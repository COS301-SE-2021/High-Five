import {Component, OnInit, Optional} from '@angular/core';
import {ToolsetConstants} from '../../../constants/toolset-constants';

@Component({
  selector: 'app-pipeline',
  templateUrl: './pipeline.component.html',
  styleUrls: ['./pipeline.component.scss'],
})
export class PipelineComponent implements OnInit {

  constructor(@Optional() public toolsetName: string, public constants: ToolsetConstants ) {
    if (!toolsetName){
      this.toolsetName='Default Toolset Name';
    }
  }

  ngOnInit() {}

}
