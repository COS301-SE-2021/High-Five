import {Component, Input, OnInit} from '@angular/core';
import {Pipeline} from '../../models/pipeline';
import {LoadingController, Platform, PopoverController, ToastController} from '@ionic/angular';
import {AddItemComponent} from '../add-item/add-item.component';
import {PipelineService} from "../../services/pipeline/pipeline.service";

@Component({
  selector: 'app-pipeline',
  templateUrl: './pipeline.component.html',
  styleUrls: ['./pipeline.component.scss'],
})
export class PipelineComponent implements OnInit {
  @Input() pipeline: Pipeline;
  constructor(private platform: Platform, private loadingController: LoadingController, private toastController: ToastController,
              private popoverController: PopoverController, private pipelineService : PipelineService) {
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
    await this.pipelineService.removeTool(this.pipeline.id, [tool]);

  }

  public async onAddTool(tools: string[]) {
    await this.pipelineService.addTool(this.pipeline.id, tools);

  }

  /**
   * A function that will emit an event to indicate that the pipeline should be removed from the parent page, in this
   * case the analytics page
   */
  public async onDeletePipeline() {
    await this.pipelineService.removePipeline(this.pipeline.id);
  }

  public async presentAddToolPopover(ev: any) {
    /**
     * A popover which contains all the tools that the user can add to the current pipeline
     */
    const addToolPopover = await this.popoverController.create({
      component: AddItemComponent,
      event: ev,
      translucent: true,
      /**
       * By filtering the available tools, we ensure that the user cannot add duplicate tools to a pipeline on the
       * frontend (backend validation also exists)
       */
      componentProps: {
        availableItems: this.pipelineService.tools.filter(tool => !this.pipeline.tools.includes(tool)),
        title: "Add Tool"
      }
    });
    await addToolPopover.present();
    await addToolPopover.onDidDismiss().then(
      data => {
        this.onAddTool(data.data.items);
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
    Array.from(document.getElementsByClassName(this.pipeline.id + '-tool-chip') as HTMLCollectionOf<HTMLElement>).forEach(value => {
      value.style.borderColor = '#' + ('000000' +
        Math.floor(0x1000000 * Math.random()).toString(16)).slice(-6);
    });
  }
}
