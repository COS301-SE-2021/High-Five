import {Component, Input, OnInit} from '@angular/core';
import {Pipeline} from '../../models/pipeline';
import {LoadingController, Platform, PopoverController, ToastController} from '@ionic/angular';
import {AddItemComponent} from '../add-item/add-item.component';
import {PipelineService} from '../../services/pipeline/pipeline.service';
import {UserToolsService} from '../../services/user-tools/user-tools.service';

@Component({
  selector: 'app-pipeline',
  templateUrl: './pipeline.component.html',
  styleUrls: ['./pipeline.component.scss'],
})
export class PipelineComponent implements OnInit {
  @Input() pipeline: Pipeline;

  constructor(private platform: Platform, private loadingController: LoadingController, private toastController: ToastController,
              private popoverController: PopoverController, private pipelineService: PipelineService,
              private userToolsService: UserToolsService) {
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


  /**
   * Will remove a tool from the current pipeline, by sending a request using the injected pieplineService
   *
   * @param tool a string representing a tool which should be removed from the pipeline
   */
  public async onRemoveTool(tool: string) {
    if (this.pipeline.tools.indexOf(tool) === this.pipeline.tools.length - 1) {
      if (this.userToolsService.drawingToolCount([tool])>0 ) {
        await this.toastController.create({
          message: `A pipeline's last tool must be a drawing tool, cannot remove tool`,
          duration: 2000,
          translucent: true,
          position: 'bottom'
        }).then((toast) => {
          toast.present();
        });
      } else {
        await this.pipelineService.removeTool(this.pipeline.id, [tool]);

      }
    } else {
      await this.pipelineService.removeTool(this.pipeline.id, [tool]);
    }


  }

  /**
   * Will send a request to add tools to the pipeline using the injected pipelineService
   *
   * @param tools, an array of strings representing the tools which must be added to the pipeline
   */
  public async onAddTool(tools: string[]) {
    this.pipelineService.addTool(this.pipeline.id, tools).then(() => {
      this.updateToolColours();
    });

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
        availableItems: this.userToolsService.userTools.map(t => t.toolName).filter(tool => !this.pipeline.tools.includes(tool)),
        title: 'Add Tool'
      }
    });
    await addToolPopover.present();
    await addToolPopover.onDidDismiss().then(
      data => {
        if (data.data !== undefined) {
          if (data.data.items !== undefined) {
            this.loadingController.create({
              spinner: 'dots',
              animated: true,
              message: 'Adding tools'
            }).then((e: HTMLIonLoadingElement) => {
              e.present();
              this.onAddTool(data.data.items);
              e.dismiss();
            });

          }
        }
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
      //SonarCloud sees this as a security threat. As it only affects colours, it can ignore this.
      value.style.borderColor = '#' + ('000000' +
        Math.floor(0x1000000 * Math.random()).toString(16)).slice(-6); //NOSONAR
    });
  }
}
