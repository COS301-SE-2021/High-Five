import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserPreferencesService {

  private readonly _mediaFilter = new BehaviorSubject<string>('all');
  // eslint-disable-next-line @typescript-eslint/member-ordering,no-underscore-dangle
  readonly mediaFilter$ = this._mediaFilter.asObservable();

  constructor() {
    this.mediaFilter = 'all';
  }

  get mediaFilter(): string {
    // eslint-disable-next-line no-underscore-dangle
    return this._mediaFilter.getValue();
  }

  set mediaFilter(val: string) {
    // eslint-disable-next-line no-underscore-dangle
    this._mediaFilter.next(val);
  }
}
