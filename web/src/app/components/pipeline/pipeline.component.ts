import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Pipeline} from '../../models/pipeline';
import {LoadingController, Platform, PopoverController, ToastController} from '@ionic/angular';
import {PipelinesService} from '../../apis/pipelines.service';
import {DeletePipelineRequest} from '../../models/deletePipelineRequest';
import {RemoveToolsRequest} from '../../models/removeToolsRequest';
import {AddToolComponent} from '../add-tool/add-tool.component';

@Component({
  selector: 'app-pipeline',
  templateUrl: './pipeline.component.html',
  styleUrls: ['./pipeline.component.scss'],
})
export class PipelineComponent implements OnInit {
  @Input() pipeline: Pipeline;
  @Input() availableTools: string[];
  @Output() deletePipeline: EventEmitter<string> = new EventEmitter<string>(); // Will send the id of the pipeline
  @Output() removeTool: EventEmitter<Pipeline> = new EventEmitter<Pipeline>(); // Will send through a new pipeline object
  constructor(private platform: Platform, private pipelinesService: PipelinesService,
              private loadingController: LoadingController, private toastController: ToastController,
              private popoverController: PopoverController) {
  }

  ngOnInit() {
    /**
     * The below allows us to add custom colours to the ionic chips, you cant use color ={{color}} as by default the
     * chips only accept the ionic colours (primary, secondary, warning, etc.)
     */
    this.platform.ready().then(() => {
      this.updateToolColours();
    });
  }

  public async onRemoveTool(tool: string) {
    if (this.pipeline.tools.length === 1) {
      await this.onDeletePipeline();
    } else {
      const removeToolRequest: RemoveToolsRequest = {
        pipelineId: this.pipeline.id,
        tools: [tool]
      };
      try {
        this.pipeline.tools = this.pipeline.tools.filter(t => t !== tool); // Optimistic update
        this.pipelinesService.removeTools(removeToolRequest).subscribe(response => {
          /**
           * If the request to the backend fails, re add the tool to the pipeline on frontend, 'undo-ing' the optimistic
           * update
           */
          if (!response.success) {
            this.pipeline.tools.push(tool);
            /**
             * Toast to indicatet that the tool could not successfully be removed
             */
            this.toastController.create({
              message: 'Removal of tool from pipeline failed, request to backend failed ',
              duration: 2000
            }).then(t => {
              t.present();
            });
          } else {
            this.removeTool.emit(this.pipeline);
          }
        });
      } catch (e) {
        const toast = await this.toastController.create({
          message: 'Removal of tool from pipeline failed',
          duration: 2000
        });
        await toast.present();
      }
    }

  }

  public async onAddTool(tools: string[]) {
    this.pipeline.tools = this.pipeline.tools.concat(tools);

    this.pipelinesService.addTools({
      pipelineId: this.pipeline.id,
      tools: this.pipeline.tools
    }).subscribe(
      () => {
        this.platform.ready().then(() => {
          this.updateToolColours();
        });
      }
    );
  }

  /**
   * A function that will emit an event to indicate that the pipeline should be removed from the parent page, in this
   * case the analytics page
   */
  public async onDeletePipeline() {
    /**
     * Display a loading animation
     */
    const loading = await this.loadingController.create({
      spinner: 'dots',
      animated: true,
    });
    await loading.present();

    const deletePipelineRequest: DeletePipelineRequest = {
      pipelineId: this.pipeline.id
    };
    try {
      this.pipelinesService.deletePipeline(deletePipelineRequest).subscribe(response => {
        /**
         * Resolve the loading animation once a response has been received from the backend
         */
        loading.dismiss();
        /**
         * Emit the event to indicate that the pipeline should be removed, optimistic updates aren't used here
         * since a disappearing pipeline that reappears shortly again if the request fails, will provide for an
         * unwanted user experience
         */
        this.deletePipeline.emit(this.pipeline.id);
      });
    } catch (e) {
      const toast = await this.toastController.create({
        message: 'Deletion of pipeline failed, please contact the developers',
        duration: 2000
      });
      await loading.dismiss();
      await toast.present();
    }
  }

  public async presentAddToolPopover(ev: any) {
    /**
     * A popover which contains all the tools that the user can add to the current pipeline
     */
    const addToolPopover = await this.popoverController.create({
      component: AddToolComponent,
      event: ev,
      translucent: true,
      /**
       * By filtering the available tools, we ensure that the user cannot add duplicate tools to a pipeline on the
       * frontend (backend validation also exists)
       */
      componentProps: {
        availableTools: this.availableTools.filter(tool => !this.pipeline.tools.includes(tool))
      }
    });
    await addToolPopover.present();
    await addToolPopover.onDidDismiss().then(
      data => {
        this.onAddTool(data.data.tools);
      }
    );
  }

  /**
   * Function that will assign each tool in he pipeline component a different colour, used to make the ui more
   * interesting
   *
   * @private
   */
  private updateToolColours() {
    const temp = Array.from(document.getElementsByClassName(this.pipeline.id + '-tool-chip') as HTMLCollectionOf<HTMLElement>);
    temp.forEach(value => {
      value.style.borderColor = '#' + ('000000' +
        Math.floor(0x1000000 * Math.random()).toString(16)).slice(-6);
    });
  }
}
