import {Component, OnInit} from '@angular/core';
import {PopoverController} from '@ionic/angular';
import {PipelineService} from '../../services/pipeline/pipeline.service';
import {Pipeline} from '../../models/pipeline';

@Component({
  selector: 'app-live-analysis-filter',
  templateUrl: './live-analysis-filter.component.html',
  styleUrls: ['./live-analysis-filter.component.scss'],
})
export class LiveAnalysisFilterComponent implements OnInit {


  constructor(private popoverController: PopoverController, public pipelineService: PipelineService) {
  }

  ngOnInit() {
  }

  /**
   *
   * @param chosenPipeline
   */
  public async onClick(chosenPipeline: Pipeline) {
    await this.popoverController.dismiss({pipeline: chosenPipeline});
  }

}
