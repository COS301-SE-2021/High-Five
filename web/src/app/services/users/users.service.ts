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
      await this.userService.deleteMedia({id}).toPromise();
    }
  }

  public async upgradeToAdmin(id: string) {
    if (this.isAdmin) {
      await this.userService.upgradeToAdmin({id}).toPromise();
    }
  }

  public async purgeOwnMedia() {
    await this.userService.deleteOwnMedia().toPromise();
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
