import { TestBed } from '@angular/core/testing';

import { UserToolsService } from './user-tools.service';

describe('UserToolsService', () => {
  let service: UserToolsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserToolsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
