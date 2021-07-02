import { TestBed } from '@angular/core/testing';

import { PipelineHandlerService } from './pipeline-handler.service';

describe('PipelineHandlerService', () => {
  let service: PipelineHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PipelineHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
