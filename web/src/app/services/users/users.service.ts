import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {User} from '../../models/user';
import {UserService} from '../../apis/user.service';
import {SnotifyService} from 'ng-snotify';
import {PipelineService} from '../pipeline/pipeline.service';
import {VideosService} from '../videos/videos.service';
import {ImagesService} from '../images/images.service';
import {AnalyzedImagesService} from '../analyzed-images/analyzed-images.service';
import {AnalyzedVideosService} from '../analyzed-videos/analyzed-videos.service';
import {UnreviewedTool} from '../../models/unreviewedTool';
import {ToolsService} from '../../apis/tools.service';
import {UserToolsService} from '../user-tools/user-tools.service';

@Injectable({
  providedIn: 'root'
})
export class UsersService {


  private readonly _users = new BehaviorSubject<User[]>([]);
  // eslint-disable-next-line @typescript-eslint/member-ordering,no-underscore-dangle
  readonly users$ = this._users.asObservable();
  private isAdmin = false;

  private readonly _unreviewedTools = new BehaviorSubject<UnreviewedTool[]>([]);
  // eslint-disable-next-line @typescript-eslint/member-ordering,no-underscore-dangle
  readonly unreviewedTools$ = this._unreviewedTools.asObservable();

  constructor(private userService: UserService, private snotifyService: SnotifyService,
              private pipelineService: PipelineService, private videosService: VideosService,
              private imagesService: ImagesService, private analyzedImagesService: AnalyzedImagesService,
              private analyzedVideosService: AnalyzedVideosService, private toolsService: ToolsService,
              private userToolsService: UserToolsService) {
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

  get unreviewedTools(): UnreviewedTool[] {
    // eslint-disable-next-line no-underscore-dangle
    return this._unreviewedTools.getValue();
  }

  set unreviewedTools(val: UnreviewedTool[]) {
    // eslint-disable-next-line no-underscore-dangle
    this._unreviewedTools.next(val);
  }

  public async purgeMedia(id: string) {
    if (this.isAdmin) {
      this.userService.deleteMedia({id}, 'response').subscribe((res) => {
        if (res.ok) {
          this.snotifyService.success('Successfully purged media of : ' + this.users.find(
            value => value.id === id).displayName, 'Media Purge');
        } else {
          this.snotifyService.error(`Error occurred while purging media, please contact an admin`, 'Media Purge');
        }
      });

    } else {
      this.snotifyService.error(`Error occurred while purging media, please contact an admin`, 'Media Purge');
    }
  }

  public async upgradeToAdmin(id: string) {
    const user = this.users.find(value => value.id === id);
    if (user) {
      const index = this.users.indexOf(user);
      this.users[index] = {
        ...user,
        isAdmin: true
      };
      this.users = [...this.users];
      if (this.isAdmin) {
        this.userService.upgradeToAdmin({id}, 'response').subscribe((res) => {
          if (res.ok) {
            this.snotifyService.success('Successfully upgraded : ' + user.displayName + ' to admin', 'User Upgrade');
          } else {
            this.snotifyService.error(`Error occurred while upgrading user to admin, please contact an admin`, 'User Upgrade');
            this.users[index] = {
              ...user,
              isAdmin: user.isAdmin
            };
          }
        });
      } else {
        this.snotifyService.error(`Error occurred while upgrading user to admin, please contact an admin`, 'User Upgrade');
      }
    } else {
      this.snotifyService.error(`User doesn't exist anymore, please contact an admin id this is a mistake`, 'User Upgrade');
    }
  }

  public async rejectTool(tool: UnreviewedTool) {
    if (this.isAdmin) {
      const unreviewedTool = this.unreviewedTools.find(t => t === tool);
      this.unreviewedTools = this.unreviewedTools.filter(t => t !== unreviewedTool);
      try {
        this.toolsService.rejectTool({toolId: tool.toolId, toolOwnerId: tool.userId}, 'response').subscribe((res) => {
          if (res.ok) {
            this.snotifyService.success('Successfully rejected tool', 'Tool Rejection');
            this.userToolsService.userTools = this.userToolsService.userTools.filter(t => t.toolId !== tool.toolId);
          } else {
            this.unreviewedTools = [...this.unreviewedTools, unreviewedTool];
          }
        });
      } catch (e) {

      }
    }
  }

  public async approveTool(tool: UnreviewedTool) {
    if (this.isAdmin) {
      const unreviewedTool = this.unreviewedTools.find(t => t === tool);
      this.unreviewedTools = this.unreviewedTools.filter(t => t !== unreviewedTool);
      try {
        this.toolsService.approveTool({toolId: tool.toolId, toolOwnerId: tool.userId}, 'response').subscribe((res) => {
          if (res.ok) {
            this.snotifyService.success('Successfully approved tool', 'Tool Approval');
            const tool1 = this.userToolsService.userTools.filter(t => t.toolId === tool.toolId)[0];
            const index = this.userToolsService.userTools.indexOf(tool1);
            this.userToolsService.userTools[index] = {
              ...tool1,
              isApproved: true
            };
          } else {
            this.unreviewedTools = [...this.unreviewedTools, unreviewedTool];
          }
        });
      } catch (e) {

      }
    }
  }

  public async fetchAllUnreviewedTools() {
    this.toolsService.getUnreviewedTools().subscribe((res) => {
      this.unreviewedTools = res.unreviewedTools;
    });
  }

  public async revokeAdmin(id: string) {
    const user = this.users.find(value => value.id === id);
    if (user) {
      const index = this.users.indexOf(user);
      this.users[index] = {
        ...user,
        isAdmin: false
      };
      if (this.isAdmin) {
        this.userService.revokeAdmin({id}, 'response').subscribe((res) => {
          if (res.ok) {
            this.snotifyService.success('Successfully revoked : ' + user.displayName + `'s admin privileges`, 'Admin Revocation');
          } else {
            this.users[index] = {
              ...user,
              isAdmin: user.isAdmin
            };
            this.snotifyService.error(`Error occurred while revoking user admin privileges, please contact an admin`, 'Admin Revocation');
          }
        });
      } else {
        this.snotifyService.error(`Error occurred while revoking user admin privileges, please contact an admin`, 'Admin Revocation');
      }
    }
  }

  public async purgeOwnMedia() {
    this.userService.deleteOwnMedia('response').subscribe((res) => {
      if (res.ok) {
        this.removeAllMedia();
        this.snotifyService.success(`Successfully purged own media`, 'Own Media Purge');
      } else {
        this.snotifyService.error(`Error occurred while purging own media, please contact an admin`, 'Own Media Purge');
      }
    });
  }


  private async queryIsAdmin() {
    this.userService.isAdmin().subscribe((value) => {
      this.isAdmin = value.isAdmin;
      if (this.isAdmin) {
        this.fetchAllUsers();
        this.fetchAllUnreviewedTools();
      } else {
        this.users = null;
      }
    });
  }


  private async fetchAllUsers() {
    this.userService.getAllUsers().subscribe((value) => {
      this.users = value.users;
    });
  }

  private async removeAllMedia() {
    this.pipelineService.pipelines = [];
    this.pipelineService.tools = [];
    this.videosService.videos = [];
    this.imagesService.images = [];
    this.analyzedImagesService.analyzedImages = [];
    this.analyzedVideosService.analyzeVideos = [];
  }


}
