import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {User} from '../../models/user';
import {UserService} from '../../apis/user.service';
import {SnotifyService} from 'ng-snotify';

@Injectable({
  providedIn: 'root'
})
export class UsersService {


  private readonly _users = new BehaviorSubject<User[]>([]);
  // eslint-disable-next-line @typescript-eslint/member-ordering,no-underscore-dangle
  readonly users$ = this._users.asObservable();
  private isAdmin = false;

  constructor(private userService: UserService, private snotifyService: SnotifyService) {
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


}
