import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {Tool} from '../../models/tool';
import {ToolsService} from '../../apis/tools.service';
import {SnotifyService} from 'ng-snotify';


@Injectable({
  providedIn: 'root'
})
export class UserToolsService {
  private readonly _userTools = new BehaviorSubject<Tool[]>([]);
  // eslint-disable-next-line @typescript-eslint/member-ordering,no-underscore-dangle
  readonly userTools$ = this._userTools.asObservable();

  private readonly _toolTypes = new BehaviorSubject<string[]>([]);
  // eslint-disable-next-line no-underscore-dangle,@typescript-eslint/member-ordering
  readonly toolTypes$ = this._toolTypes.asObservable();

  constructor(private toolsService: ToolsService, private snotifyService: SnotifyService) {
    this.fetchAllTools();
    this.fetchAllToolTypes();
  }


  get userTools(): Tool[] {
    // eslint-disable-next-line no-underscore-dangle
    return this._userTools.getValue();
  }

  set userTools(val: Tool[]) {
    // eslint-disable-next-line no-underscore-dangle
    this._userTools.next(val);
  }

  get toolTypes(): string[] {
    // eslint-disable-next-line no-underscore-dangle
    return this._toolTypes.getValue();
  }

  set toolTypes(val: string[]) {
    // eslint-disable-next-line no-underscore-dangle
    this._toolTypes.next(val);
  }


  public async addAnalysisTool(classFile: any, model: any, type: string = 'BoxCoordinates', name: string) {
    try {
      this.toolsService.uploadAnalysisToolForm(classFile, model, type, name, 'response').subscribe((res) => {
        if (res.ok) {
          this.userTools = this.userTools.concat(res.body);
          this.snotifyService.success('Successfully added analysis tool : ' + name, 'Tool Addition');
        } else {
          this.snotifyService.error('Could not add tool : ' + name + ' please contact an admin', 'Tool Addition');
        }
      });
    } catch (e) {
      this.snotifyService.error('Could not add tool : ' + name + ' please contact an admin', 'Tool Addition');
    }
  }


  public async addDrawingTool(classFile: any, type: string = 'BoxCoordinates', name: string) {
    try {
      this.toolsService.uploadDrawingToolForm(classFile, type, name, 'response').subscribe((res) => {
        if (res.ok) {
          this.userTools = this.userTools.concat(res.body);
          this.snotifyService.success('Successfully added drawing tool tool : ' + name, 'Tool Addition');
        } else {
          this.snotifyService.error('Could not add tool : ' + name + ' please contact an admin', 'Tool Addition');
        }
      });
    } catch (e) {
      this.snotifyService.error('Could not add tool : ' + name + ' please contact an admin', 'Tool Addition');
    }
  }


  public async removeTool(userToolId: string, type: string, serverRemove: boolean = true) {
    const userTool = this.userTools.find(t => t.toolId === userToolId);
    this.userTools = this.userTools.filter(p => p.toolId !== userToolId);
    if (serverRemove) {
      try {
        this.toolsService.deleteTool({toolId: userToolId, toolType: type}, 'response').subscribe((res) => {
          if (res.ok) {
            this.snotifyService.success('Successfully deleted tool : ' + userTool.toolName, 'Tool Deletion');
          } else {
            this.snotifyService.error('Could not delete : ' + userTool.toolName + ' please contact an admin', 'Tool Deletion');
            this.userTools = [...this.userTools, userTool];
          }
        });
      } catch (e) {
        this.snotifyService.error('Could not delete : ' + userTool.toolName + ' please contact an admin', 'Tool Deletion');
        console.log(e);
        this.userTools = [...this.userTools, userTool];
      }
    }
  }


  public async fetchAllTools() {
    this.toolsService.getTools().subscribe((res) => {
      this.userTools = res.tools;
    });
  }

  public async fetchAllToolTypes() {
    this.toolsService.getToolTypes().subscribe((res) => {
      this.toolTypes = res.toolTypes;
    });
  }

  public  drawingToolCount(toolNames: string[]): number {
    let count  =0;
    for (const toolName of toolNames) {
      if(this.userTools.filter(tool => tool.toolName === toolName)[0].toolType === 'drawing'){
        count++;
      }
    }
    return count;
  }
}
