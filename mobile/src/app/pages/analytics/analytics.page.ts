import { Component, OnInit } from '@angular/core';
import {ScreenSizeServiceService} from '../../services/screen-size-service.service';
import {PipelineModel} from '../../models/pipeline.model';
import {PipelineService} from '../../services/pipeline/pipeline.service';

@Component({
  selector: 'app-analytics',
  templateUrl: './analytics.page.html',
  styleUrls: ['./analytics.page.scss'],
})
export class AnalyticsPage implements OnInit {
  public pipelineData: PipelineModel[];
  public pipelineCount: number;
  public isDesktop: boolean;
  constructor(private screenSizeService: ScreenSizeServiceService) {
    this.screenSizeService.isDesktopView().subscribe(isDesktop=>{
      this.isDesktop = isDesktop;
    });
  }

  ngOnInit() {
  }

}
