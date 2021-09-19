import { TestBed } from '@angular/core/testing';

import { LiveStreamingService } from './live-streaming.service';

describe('LiveStreamingService', () => {
  let service: LiveStreamingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LiveStreamingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
