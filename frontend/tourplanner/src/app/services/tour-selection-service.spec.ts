import { TestBed } from '@angular/core/testing';

import { TourSelectionService } from './tour-selection-service';

describe('TourSelectionService', () => {
  let service: TourSelectionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TourSelectionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
