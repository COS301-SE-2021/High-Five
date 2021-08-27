import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {User} from '../../models/user';
import {UserService} from '../../apis/user.service';

@Injectable({
  providedIn: 'root'
})
export class UsersService {


  private readonly _users = new BehaviorSubject<User[]>([]);
  // eslint-disable-next-line @typescript-eslint/member-ordering,no-underscore-dangle
  readonly users$ = this._users.asObservable();
  private isAdmin = false;

  constructor(private userService: UserService) {
    this.queryIsAdmin();
  }


  get users(): User[] {
    // eslint-disable-next-line no-underscore-dangle
    return this._users.getValue();
  }

  set users(val: User[]) {
    // eslint-disable-next-line no-underscore-dangle
    this._users.next(val);
  }

  getIsAdmin(): boolean {
    return this.isAdmin;
  }

  public async purgeMedia(id: string) {
    if (this.isAdmin) {
      this.userService.deleteMedia({id});
    }
  }

  public async upgradeToAdmin(id: string) {
    if (this.isAdmin) {
      this.userService.upgradeToAdmin({id});
    }
  }


  // /**
  //  * Function adds a pipeline by sending a request using the OpenAPI generate PipelinesService
  //  *
  //  * @param name a string representing the name of the pipeline
  //  * @param tools an array of string representing the tools of the pipeline
  //  */
  // async addPipeline(name: string, tools: string[]) {
  //   try {
  //     await this.pipelinesService.createPipeline({pipeline: {name, tools}}).toPromise();
  //     await this.fetchAllPipelines();
  //   } catch (e) {
  //     console.log(e);
  //   }
  // }
  //
  // /**
  //  *
  //  * @param pipelineId the id of the pipeline which to remove (string)
  //  * @param serverRemove boolean, if true  will send a request to the backend to remove the pipeline, otherwise
  //  * the pipeline will only be removed locally, by default this parameter is set to true
  //  */
  // public async removePipeline(pipelineId: string, serverRemove: boolean = true) {
  //   const pipeline = this.pipelines.find(p => p.id === pipelineId);
  //   this.pipelines = this.pipelines.filter(p => p.id !== pipelineId);
  //   if (serverRemove) {
  //     try {
  //       await this.pipelinesService.deletePipeline({pipelineId}).toPromise();
  //     } catch (e) {
  //       console.error(e);
  //       this.pipelines = [...this.pipelines, pipeline];
  //     }
  //   }
  // }
  //
  // public async addTool(id: string, tools: string[]) {
  //   const pipeline = this.pipelines.find(p => p.id === id);
  //   console.log('Old ');
  //   if (pipeline) {
  //     const index = this.pipelines.indexOf(pipeline);
  //
  //     this.pipelines[index] = {
  //       ...pipeline,
  //       tools: pipeline.tools.concat(tools)
  //     };
  //
  //     this.pipelines = [...this.pipelines];
  //
  //     try {
  //       await this.pipelinesService.addTools({pipelineId: id, tools}).toPromise();
  //     } catch (e) {
  //       console.error(e);
  //       this.pipelines[index] = {
  //         ...pipeline,
  //         tools: pipeline.tools
  //       };
  //     }
  //   }
  // }
  //
  // public async removeTool(id: string, tools: string[]) {
  //   const pipeline = this.pipelines.find(p => p.id === id);
  //   if (pipeline) {
  //     const index = this.pipelines.indexOf(pipeline);
  //
  //     this.pipelines[index] = {
  //       ...pipeline,
  //       tools: pipeline.tools.filter((e) => tools.some((f) => e !== f))
  //     };
  //
  //     this.pipelines = [...this.pipelines];
  //
  //     try {
  //       await this.pipelinesService.removeTools({pipelineId: id, tools}).toPromise();
  //     } catch (e) {
  //       console.error(e);
  //       this.pipelines[index] = {
  //         ...pipeline,
  //         tools: pipeline.tools
  //       };
  //     }
  //   }
  // }\
  private async queryIsAdmin() {
    this.userService.isAdmin().subscribe((value) => {
      this.isAdmin = value.isAdmin;
      console.log(this.isAdmin);
      if (this.isAdmin) {
        this.fetchAllUsers();
      } else {
        this.users = null;
      }
    });
  }

  private async fetchAllUsers() {
    this.userService.getAllUsers().subscribe((value) => {
      this.users = value.users.concat([{id: 'Temp', displayName: 'XD', isAdmin: true, email: 'test@gmail.com'}]);
      console.log('Finished users');
    });
  }


}
