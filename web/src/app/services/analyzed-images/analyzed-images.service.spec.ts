import { TestBed } from '@angular/core/testing';

import { AnalyzedImagesService } from './analyzed-images.service';

describe('AnalyzedImagesService', () => {
  let service: AnalyzedImagesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AnalyzedImagesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
