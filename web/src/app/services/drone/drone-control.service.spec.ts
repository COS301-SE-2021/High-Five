import { TestBed } from '@angular/core/testing';

import { DroneControlService } from './drone-control.service';

describe('DroneControlService', () => {
  let service: DroneControlService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DroneControlService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
