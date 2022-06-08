import { TestBed } from '@angular/core/testing';

import { BcmService } from './bcm.service';

describe('BcmService', () => {
  let service: BcmService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BcmService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
