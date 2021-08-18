import {TestBed} from '@angular/core/testing';

import {AnalyzedVideosService} from './analyzed-videos.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {AnalysisService} from '../../apis/analysis.service';
import {MediaStorageService} from '../../apis/mediaStorage.service';

describe('AnalyzedVideosService', () => {
  let service: AnalyzedVideosService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AnalysisService, MediaStorageService],
    });
    service = TestBed.inject(AnalyzedVideosService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
