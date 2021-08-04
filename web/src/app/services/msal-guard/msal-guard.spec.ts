import { TestBed } from '@angular/core/testing';

import { MsalGuard } from './msal-guard';

describe('MsalGuardService', () => {
  let service: MsalGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MsalGuard);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
