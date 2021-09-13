import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {UserTool} from '../../models/userTool';
import {MetaData} from '../../models/metaData';

@Injectable({
  providedIn: 'root'
})
export class UserToolsService {
  private readonly _userTools = new BehaviorSubject<UserTool[]>([]);
  // eslint-disable-next-line @typescript-eslint/member-ordering,no-underscore-dangle
  readonly userTools$ = this._userTools.asObservable();

  constructor() {
    this.fetchAllTools();
  }


  get userTools(): UserTool[] {
    // eslint-disable-next-line no-underscore-dangle
    return this._userTools.getValue();
  }

  set userTools(val: UserTool[]) {
    // eslint-disable-next-line no-underscore-dangle
    this._userTools.next(val);
  }


  public async addUserTool(name: string, id: string, onnxModel: any, toolClassFile: any, metaDataType: MetaData, type: string) {
    this.userTools = this.userTools.concat({id, name, toolClassFile, metaDataType, onnxModel, type});
  }


  public async removeUserTool(userToolId: string, serverRemove: boolean = true) {
    this.userTools = this.userTools.filter(p => p.id !== userToolId);

  }


  public async fetchAllTools() {
    //this.tools = await this.pipelinesService.getAllTools().toPromise();
    this.userTools = [{
      id: 'x',
      toolClassFile: 'some',
      name: 'Tool1',
      onnxModel: 'xd',
      metaDataType: {id: 'xd2meta', name: 'boxbox'},
      type: 'analysis'
    }];
  }
}
