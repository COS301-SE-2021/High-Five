import { TestBed } from '@angular/core/testing';

import { ScreenSizeServiceService } from './screen-size-service.service';

describe('ScreenSizeServiceService', () => {
  let service: ScreenSizeServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ScreenSizeServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
