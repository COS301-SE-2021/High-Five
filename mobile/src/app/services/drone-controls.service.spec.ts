import { TestBed } from '@angular/core/testing';

import { DroneControlsService } from './drone-controls.service';

describe('DroneControlsService', () => {
  let service: DroneControlsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DroneControlsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
