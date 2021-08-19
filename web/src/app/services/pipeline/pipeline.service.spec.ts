import {TestBed} from '@angular/core/testing';

import {PipelineService} from './pipeline.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {PipelinesService} from '../../apis/pipelines.service';

describe('PipelineService', () => {
  let service: PipelineService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [PipelinesService],
    });
    service = TestBed.inject(PipelineService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
