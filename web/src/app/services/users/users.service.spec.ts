import {TestBed} from '@angular/core/testing';

import {UsersService} from './users.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {UserService} from '../../apis/user.service';

describe('UsersService', () => {
  let service: UsersService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [UserService]
    });
    service = TestBed.inject(UsersService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
