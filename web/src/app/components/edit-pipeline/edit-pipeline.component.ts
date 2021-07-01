import {Component, Input, OnInit} from '@angular/core';
import {LoadingController, ModalController, ToastController} from '@ionic/angular';
import {Pipeline} from '../../models/pipeline';
import {ToolsetConstants} from '../../../constants/toolset-constants';
import {PipelinesService} from '../../apis/pipelines.service';
import {RemoveToolsRequest} from '../../models/removeToolsRequest';
import {AddToolsRequest} from '../../models/addToolsRequest';

@Component({
  selector: 'app-edit-pipeline',
  templateUrl: './edit-pipeline.component.html',
  styleUrls: ['./edit-pipeline.component.scss'],
})
export class EditPipelineComponent implements OnInit {
  /**
   * The below inputs are all pased in from the pipeline component's function which opens the modal
   */
  @Input() modalController: ModalController;
  @Input() pipeline: Pipeline;
  @Input() loadingController: LoadingController;
  public selectedTools: boolean[];
  public tools: string[];

  /**
   *
   * @param toolsetConstants constants injected that represents all the tools and constants used by the pipeline and tools
   * @param pipelinesService the services which OpenAPI generated, which we will use to send requests
   * @param toastController toast controller that will enable us to display unobtrusive informative messages
   */
  constructor(private toolsetConstants: ToolsetConstants, private pipelinesService: PipelinesService,
              private toastController: ToastController) {
    this.tools = this.toolsetConstants.labels.tools;
    this.selectedTools = new Array<boolean>(this.tools.length);
  }

  ngOnInit() {
  }


  /**
   * This function closes the modal without performing any function or updating the pipeline's values, all changes will
   * be discarded when this function is called (if there were any changes made in the frontend to the tools in the
   * pipeline)
   */
  dismiss() {
    this.modalController.dismiss({
      dismissed: true
    });
  }

}


