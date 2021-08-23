import {TestBed} from '@angular/core/testing';

import {AnalyzedImagesService} from './analyzed-images.service';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {MediaStorageService} from '../../apis/mediaStorage.service';
import {AnalysisService} from '../../apis/analysis.service';

describe('AnalyzedImagesService', () => {
  let service: AnalyzedImagesService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [MediaStorageService, AnalysisService]
    });
    service = TestBed.inject(AnalyzedImagesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
