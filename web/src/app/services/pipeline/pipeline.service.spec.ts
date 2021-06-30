import { TestBed } from '@angular/core/testing';

import { PipelineService } from './pipeline.service';

describe('PipelineService', () => {
  let service: PipelineService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PipelineService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
