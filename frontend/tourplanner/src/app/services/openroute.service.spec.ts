import { TestBed } from '@angular/core/testing';

import { Openroute } from './openroute';

describe('Openroute', () => {
  let service: Openroute;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Openroute);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
