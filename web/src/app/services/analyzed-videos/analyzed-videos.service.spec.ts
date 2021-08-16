import { TestBed } from '@angular/core/testing';

import { AnalyzedVideosService } from './analyzed-videos.service';

describe('AnalyzedVideosService', () => {
  let service: AnalyzedVideosService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AnalyzedVideosService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
