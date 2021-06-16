import { TestBed } from '@angular/core/testing';

import { VideouploadService } from './videoupload.service';

describe('VideouploadService', () => {
  let service: VideouploadService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(VideouploadService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
